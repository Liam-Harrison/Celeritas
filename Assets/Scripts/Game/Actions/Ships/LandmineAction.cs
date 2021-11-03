using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Celeritas.Game.Entities;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Actions
{
	public class LandmineAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public LandmineActionData LandmineData { get; private set; }

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;

			if (ship.PlayerShip == true)
			{
				ship.DeployMines(LandmineData.LandminePrefab, (LandmineData.damage + (LandmineData.damagePerLevel * level)), LandmineData.duration, (LandmineData.Amount + (LandmineData.AmountPerLevel * level)));
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			LandmineData = data as LandmineActionData;

			RareMetalCost = LandmineData.RareMetalCost;

			base.Initialize(data, isPlayer, owner);
		}
	}
}