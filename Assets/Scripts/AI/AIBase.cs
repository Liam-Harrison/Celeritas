using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.AI
{
	/// <summary>
	/// Base implemenation for all AI classes.
	/// </summary>
	[RequireComponent(typeof(ShipEntity))]
	public abstract class AIBase : MonoBehaviour
	{
		[SerializeField, TitleGroup("Steering")]
		private Steerer obstacle = new Steerer();

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public ShipEntity ShipEntity { get; private set; }

		/// <summary>
		/// The desired velocity of this ship.
		/// </summary>
		public Vector3 DesiredVelocity { get; protected set; }

		/// <summary>
		/// The target of this AI.
		/// </summary>
		public Vector3 AimTarget { get; protected set; }

		/// <summary>
		/// The found bounds of this AI.
		/// </summary>
		public Bounds Bounds { get; protected set; }

		/// <summary>
		/// The deadzone of this AIs locomotion.
		/// </summary>
		public abstract float Deadzone { get; }

		private ContactFilter2D filter = new ContactFilter2D();

		private readonly List<Steerer> steeringAgents = new List<Steerer>();

		protected virtual void Awake()
		{
			ShipEntity = GetComponent<ShipEntity>();
			filter.NoFilter();
			AddSteeringAgent(obstacle);
		}

		public void OnAttatched()
		{
			FetchBounds();
		}

		private void FetchBounds()
		{
			var collider = GetComponentInChildren<EdgeCollider2D>();
			Bounds = collider.bounds;
		}

		protected virtual void FixedUpdate()
		{
			if (EntityDataManager.Instance.Loaded)
			{
				AIUpdate();
				InternalAIUpdate();
			}
		}

		protected virtual void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			foreach (var steerer in steeringAgents)
			{
				Gizmos.DrawLine(transform.position, transform.position + steerer.DesiredVelocity);
			}

			var v2d = ShipEntity.Rigidbody.velocity;
			var vel = new Vector3(v2d.x, v2d.y, 0);
			var veln = vel.normalized;

			if (vel.magnitude > 0.5f)
			{
				var hits = Physics2D.CircleCastAll(ShipEntity.Position, Mathf.Max(Bounds.extents.x, Bounds.extents.y), veln, obstacle.Sight);

				Gizmos.color = Color.yellow;

				if (hits.Length > 0)
				{
					foreach (var hit in hits)
					{
						if (hit.transform.CompareTag("Obstacle"))
						{
							Gizmos.color = Color.green;
							break;
						}
					}
				}

				Gizmos.DrawWireSphere(ShipEntity.Position + (veln * obstacle.Sight), Mathf.Max(Bounds.extents.x, Bounds.extents.y));
			}
		}

		/// <summary>
		/// Update this AI with custom behaviour.
		/// </summary>
		protected abstract void AIUpdate();

		private void InternalAIUpdate()
		{
			var v2d = ShipEntity.Rigidbody.velocity;
			var vel = new Vector3(v2d.x, v2d.y, 0);
			var veln = vel.normalized;

			if (vel.magnitude > 0.5f)
			{
				var hits = Physics2D.CircleCastAll(ShipEntity.Position, Mathf.Max(Bounds.extents.x, Bounds.extents.y), veln, obstacle.Sight);
				if (hits.Length > 0)
				{
					foreach (var hit in hits)
					{
						if (hit.transform.CompareTag("Obstacle"))
						{
							if (TryFindDirection(out var dir))
							{
								var s = Vector3.ClampMagnitude(dir, obstacle.Force);
								obstacle.DesiredVelocity = s * obstacle.Scale;
								break;
							}
							else
							{
								obstacle.DesiredVelocity = Vector3.zero;
							}

							break;
						}
					}
				}
			}
			else
			{
				obstacle.DesiredVelocity = Vector3.zero;
			}

			var translation = Vector3.zero;
			foreach (var steerer in steeringAgents)
			{
				translation += steerer.DesiredVelocity;
			}

			ShipEntity.Translation = Vector3.ClampMagnitude(translation, 1);
			ShipEntity.AimTarget = AimTarget;
		}

		protected void AddSteeringAgent(Steerer steerer)
		{
			steeringAgents.Add(steerer);
		}

		private bool TryFindDirection(out Vector3 dir)
		{
			for (int r = 0; r <= 90; r += 15)
			{
				for (int sign = -1; sign <= 1; sign += 2)
				{
					var rot = Quaternion.Euler(0, 0, r * sign);

					var hits = Physics2D.CircleCastAll(ShipEntity.Position, Mathf.Max(Bounds.extents.x, Bounds.extents.y), rot * ShipEntity.Velocity.normalized, obstacle.Sight);
					bool valid = true;
					foreach (var hit in hits)
					{
						if (hit.transform.CompareTag("Obstacle"))
						{
							valid = false;
							break;
						}
					}

					if (valid)
					{
						dir = rot * ShipEntity.Velocity.normalized;
						return true;
					}
				}
			}
			dir = default;
			return false;
		}
	}
}
