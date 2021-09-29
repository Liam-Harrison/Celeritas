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
		[PropertyRange(0, 100)]
		public float distance;

		[PropertyRange(0, 100)]
		public float distancePerLevel;

		/// <inheritdoc/>
		public override Type ActionType => typeof(TeleportAction);

		public override string GetTooltip(int level)
		{
			return $"Teleports you up to <color=\"green\">{distance:0}m</color> + (<color=\"green\">{distancePerLevel:0}m</color>) in the direction of your crossheir.";
		}
	}
}
