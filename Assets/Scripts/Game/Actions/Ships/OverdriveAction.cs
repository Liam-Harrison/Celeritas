using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using UnityEngine;
using System.Collections;

namespace Celeritas.Game.Actions
{
	public class OverdriveAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public OverdriveActionData OverdriveData { get; private set; }

		private float percentageToAdd;

		private float duration;

		protected override void Execute(Entity entity, int level)
		{
			//WeaponEntity weapon = (WeaponEntity)entity;
			var ship = entity as ShipEntity;

			percentageToAdd = 1 + (OverdriveData.attackSpeed + (OverdriveData.attackSpeedPerLevel * (level - 1)));

			duration = (OverdriveData.duration + (OverdriveData.durationPerLevel * (level - 1)));

			if (ship.PlayerShip == true)
			{
				for (int i = 0; i < ship.WeaponEntities.Count; i++)
				{
					ship.WeaponEntities[i].OverDrive(percentageToAdd, duration);
				}
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			OverdriveData = data as OverdriveActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}