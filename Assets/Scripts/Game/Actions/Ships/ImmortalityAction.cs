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

		float totalDuration;

		public Color gold = new Color(255f, 211f, 0f);

		protected override void Execute(Entity entity, int level)
		{

			var ship = entity as ShipEntity;

			totalDuration = ImmortalityData.duration + (ImmortalityData.durationPerLevel * (level - 1));

			if (ship.PlayerShip == true)
			{
				ship.Immortality(totalDuration);
				ship.ColorFlash(totalDuration, gold);
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			ImmortalityData = data as ImmortalityActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}