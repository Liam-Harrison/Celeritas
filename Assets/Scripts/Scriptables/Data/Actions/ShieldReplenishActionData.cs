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

		public float coolDown;

		/// <inheritdoc/>
		public override Type ActionType => typeof(ShieldReplenishAction);

		public override string GetTooltip(int level)
		{
			return $"Instantly replenish your shields. Ability is on a <color=\"green\">{coolDown:0} second cooldown</color>. Additional levels decrease the cooldown.";
		}
	}
}
