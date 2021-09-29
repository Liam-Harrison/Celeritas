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

		float originalCooldown;

		protected override void Execute(Entity entity, int level)
		{

			var ship = entity as ShipEntity;

			if (ship.PlayerShip == true)
			{
				ship.Immortality(ImmortalityData.duration);
                ImmortalityData.CooldownSeconds = originalCooldown - ((originalCooldown / 100) * (ImmortalityData.cooldownPerLevel * level));
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			ImmortalityData = data as ImmortalityActionData;

			originalCooldown = ImmortalityData.CooldownSeconds;

			base.Initialize(data, isPlayer, owner);
		}
	}
}