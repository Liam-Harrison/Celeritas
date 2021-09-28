using UnityEngine;
using System.Collections;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;

namespace Celeritas.Game.Actions
{
	public class ImmortalityAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public ImmortalityActionData ImmortalityData { get; private set; }

		protected override void Execute(Entity entity)
		{
			var ship = entity as ShipEntity;

			if (ship.PlayerShip == true)
			{
				ship.Immortality(ImmortalityData.duration);
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			ImmortalityData = data as ImmortalityActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}