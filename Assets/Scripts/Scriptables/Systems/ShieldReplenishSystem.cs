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
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Removes ships out of combat shield regeneration.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			// TODO: may need to update effect when system levels up, depending on how game loop works.
			// otherwise effects may not reflect levels

			var ship = entity as ShipEntity;
			shipShieldRegen = ship.ShieldRegenAmount;
			ship.ShieldRegenAmount = 0;
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			ship.ShieldRegenAmount = shipShieldRegen;
		}
	}
}