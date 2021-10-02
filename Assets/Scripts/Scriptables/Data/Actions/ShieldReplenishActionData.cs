using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// A teleport action.
	/// </summary>
	[CreateAssetMenu(fileName = "New Shield Replenish Action", menuName = "Celeritas/Actions/Shield Replenish")]
	public class ShieldReplenishActionData : ActionData
	{
		// Percentage cooldown is reduced by per level
		public float cooldownPerLevel;

		/// <inheritdoc/>
		public override Type ActionType => typeof(ShieldReplenishAction);

		public override string GetTooltip(int level)
		{
			return $"Instantly replenish your shields. Ability is on a <color=\"green\">{cooldownPerLevel:0} second cooldown</color>. Additional levels decrease the cooldown.";
		}
	}
}
