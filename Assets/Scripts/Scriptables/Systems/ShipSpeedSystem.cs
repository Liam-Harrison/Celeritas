using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// Marks certain ship directions.
	/// </summary>
	[System.Flags]
	public enum ShipDirections
	{
		None = 0,
		Forward = 1,
		Side = 2,
		Back = 4,
	}

	[CreateAssetMenu(fileName = "New Ship Speed Modifier", menuName = "Celeritas/Modifiers/Ship Speed")]
	public class ShipSpeedSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
	{
		[SerializeField, Title("Speed System")]
		private ShipDirections affectedDirections;

		[SerializeField, PropertyRange(0, 5), InfoBox("Percentage to add, 1 (default ship speed) + 1 (value) = 2 (200% ship speed)")]
		private float amount;

		[SerializeField, PropertyRange(0, 2), InfoBox("Percentage extra to add per level")]
		private float amountExtraPerLevel;

		/// <summary>
		/// Get the directions this effect manipulates.
		/// </summary>
		public ShipDirections AffectDirections { get => affectedDirections; }

		/// <summary>
		/// The amount this system modifies the ship directions.
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much extra the ship speed is modifier per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets =>  SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Increase {AffectDirections} speed by <color=green>{(Amount + (AmountExtraPerLevel * level)) * 100:0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{

			var ship = entity as ShipEntity;
			float amountToAdd = amount + (wrapper.Level * amountExtraPerLevel);

			if (affectedDirections.HasFlag(ShipDirections.Forward))
				ship.MovementModifier.Forward += amountToAdd;

			if (affectedDirections.HasFlag(ShipDirections.Side))
				ship.MovementModifier.Side += amountToAdd;

			if (affectedDirections.HasFlag(ShipDirections.Back))
				ship.MovementModifier.Back += amountToAdd;
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			float amountToSubtract = amount + (wrapper.Level * amountExtraPerLevel);

			if (affectedDirections.HasFlag(ShipDirections.Forward))
				ship.MovementModifier.Forward -= amountToSubtract;

			if (affectedDirections.HasFlag(ShipDirections.Side))
				ship.MovementModifier.Side -= amountToSubtract;

			if (affectedDirections.HasFlag(ShipDirections.Back))
				ship.MovementModifier.Back -= amountToSubtract;
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
		{
			var ship = entity as ShipEntity;
			float amountToModify = (newLevel - previous) * amountExtraPerLevel;

			if (affectedDirections.HasFlag(ShipDirections.Forward))
				ship.MovementModifier.Forward += amountToModify;

			if (affectedDirections.HasFlag(ShipDirections.Side))
				ship.MovementModifier.Side += amountToModify;

			if (affectedDirections.HasFlag(ShipDirections.Back))
				ship.MovementModifier.Back += amountToModify;
		}
	}
}
