using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.UI
{
	[Serializable]
	public class EntityStatBar
	{
		[SerializeField, PropertyRange(1, 100), Title("Max Health")]
		private uint maxValue;

		[SerializeField, PropertyRange(1, 100), Title("Current Health")]
		private int currentValue;

		/// <summary>
		/// The entity's maximum health
		/// </summary>
		public uint MaxValue { get => maxValue; }

		/// <summary>
		/// The entity's current health
		/// </summary>
		public int CurrentValue { get => currentValue; }

		public EntityStatBar(uint startingValue)
		{
			maxValue = startingValue;
			currentValue = (int)startingValue;
		}

		/// <summary>
		/// Checks whether the stat is empty or not, returns true if so
		/// </summary>
		/// <returns>true if bar is empty (ie, current value == 0). If health, entity is dead.</returns>
		public bool IsEmpty()
		{
			if (currentValue < 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// damages entity's stat equal to the passed amount
		/// </summary>
		/// <param name="amount">Amount to damage entity with</param>
		public void Damage(int amount)
		{
			currentValue -= amount;
		}
	}
}
