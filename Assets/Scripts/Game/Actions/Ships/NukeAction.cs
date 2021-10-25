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

			if (ship.PlayerShip == true)
			{
				ship.DeployNuke(NukeData.nukePrefab, (NukeData.damage + (NukeData.damagePerLevel * level)), NukeData.duration);
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			NukeData = data as NukeActionData;

			RareMetalCost = NukeData.RareMetalCost;

			base.Initialize(data, isPlayer, owner);
		}
	}
}