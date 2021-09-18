using Celeritas.Game.Controllers;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Celeritas.AI
{
	[System.Serializable]
	public class Steerer
	{
		[SerializeField, TitleGroup("Steering Force")]
		private float sight = 5f;

		[SerializeField, TitleGroup("Steering Force"), Range(1, 5)]
		private float force = 4f;

		[SerializeField, TitleGroup("Steering Force"), Range(-1, 1)]
		private float scale = 0.75f;

		public float Sight { get => sight; }

		public float Force { get => force; }

		public float Scale { get => scale; }

		public Vector3 DesiredVelocity { get; set; }
	}

	/// <summary>
	/// A basic AI module which follows the player and keeps a specfic distance.
	/// </summary>
	public class AIBasicChase : AIBase
	{
		[SerializeField, TitleGroup("Basic Chase Settings")]
		private Steerer approach;

		/// <summary>
		/// The range this AI will keep from the player.
		/// </summary>
		public float Range { get; set; } = 20f;

		/// <inheritdoc/>
		public override float Deadzone => 1f;

		protected override void Awake()
		{
			base.Awake();
			AddSteeringAgent(approach);
		}

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			Gizmos.color = Color.green;

			if (PlayerController.Instance == null)
				return;

			var player = PlayerController.Instance.PlayerShipEntity.Position;
			var goal = player + ((transform.position - player).normalized * Range);

			var v2d = ShipEntity.Rigidbody.velocity;
			var vel = new Vector3(v2d.x, v2d.y, 0);

			Gizmos.DrawSphere(goal, 1f);
			Gizmos.DrawLine(transform.position, transform.position + approach.DesiredVelocity);
		}

		/// <inheritdoc/>
		protected override void AIUpdate()
		{
			if (PlayerController.Instance == null)
				return;

			var player = PlayerController.Instance.PlayerShipEntity.Position;
			var goal = player + ((transform.position - player).normalized * Range);

			var v2d = ShipEntity.Rigidbody.velocity;
			var vel = new Vector3(v2d.x, v2d.y, 0);

			var d = goal - transform.position;

			if (d.magnitude < 5f)
			{
				approach.DesiredVelocity = Vector3.zero;
			}
			else
			{
				var s = Vector3.ClampMagnitude(d, approach.Force);
				approach.DesiredVelocity = s * approach.Scale;
			}

			AimTarget = PlayerController.Instance.PlayerShipEntity.Position;

			foreach (var weapon in ShipEntity.WeaponEntities)
			{
				weapon.Firing = Vector3.Dot(weapon.Forward, (AimTarget - weapon.Position).normalized) > 0.9f;
			}
		}
	}
}
