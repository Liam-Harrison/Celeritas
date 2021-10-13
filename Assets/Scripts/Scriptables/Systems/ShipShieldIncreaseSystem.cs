using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Shield Increase Modifier", menuName = "Celeritas/Modifiers/Shield Increase")]
	public class ShipShieldIncreaseSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
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

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Increase shield capacity by <color=green>{(Amount + (AmountExtraPerLevel * level)) * 100:0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			float amountToAdd = 1 + amount + (wrapper.Level * amountExtraPerLevel);

			float newValue = (ship.Shield.MaxValue * amountToAdd);
			ship.Shield.MaxValue = newValue;
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			float amountToAdd = 1 + amount + (wrapper.Level * amountExtraPerLevel);

			float newValue = (ship.Shield.MaxValue / amountToAdd);

			ship.Shield.MaxValue = newValue;
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
		{
			// revert to base value
			var ship = entity as ShipEntity;
			
			float amountToAdd = 1 + amount + (previous * amountExtraPerLevel);
			ship.Shield.MaxValue /= amountToAdd;

			// apply new level changes
			amountToAdd = 1 + amount + (newLevel * amountExtraPerLevel);
			ship.Shield.MaxValue *= amountToAdd;
		}
	}
}