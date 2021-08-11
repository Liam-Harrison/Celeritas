﻿using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// Increases the Hull strength(health) of the ship.
	/// </summary>

	[CreateAssetMenu(fileName = "New health Increase modifier", menuName = "Celeritas/Modifiers/Hull Strength Increase")]
	public class ShipHealthIncreaseSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{

		[SerializeField, PropertyRange(0, 100), Title("Percentage to add")]
		private float amount;

		[SerializeField, PropertyRange(0, 100), Title("Percentage extra to add per level")]
		private float amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies health quantity
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much extra the health are increased per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"<color=green>▲</color> Increase hull strength by <color=green>{(Amount + (AmountExtraPerLevel * level)):0}%</color>.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;

			float amountToAdd = (amount/100) + ((level * amountExtraPerLevel)/100);

			ship.Health.MaxValue = Convert.ToUInt32(ship.ShipData.StartingHealth + (ship.ShipData.StartingHealth * amountToAdd));
			Debug.Log("Changed ship health to " + ship.Health.MaxValue);
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			float shiphealth = Convert.ToSingle(ship.Health.MaxValue);
		
			float amountToRemove = (amount / 100) + ((level * amountExtraPerLevel) / 100);

			ship.Health.MaxValue = Convert.ToUInt32(shiphealth - (ship.ShipData.StartingHealth * amountToRemove));
			Debug.Log("Changed ship health to " + ship.Health.MaxValue);
		}
	}
}