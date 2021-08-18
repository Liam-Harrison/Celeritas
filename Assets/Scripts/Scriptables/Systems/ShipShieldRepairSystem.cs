using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// Repairs shields by set percentage per second if shields have taken damage.
	/// </summary>

	[CreateAssetMenu(fileName = "New Ship Shield Repair Modifier", menuName = "Celeritas/Modifiers/Ship Shield Repair")]

	public class ShipShieldRepairSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityUpdated
	{

		[SerializeField, PropertyRange(0, 100), Title("Percentage repaired per second")]
		private float amount;

		[SerializeField, PropertyRange(0, 100), Title("Percentage added per level")]
		private float amountExtraPerLevel;

		/// <summary>
		/// The amount this system modifies shield repair
		/// </summary>
		public float Amount { get => amount; }

		/// <summary>
		/// How much extra shields are repaired per level
		/// </summary>
		public float AmountExtraPerLevel { get => amountExtraPerLevel; }

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"<color=green>▲</color> Repairs shields by <color=green>{(Amount + (AmountExtraPerLevel * level)):0}%</color> per second.";

		/// <summary>
		/// Determines whether the shield repairs or not.
		/// </summary>
		private bool IsActive = false;

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			IsActive = true;
			nextTime = (Time.time + interval);
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;
			IsActive = false;
		}

		/// <summary>
		/// Seconds between repairs
		/// </summary>
		private int interval = 1;

		/// <summary>
		/// Timer
		/// </summary>
		private float nextTime = 0;

		/// <summary>
		/// Checks if shield isn't full and will repair the ship by sending negative damage to the shield.
		/// </summary>
		public void OnEntityUpdated(Entity entity, ushort level)
		{
			var ship = entity as ShipEntity;

			
			float amountToAdd = (ship.Shield.MaxValue / 100) * (amount + (level * amountExtraPerLevel));

			if (IsActive == true)
			{
				if (Time.time > nextTime)
				{
					if (ship.Shield.CurrentValue < ship.Shield.MaxValue)
					{
						
						ship.Shield.Damage(Mathf.RoundToInt(-1 * amountToAdd));

					}
					nextTime = (nextTime + interval);
		
				}
			}
		}
	}
}