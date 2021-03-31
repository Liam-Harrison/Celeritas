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
		//private MovementModifierForModule shipSpeedModifier;
		private float shipSpeedModifier;

		[SerializeField]
		//private MovementModifierForModule addMovementModifier;
		private float addMovementPerLevel;

		//[SerializeField, Title("Ship Turning Speed")]
		//private float shipTurningSpeedModifier;

		//[SerializeField]
		//private float shipTurningSpeedAddPerLevel;

		/// <summary>
		/// How much the ship speed will be modified at level 0. (multiplier)
		/// </summary>
		public float ShipSpeedModifier {get => shipSpeedModifier; }
		/// <summary>
		/// How much the ship speed will increase per level
		/// </summary>
		public float AddMovementPerLevel { get => AddMovementPerLevel;  }

		//public float ShipTurningSpeedModifier { get => shipTurningSpeedModifier; }
		//public float ShipTurningSpeedAddPerLevel { get => shipTurningSpeedAddPerLevel; }

		/// <inheritdoc />
		public override bool Stacks => false;

		/// <inheritdoc />
		public override SystemTargets Targets => SystemTargets.Ship;

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			ShipEntity shipEntity = (ShipEntity)entity;
			/*float forward = shipSpeedModifier.forward + (level * addMovementModifier.forward);
			float side = shipSpeedModifier.side + (level * addMovementModifier.side);
			float back = shipSpeedModifier.back + (level * addMovementModifier.back);
			float rotation = shipSpeedModifier.rotation + (level * addMovementModifier.rotation);

			Vector3 toAddForce = shipEntity.Right * shipEntity.Translation.x * side *Time.smoothDeltaTime;
			toAddForce += shipEntity.Up * ((Mathf.Max(shipEntity.Translation.y, 0) * forward) +
							(Mathf.Min(shipEntity.Translation.y, 0) * back)) * Time.smoothDeltaTime;

			shipEntity.Rigidbody.AddForce(toAddForce);*/

			// todo: torque

			float forceMultiplier = shipSpeedModifier + (level * addMovementPerLevel);

			shipEntity.Rigidbody.AddForce(shipEntity.Velocity * forceMultiplier, ForceMode2D.Force);

			//float torqueMultiplier = shipTurningSpeedAddPerLevel + (level * shipTurningSpeedAddPerLevel);
			//shipEntity.
			//shipEntity.Rigidbody.AddTorque(shipEntity.Velocity * torqueMultiplier, ForceMode2D.Force);
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
