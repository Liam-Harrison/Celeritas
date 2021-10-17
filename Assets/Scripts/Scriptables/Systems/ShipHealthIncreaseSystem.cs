using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Health Increase Modifier", menuName = "Celeritas/Modifiers/Health Increase")]
	public class ShipHealthIncreaseSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
	{

		[SerializeField, PropertyRange(0, 1), InfoBox("Percentage to add")]
		private float amount;

		[SerializeField, PropertyRange(0, 1), InfoBox("Percentage extra to add per level")]
		private float amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies health quantity
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much extra the health are increased per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Increase ship's health by <color=green>{(Amount + (AmountExtraPerLevel * level)) * 100:0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;

			float amountToAdd = 1 + amount + (wrapper.Level * amountExtraPerLevel);

			float shipCurrent = (float)ship.Health.CurrentValue;
			float shipMax = (float)ship.Health.MaxValue;
			float percentageToHeal = (shipCurrent / shipMax);

			//Updates the maximum health of the ship.
			ship.Health.MaxValue = ship.ShipData.StartingHealth * amountToAdd;

			//Calculates how much damage needs to be inflicted to heal the added health.
			float damageToInflict = ship.Health.CurrentValue - (ship.Health.MaxValue * percentageToHeal);

			ship.Health.Damage(damageToInflict);
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;

			float shiphealth = ship.Health.MaxValue;
			float amountToRemove = 1 + amount + (wrapper.Level * amountExtraPerLevel);

			float shipCurrent = (float)ship.Health.CurrentValue;
			float shipMax = (float)ship.Health.MaxValue;
			float percentageToDamage = (shipCurrent / shipMax);

			//Updates the maximum health of the ship.
			ship.Health.MaxValue = ship.Health.MaxValue / amountToRemove;

			//Calculates how much damage needs to be inflicted due to added health removal
			float damageToInflict = ship.Health.CurrentValue - (ship.ShipData.StartingHealth * percentageToDamage);

			ship.Health.Damage(damageToInflict);
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
		{
			// revert to base value
			var ship = entity as ShipEntity;

			float amountToRemove = 1 + amount + (previous * amountExtraPerLevel);

			float shipCurrent = (float)ship.Health.CurrentValue;
			float shipMax = (float)ship.Health.MaxValue;
			float percentageToDamage = (shipCurrent / shipMax);

			ship.Health.MaxValue /= amountToRemove;

			//Calculates how much damage needs to be inflicted due to added health removal
			float damageToInflict = ship.Health.CurrentValue - (ship.ShipData.StartingHealth * percentageToDamage);
			ship.Health.Damage(damageToInflict);

			// apply new level changes
			float amountToAdd = 1 + amount + (newLevel * amountExtraPerLevel);

			shipCurrent = (float)ship.Health.CurrentValue;
			shipMax = (float)ship.Health.MaxValue;
			float percentageToHeal = (shipCurrent / shipMax);

			ship.Health.MaxValue *= amountToAdd;

			//Calculates how much damage needs to be inflicted to heal the added health.
			damageToInflict = ship.Health.CurrentValue - (ship.Health.MaxValue * percentageToHeal);
			ship.Health.Damage(damageToInflict);
		}
	}
}