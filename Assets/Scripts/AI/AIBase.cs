using Celeritas.Game.Entities;
using UnityEngine;

namespace Celeritas.AI
{
	/// <summary>
	/// Base implemenation for all AI classes.
	/// </summary>
	[RequireComponent(typeof(ShipEntity))]
	public abstract class AIBase : MonoBehaviour
	{
		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public ShipEntity ShipEntity { get; private set; }

		/// <summary>
		/// The position goal of this AI.
		/// </summary>
		public Vector3 Goal { get; protected set; }

		/// <summary>
		/// The target of this AI.
		/// </summary>
		public Vector3 Target { get; protected set; }

		/// <summary>
		/// The deadzone of this AIs locomotion.
		/// </summary>
		public abstract float Deadzone { get; }

		protected virtual void Awake()
		{
			ShipEntity = GetComponent<ShipEntity>();
		}

		protected virtual void FixedUpdate()
		{
			if (EntityDataManager.Instance.Loaded)
			{
				AIUpdate();
				InternalAIUpdate();
			}
		}

		/// <summary>
		/// Update this AI with custom behaviour.
		/// </summary>
		protected abstract void AIUpdate();

		private void InternalAIUpdate()
		{
			var gDelta = Goal - transform.position;
			var gMag = gDelta.magnitude;

			if (gMag > Deadzone)
			{
				var gDir = gDelta.normalized;
				var fwd = Vector3.Dot(ShipEntity.Forward, gDir);
				var right = Vector3.Dot(ShipEntity.Right, gDir);

				ShipEntity.Translation = ((Vector3.up * fwd) + (Vector3.right * right)).normalized;
			}
			else
			{
				ShipEntity.Translation = Vector3.zero;
			}

			ShipEntity.Target = Target;
		}
	}
}
