using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// Reduces incoming damage by set percentage from all sources.
	/// </summary>

	[CreateAssetMenu(fileName = "New Ship Damage Resistance Modifier", menuName = "Celeritas/Modifiers/Ship Damage Resistance")]

	public class DamageResistanceSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
	{

		[SerializeField, PropertyRange(-1.0f, 1.0f), Title("Damage Taken Modifier Percentage", "Zero = no mutiplier, -1 = 100% less damage, 1 = 100% more damage.")]
		private float amount;

		[SerializeField, PropertyRange(-1.0f, 1.0f), Title("Damage Taken Modifier Percentage added per level", "Amount given on top of the base amount.")]
		private float amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies damage taken
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much the damage taken modifier increases per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(int level) => $"Reduces amount of damage taken by <color=green>{(Amount + (AmountExtraPerLevel * level))*100:0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			ship.DamageModifierPercentage = (Amount + (AmountExtraPerLevel * wrapper.Level));
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			ship.DamageModifierPercentage = (ship.DamageModifierPercentage - (Amount + (AmountExtraPerLevel * wrapper.Level)));
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
		{
			// revert to base value
			var ship = entity as ShipEntity;

			ship.DamageModifierPercentage = (Amount + (AmountExtraPerLevel * newLevel));
			Debug.Log("Amount: " + Amount);
			Debug.Log("Amount Extra: " + AmountExtraPerLevel);
			Debug.Log("Level: " + newLevel);
			Debug.Log(ship.DamageModifierPercentage);
		}
	}
}