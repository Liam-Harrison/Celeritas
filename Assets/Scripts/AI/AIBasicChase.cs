using Celeritas.Game.Controllers;
using UnityEngine;

namespace Celeritas.AI
{
	public class AIBasicChase : AIBase
	{
		/// <summary>
		/// The range this AI will keep from the player.
		/// </summary>
		public const float RANGE = 5f;

		private Vector3 goal;

		protected virtual void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position + ShipEntity.Translation);
			Gizmos.DrawWireSphere(transform.position, RANGE);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, goal);
		}

		protected override void AIUpdate()
		{
			var delta = transform.position - PlayerController.Instance.transform.position;
			if (delta.magnitude > RANGE)
			{
				goal = PlayerController.Instance.transform.position + (delta.normalized * RANGE);
				ShipEntity.Translation = (goal - transform.position).normalized;
			}
			else
			{
				ShipEntity.Translation = Vector3.zero;
			}

			ShipEntity.Target = PlayerController.Instance.transform.position;
		}
	}
}
