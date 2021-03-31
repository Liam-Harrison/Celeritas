using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Ship Modifier", menuName = "Celeritas/Modifiers/Ship")]
	public class ShipSpeedSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		public override bool Stacks => true;

		public override SystemTargets Targets =>  SystemTargets.Ship;

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			ship.MovementModifier.SetForward(ship.MovementModifier.forward + 1f);
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			ship.MovementModifier.SetForward(ship.MovementModifier.forward - 1f);
		}
	}
}
