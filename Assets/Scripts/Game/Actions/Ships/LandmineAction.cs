using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Game.Controllers;
using UnityEngine;
using System.Collections;

namespace Celeritas.Game.Actions
{
	public class LandmineAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public LandmineActionData LandmineData { get; private set; }

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;

			//if (LootController.instance != null)
			//{
			//if (LootController.instance.RareMetals >= NukeData.scrapMetalCost)
			//{
			//LootController.instance.SpendRareMetals(Mathf.RoundToInt(NukeData.scrapMetalCost));

			if (ship.PlayerShip == true)
			{
				ship.DeployMines(LandmineData.LandminePrefab, (LandmineData.damage + (LandmineData.damagePerLevel * level)), LandmineData.duration, (LandmineData.Amount + (LandmineData.AmountPerLevel * level)));
			}
			//}
			//else
			//{
			//	Debug.Log("Not enough rare metals.");
			//}
			//}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			LandmineData = data as LandmineActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}