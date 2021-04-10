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
		public float Range { get; set; } = 4f;

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
			var player = PlayerController.Instance.ShipEntity.Position;
			var delta = ShipEntity.Position - player;

			Goal = player + (delta.normalized * Range);
			Target = PlayerController.Instance.ShipEntity.Position;
		}
	}
}
