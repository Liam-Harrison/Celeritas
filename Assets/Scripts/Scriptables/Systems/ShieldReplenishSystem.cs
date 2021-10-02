using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Shield Replenish Modifier", menuName = "Celeritas/Modifiers/Shield Replenish")]
	public class ShieldReplenishSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		// Used to store the shield regen amount
		private float shipShieldRegen;

		/// <inheritdoc/>
		public override bool Stacks => false;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(ushort level) => $"Removes ships out of combat shield regeneration</color>.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			shipShieldRegen = ship.ShieldRegenAmount;
			ship.ShieldRegenAmount = 0;
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			ship.ShieldRegenAmount = shipShieldRegen;
		}
	}
}