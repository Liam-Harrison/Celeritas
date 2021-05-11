using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Controllers;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	public class LootController : Singleton<LootController>
	{
		public enum LootType { Module, RareMetal}

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
		public LootData moduleDropData;

		/// <summary>
		/// Data for creating dropped metal loot (appears on map, player can pick it up)
		/// </summary>
		[SerializeField]
		public LootData rareMetalDropData;

		PlayerShipEntity player; // here to test if giving parent will help w lootEntity destruction

		public void Start()
		{
			player = PlayerController.Instance.PlayerShipEntity; // just for testing.
		}

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
					LootEntity created = EntityDataManager.InstantiateEntity<LootEntity>(moduleDropData, player);
					created.transform.position = dropLocation;
					created.Amount = 1; // amount of loot -- currently will only drop 1 module each time
					//CombatHUD.Instance.PrintNotification("Module dropped! You now have: " + ModuleComponents);
				}
			}

			if (dropType == DropType.Asteroid)
			{
				float lootRoll = GenerateRandomValue();
				if (lootRoll < dropValue)
				{
					int amount = Random.Range(1, 10); // will drop 1-9 rare metals
					LootEntity created = EntityDataManager.InstantiateEntity<LootEntity>(rareMetalDropData, player);
					created.transform.position = dropLocation;
					created.Amount = amount; 

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

					if (CombatHUD.Instance != null)
					{
						CombatHUD.Instance.PrintNotification("+" + amount + " Modules!");
						CombatHUD.Instance.UpdateLootCount(LootType.Module, ModuleComponents);
					}

					break;

				case LootType.RareMetal:
					RareMetals += amount;

					if (CombatHUD.Instance != null)
					{
						CombatHUD.Instance.PrintNotification("+"+amount+" Rare Metals!");
						CombatHUD.Instance.UpdateLootCount(LootType.RareMetal, RareMetals);
					}

					break;
			}
		}
	}
}