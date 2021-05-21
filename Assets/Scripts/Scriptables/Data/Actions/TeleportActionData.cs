using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// A teleport action.
	/// </summary>
	[CreateAssetMenu(fileName = "New Jump Action", menuName = "Celeritas/Actions/Jump")]
	public class TeleportActionData : ActionData
	{
		/// <summary>
		/// The distance this action will teleport the entity.
		/// </summary>
		[PropertyRange(0, 500)]
		public float distance;

		/// <inheritdoc/>
		public override Type ActionType => typeof(TeleportAction);
	}
}
