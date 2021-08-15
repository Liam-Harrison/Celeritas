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

	public class DamageResistanceSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{

		[SerializeField, PropertyRange(-100, 100), Title("Damage Taken Modifier Percentage")]
		private int amount;

		[SerializeField, PropertyRange(-100, 100), Title("Damage Taken Modifier Percentage added per level")]
		private int amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies damage taken
		/// </summary>
		public int Amount { get => amount; }

		/// <summary>
		/// How much the damage taken modifier increases per level
		/// </summary>
		public int AmountExtraPerLevel { get => amountExtraPerLevel; }

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"<color=green>▲</color> Reduces amount of damage taken by <color=green>{(Amount + (AmountExtraPerLevel * level)):0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			ship.damageModifierPercentage = (Amount + (AmountExtraPerLevel * level));
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			ship.damageModifierPercentage = 0;
		}
	}
}