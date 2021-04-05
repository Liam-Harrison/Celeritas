using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game {
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField, Title("Settings")] private ShipData ship;

		private void Awake()
		{
			EntityDataManager.OnLoadedAssets += () =>
			{
				var s = EntityDataManager.InstantiateEntity<ShipEntity>(ship);
			};
		}
	}
}
