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
		public float durationPerLevel;

		/// <inheritdoc/>
		public override Type ActionType => typeof(ImmortalityAction);

		public override string GetTooltip(int level)
		{
			return $"Grants immortality for <color=\"green\">{duration + (durationPerLevel * (level - 1))} seconds</color> on a <color=\"green\">{CooldownSeconds:0} second cooldown</color>. Further upgrades increase the duration of the ability.";
		}
	}
}
