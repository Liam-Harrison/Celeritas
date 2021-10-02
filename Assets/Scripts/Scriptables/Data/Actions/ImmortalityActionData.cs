using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Temporarily take no damage from all sources
	/// </summary>
	[CreateAssetMenu(fileName = "New Immortality Action", menuName = "Celeritas/Actions/Immortality")]
	public class ImmortalityActionData : ActionData
	{
		/// <summary>
		/// How long this will last
		/// </summary>
		[SerializeField]
		public float duration;

		[SerializeField]
		public float cooldownPerLevel;

		/// <inheritdoc/>
		public override Type ActionType => typeof(ImmortalityAction);

		public override string GetTooltip(int level)
		{
			return $"Grants immortality for <color=\"green\">{duration:0} seconds</color> on a <color=\"green\">{CooldownSeconds - ((CooldownSeconds/100) * (cooldownPerLevel * level)):0} cooldown</color>. Further upgrades reduce the cooldown of ability.";
		}
	}
}
