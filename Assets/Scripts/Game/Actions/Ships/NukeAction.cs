using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Game.Controllers;
using UnityEngine;
using System.Collections;

namespace Celeritas.Game.Actions
{
	public class NukeAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public NukeActionData NukeData { get; private set; }

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
						ship.DeployNuke(NukeData.nukePrefab, (NukeData.damage + (NukeData.damagePerLevel * level)), NukeData.duration);
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
			NukeData = data as NukeActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}