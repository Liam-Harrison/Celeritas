using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Shield Increase Modifier", menuName = "Celeritas/Modifiers/Shield Increase")]
	public class ShipShieldIncreaseSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{

		[SerializeField, PropertyRange(0, 5), InfoBox("Percentage to add, 1 (default shield amount + 1 (value) = 2 (200% shield amount)")]
		private float amount;

		[SerializeField, PropertyRange(0, 2), InfoBox("Percentage extra to add per level")]
		private float amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies shield quantity
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much extra the shields are increased per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			// TODO: may need to update effect when system levels up, depending on how game loop works.
			// otherwise effects may not reflect levels

			var ship = entity as ShipEntity;
			float amountToAdd = amount + (level * amountExtraPerLevel);

			//ship.Shield += amountToAdd;

		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;

			//ship.Shield -= amount;
		}
	}
}
