using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.UI
{
	[Serializable]
	public class EntityStatBar
	{
		[SerializeField, PropertyRange(0, 100), Title("Max Health")]
		private float maxValue;

		[SerializeField, PropertyRange(0, 100), Title("Current Health")]
		private float currentValue;

		/// <summary>
		/// The entity's maximum health
		/// </summary>
		public float MaxValue { get => maxValue; set => maxValue = value; }

		/// <summary>
		/// The entity's current health
		/// </summary>
		public float CurrentValue { get => currentValue; }

		public EntityStatBar(float startingValue)
		{
			maxValue = startingValue;
			currentValue = startingValue;
		}

		/// <summary>
		/// Checks whether the stat is empty or not, returns true if so
		/// </summary>
		/// <returns>true if bar is empty (ie, current value == 0). If health, entity is dead.</returns>
		public bool IsEmpty()
		{
			return currentValue == 0;
		}

		/// <summary>
		/// damages entity's stat equal to the passed amount
		/// </summary>
		/// <param name="amount">Amount to damage entity with</param>
		public void Damage(float amount)
		{
			currentValue = Mathf.Clamp(currentValue - amount, 0, float.MaxValue);
		}
	}
}
