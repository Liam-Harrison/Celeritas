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

		public override string GetTooltip(int level) => $"Repairs shields by <color=green>{(Amount + (AmountExtraPerLevel * level)):0}%</color> per second.";

		/// <summary>
		/// Seconds between repairs
		/// </summary>
		private float interval = 1.0f;

		[SerializeField, Title("Interval between repairs", "Seconds between each repair")]
		public float Interval { get => interval; set => interval = value; }

		/// <summary>
		/// Timer
		/// </summary>
		private float nextTime = 0.0f;

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			nextTime = (Time.deltaTime + interval);
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
		}

		/// <summary>
		/// Checks if shield isn't full and will repair the ship by sending negative damage to the shield.
		/// </summary>
		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			var ship = entity as ShipEntity;
			
			float amountToAdd = (ship.Shield.MaxValue / 100) * (amount + (wrapper.Level * amountExtraPerLevel));

			nextTime = nextTime + Time.deltaTime;

			if (nextTime >= interval)
			{
				if (ship.Shield.CurrentValue < ship.Shield.MaxValue)
				{

					ship.Shield.Damage(Mathf.RoundToInt(-1 * amountToAdd));
					//Debug.Log("Healed: " + Mathf.RoundToInt(-1 * amountToAdd));

				}
				nextTime = 0.0f;
			}
		}
	}
}