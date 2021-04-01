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
	public class ShipSpeedSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, Title("Speed System")]
		private ShipDirections affectedDirections;

		[SerializeField, PropertyRange(0, 4), InfoBox("Percentage to add, 1 (default ship speed) + 1 (value) = 2 (200% ship speed)")]
		private float amount;

		/// <summary>
		/// Get the directions this effect manipulates.
		/// </summary>
		public ShipDirections AffectDirections { get => affectedDirections; }

		/// <summary>
		/// The amount this system modifies the ship directions.
		/// </summary>
		public float Amount { get => amount; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets =>  SystemTargets.Ship;

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;

			if (affectedDirections.HasFlag(ShipDirections.Forward))
				ship.MovementModifier.Forward += amount;

			if (affectedDirections.HasFlag(ShipDirections.Side))
				ship.MovementModifier.Side += amount;

			if (affectedDirections.HasFlag(ShipDirections.Back))
				ship.MovementModifier.Back += amount;
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;

			if (affectedDirections.HasFlag(ShipDirections.Forward))
				ship.MovementModifier.Forward -= amount;

			if (affectedDirections.HasFlag(ShipDirections.Side))
				ship.MovementModifier.Side -= amount;

			if (affectedDirections.HasFlag(ShipDirections.Back))
				ship.MovementModifier.Back -= amount;
		}
	}
}
