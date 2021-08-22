using Celeritas.Game.Controllers;
using UnityEngine;

namespace Celeritas.AI
{
	/// <summary>
	/// A basic AI module which follows the player and keeps a specfic distance.
	/// </summary>
	public class AIBasicChase : AIBase
	{
		/// <summary>
		/// The range this AI will keep from the player.
		/// </summary>
		public float Range { get; set; } = 15f;

		/// <inheritdoc/>
		public override float Deadzone => 1f;

		protected virtual void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position + ShipEntity.Translation);
			Gizmos.DrawWireSphere(transform.position, Range);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, Goal);
		}

		/// <inheritdoc/>
		protected override void AIUpdate()
		{
			if (PlayerController.Instance == null)
				return;

			var player = PlayerController.Instance.PlayerShipEntity.Position;
			var delta = ShipEntity.Position - player;

			Goal = player + (delta.normalized * Range);
			Target = PlayerController.Instance.PlayerShipEntity.Position;

			foreach (var weapon in ShipEntity.WeaponEntities)
			{
				weapon.Firing = Vector3.Dot(weapon.Forward, (Target - weapon.Position).normalized) > 0.9f;
			}
		}
	}
}
