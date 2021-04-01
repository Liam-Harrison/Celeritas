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
	[CreateAssetMenu(fileName = "New ShipSpeed Modifier", menuName = "Celeritas/Modifiers/Ship Speed")]
	public class ModifyShipSpeedSystem : ModifierSystem, IEntityUpdated
	{

		[SerializeField, Title("Ship Speed")]
		//private MovementModifier shipSpeedModifier;
		private float shipSpeedModifier;

		[SerializeField]
		//private MovementModifier addMovementModifier;
		private float addMovementPerLevel;

		//[SerializeField, Title("Ship Turning Speed")]
		//private float shipTurningSpeedModifier;

		//[SerializeField]
		//private float shipTurningSpeedAddPerLevel;

		/// <summary>
		/// How much the ship speed will be modified at level 0. (adds)
		/// </summary>
		public float ShipSpeedModifier {get => shipSpeedModifier; }
		/// <summary>
		/// How much the ship speed will increase per level
		/// </summary>
		public float AddMovementModifier { get => addMovementPerLevel;  }

		//public float ShipTurningSpeedModifier { get => shipTurningSpeedModifier; }
		//public float ShipTurningSpeedAddPerLevel { get => shipTurningSpeedAddPerLevel; }

		/// <inheritdoc />
		public override bool Stacks => false;

		/// <inheritdoc />
		public override SystemTargets Targets => SystemTargets.Ship;

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			ShipEntity shipEntity = (ShipEntity)entity;

			float forceMultiplier = shipSpeedModifier + (level * addMovementPerLevel);

			shipEntity.Rigidbody.AddForce(shipEntity.Velocity * forceMultiplier, ForceMode2D.Force);

			// below is for allowing modification of each aspect of motion individually (doesn't seem to work though)
			
			/*	
			MovementModifier toApply = new MovementModifier {
				back = shipEntity.BaseMovementModifier.back + shipSpeedModifier.back + (level * addMovementModifier.back),
				forward = shipEntity.BaseMovementModifier.forward + shipSpeedModifier.forward + (level * addMovementModifier.forward),
				rotation = shipEntity.BaseMovementModifier.rotation + shipSpeedModifier.rotation + (level * addMovementModifier.rotation),
				side = shipEntity.BaseMovementModifier.side + shipSpeedModifier.side + (level * addMovementModifier.side)
			};
			// needs to not over ride the ship's original movement modifiers.
			shipEntity.MovementModifiers = toApply;
			*/

		}

		/// <summary>
		/// For modifying the ship's movement speed via modules
		/// Point of difference from MovementModifier: can go negative
		/// </summary>
		[System.Serializable]
		public struct MovementModifierForModule
		{
			[PropertyRange(-2, 2), Title("Movement Modifiers")]
			public float forward;

			[PropertyRange(-2, 2)]
			public float side;

			[PropertyRange(-2, 2)]
			public float back;

			[PropertyRange(-2, 2)]
			public float rotation;
		}
	}
}
