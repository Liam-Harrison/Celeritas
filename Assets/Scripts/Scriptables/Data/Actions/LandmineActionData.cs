using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Deploys some landmines.
	/// </summary>
	[CreateAssetMenu(fileName = "New landmine Action", menuName = "Celeritas/Actions/landmine")]
	public class LandmineActionData : ActionData
	{
		[SerializeField]
		public float duration;

		[SerializeField]
		public float damage;

		[SerializeField]
		public float damagePerLevel;

		[SerializeField]
		public float Amount;

		[SerializeField]
		public float AmountPerLevel;

		[SerializeField]
		public float scrapMetalCost;

		[SerializeField]
		public GameObject LandminePrefab;

		/// <inheritdoc/>
		public override Type ActionType => typeof(LandmineAction);

		public override string GetTooltip(int level)
		{
			return $"Convert <color=\"green\">{scrapMetalCost:0}</color> amount of Scrap Metal into Scrap Mines dealing <color=\"green\">{damage + (damagePerLevel * level)}</color> damage per mine.";
		}
	}
}