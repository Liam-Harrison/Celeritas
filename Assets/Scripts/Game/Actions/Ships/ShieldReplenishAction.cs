using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using UnityEngine;
using System.Collections;

namespace Celeritas.Game.Actions
{
	public class ShieldReplenishAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public ShieldReplenishActionData ShieldReplenishData { get; private set; }

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;

			if (ship.PlayerShip == true)
			{
				float damageToHeal = ship.Shield.MaxValue - ship.Shield.CurrentValue;
				ship.TakeDamage(ship, -damageToHeal);
				//Debug.Log("Shields replenished: " + damageToHeal);
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			ShieldReplenishData = data as ShieldReplenishActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}

