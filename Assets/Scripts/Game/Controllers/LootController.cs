using Celeritas.Extensions;
using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
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

		public void LootDrop(float dropValue, bool isBoss, string dropType, Vector3 dropLocation)
		{
			if (dropType == "EnemyShip")
            {
				float lootRoll = Random.Range(100, 10100);
				lootRoll = (lootRoll - 100);
				lootRoll = (lootRoll / 100);

				if (dropValue > lootRoll)
                {
					ModuleComponents = ModuleComponents + 1;
					Debug.Log("Module dropped! You now have: " + ModuleComponents);

					//var drop = EntityDataManager.InstantiateEntity<*LOOT DROP VISUAL*>;
				}
			}

			if (dropType == "Asteroid")
			{
				float lootRoll = Random.Range(100, 10100);
				lootRoll = (lootRoll - 100);
				lootRoll = (lootRoll / 100);

				if (dropValue > lootRoll)
				{
					RareMetals = RareMetals + Random.Range(1, 10);
					Debug.Log("Rare metals dropped! You now have: " + RareMetals);

					//var drop = EntityDataManager.InstantiateEntity<*LOOT DROP VISUAL*>;
				}
			}
		}
	}
}