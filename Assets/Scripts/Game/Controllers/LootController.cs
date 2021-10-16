using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game.Controllers
{
	public enum LootType { Module, RareMetal }

	public class LootController : Singleton<LootController>
	{
		/// <summary>
		/// The attatched ship data. How many modules the player has.
		/// </summary>
		public int ModuleComponents { get; private set; }

		/// <summary>
		/// The quanitity of Rare Metals the player has. This should be set to the players out-of-run rare metals at the start of the run scene.
		/// </summary>
		public int RareMetals { get; private set; }

		/// <summary>
		/// Data for creating dropped module loot
		/// </summary>
		[SerializeField]
		private LootData moduleDropData;

		/// <summary>
		/// Data for creating dropped metal loot (appears on map, player can pick it up)
		/// </summary>
		[SerializeField]
		private LootData rareMetalDropData;

		/// <summary>
		/// Max number of rare metals that will drop when an asteroid is destroyed
		/// </summary>
		[SerializeField, PropertyRange(1, 25)]
		private int maxRareMetalDropAmount;

		/// <summary>
		/// Max number of modules that will drop when a ship is destroyed
		/// </summary>
		[SerializeField, PropertyRange(1, 25)]
		private int maxModuleDropAmount;

		/// <summary>
		/// Invoked when the amount of modules changes.
		/// </summary>
		public static event System.Action<int, int> OnModulesChanged;

		/// <summary>
		/// Invoked when the amount of rare components changes.
		/// </summary>
		public static event System.Action<int, int> OnRareComponentsChanged;

		/// <summary>
		/// Generate a loot drop. Currently will automatically be picked up by the player
		/// </summary>
		/// <param name="dropValue">percentage chance of item dropping ( 1 = 1% chance)</param>
		/// <param name="dropType">type of loot drop (eg asteroid, ship)</param>
		/// <param name="dropLocation">presently unused</param>
		public void LootDrop(float dropValue, DropType dropType, Vector3 dropLocation)
		{
			if (dropType == DropType.Ship)
			{
				float lootRoll = GenerateRandomValue();

				if (lootRoll < dropValue)
				{
					LootEntity created = EntityDataManager.InstantiateEntity<LootEntity>(moduleDropData, dropLocation);
					created.Amount = Random.Range(1, maxModuleDropAmount);
				}
			}

			if (dropType == DropType.Asteroid)
			{
				float lootRoll = GenerateRandomValue();
				if (lootRoll < dropValue)
				{
					LootEntity created = EntityDataManager.InstantiateEntity<LootEntity>(rareMetalDropData, dropLocation);
					created.Amount = Random.Range(1, maxRareMetalDropAmount); 
				}
			}
		}

		public float GenerateRandomValue()
		{
			float roll = Random.Range(0f, 100f);

			return roll;
		}

		/// <summary>
		/// Gives the player loot
		/// </summary>
		/// <param name="type">The type of the loot (Module or Rare Metal)</param>
		/// <param name="amount">How much of the loot to give the player</param>
		public void GivePlayerLoot(LootType type, int amount)
		{
			switch (type)
			{
				case LootType.Module:
					ModuleComponents += amount;
					OnModulesChanged?.Invoke(ModuleComponents, amount);
					break;

				case LootType.RareMetal:
					RareMetals += amount;
					if (RareMetals > 99)
					{
						RareMetals -= 100;
						CombatHUD.Instance.PrintNotification("-100 Rare Metals!");
						GivePlayerLoot(LootType.Module, 1);
					}
					OnRareComponentsChanged?.Invoke(RareMetals, amount);
					break;
			}
		}
	}
}