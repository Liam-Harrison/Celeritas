using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// A teleport action.
	/// </summary>
	[CreateAssetMenu(fileName = "New Overdrive Action", menuName = "Celeritas/Actions/Overdrive")]
	public class OverdriveActionData : ActionData
	{
		public float duration;

		public float durationPerLevel;

		public float attackSpeed;

		public float attackSpeedPerLevel;

		/// <inheritdoc/>
		public override Type ActionType => typeof(OverdriveAction);

		public override string GetTooltip(int level)
		{
			return $"Temporarily increases attack speed by (<color=\"green\">{((attackSpeed + (attackSpeedPerLevel * level))*100)}%</color>) for <color=\"green\">{duration + (durationPerLevel * level):0} seconds</color>.";
		}
	}
}
