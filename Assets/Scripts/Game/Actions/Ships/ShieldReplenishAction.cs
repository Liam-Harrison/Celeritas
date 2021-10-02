using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;
using System.Collections;

namespace Celeritas.Game.Actions
{
	public class ShieldReplenishAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public ShieldReplenishActionData ShieldReplenishData { get; private set; }

		float originalCooldown;

		float currentCooldown;

		int levelCounter;

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;
			levelCounter = level;
			currentCooldown = originalCooldown;

			if (ship.PlayerShip == true)
			{
				float damageToHeal = ship.Shield.MaxValue - ship.Shield.CurrentValue;
				ship.TakeDamage(ship, -damageToHeal);

				if (level > 1)
				{
					while (levelCounter > 0)
					{
						currentCooldown = currentCooldown - ((currentCooldown / 100) * ShieldReplenishData.cooldownPercentagePerLevel);
						levelCounter = levelCounter - 1;
					}
				}

				ShieldReplenishData.CooldownSeconds = currentCooldown;
				//ShieldReplenishData.CooldownSeconds = originalCooldown - ((originalCooldown / 100) * (ShieldReplenishData.cooldownPercentagePerLevel * level));
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			ShieldReplenishData = data as ShieldReplenishActionData;

			originalCooldown = ShieldReplenishData.CooldownSeconds;

			base.Initialize(data, isPlayer, owner);
		}
	}
}

