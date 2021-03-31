using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// contains the instanced information for a modifier that will change a ship's movement speed
	/// </summary>
	[CreateAssetMenu(fileName = "New ShipSpeed Modifier", menuName= "Celeritas/Modifiers/Ship Speed")]
	public class ModifyShipSpeedSystem : ModifierSystem, IEntityUpdated
	{

		[SerializeField, Title("Ship Speed")]
		private float shipSpeedModifier;

		[SerializeField]
		private float addMovementPerLevel;

		/// <summary>
		/// How much the ship speed will be modified at level 0. (multiplier)
		/// </summary>
		public float ShipSpeedModifier { get => shipSpeedModifier; }
		/// <summary>
		/// How much the ship speed will increase per level
		/// </summary>
		public float AddMovementPerLevel { get => AddMovementPerLevel;  }

		/// <inheritdoc />
		public override bool Stacks => false;

		/// <inheritdoc />
		public override SystemTargets Targets => SystemTargets.Ship;

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			ShipEntity shipEntity = (ShipEntity)entity;

			float forceMultiplier = shipSpeedModifier + (level * addMovementPerLevel);

			shipEntity.Rigidbody.AddForce(shipEntity.Velocity * forceMultiplier, ForceMode2D.Force);

		}
	}
}
