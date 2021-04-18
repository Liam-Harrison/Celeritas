using UnityEngine;

namespace Celeritas.Game.Entities
{
	public class LootController : Singleton<LootController>
	{
		/// <summary>
		/// The attatched ship data.
		/// </summary>
		public int ModuleComponents { get; private set; }

		/// <summary>
		/// The quanitity of Rare Metals the player has. This should be set to the players out-of-run rare metals at the start of the run scene.
		/// </summary>
		public int RareMetals { get; private set; }

		public void LootDrop(float dropValue, DropType dropType, Vector3 dropLocation)
		{
			if (dropType == DropType.Ship)
			{
				float lootRoll = GenerateRandomValue();

				if (lootRoll < dropValue)
				{
					ModuleComponents = ModuleComponents + 1;
					Debug.Log("Module dropped! You now have: " + ModuleComponents);

					//var drop = EntityDataManager.InstantiateEntity<*LOOT DROP VISUAL*>;
				}
			}

			if (dropType == DropType.Asteroid)
			{
				float lootRoll = GenerateRandomValue();

				if (lootRoll < dropValue)
				{
					RareMetals = RareMetals + Random.Range(1, 10);
					Debug.Log("Rare metals dropped! You now have: " + RareMetals);

					//var drop = EntityDataManager.InstantiateEntity<*LOOT DROP VISUAL*>;
				}
			}
		}

		public float GenerateRandomValue()
		{
			float roll = Random.Range(0f, 100f);

			return roll;
		}
	}
}