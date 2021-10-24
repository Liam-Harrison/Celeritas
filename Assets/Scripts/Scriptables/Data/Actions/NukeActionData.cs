using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Creates a nuke that goes KABOOM!.
	/// </summary>
	[CreateAssetMenu(fileName = "New Nuke Action", menuName = "Celeritas/Actions/Nuke")]
	public class NukeActionData : ActionData
	{
		[SerializeField]
		public float duration;

		[SerializeField]
		public float damage;

		[SerializeField]
		public float damagePerLevel;

		[SerializeField]
		public float scrapMetalCost;

		[SerializeField]
		public GameObject nukePrefab;

		/// <inheritdoc/>
		public override Type ActionType => typeof(NukeAction);

		public override string GetTooltip(int level)
		{
			return $"Convert <color=\"green\">{scrapMetalCost:0}</color> amount of Scrap Metal into unstable quantum explosion dealing <color=\"green\">{damage + (damagePerLevel * level)}</color> damage.";
		}
	}
}
