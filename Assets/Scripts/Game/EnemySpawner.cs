using Celeritas.AI;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField, Title("AI Ship Spawner")]
		private ShipData[] aiShips;

		private void Awake()
		{
			EntityDataManager.OnLoadedAssets += () =>
			{
				for (int i = 0; i < aiShips.Length; i++)
				{
					var ai = EntityDataManager.InstantiateEntity<ShipEntity>(aiShips[i]);
					ai.transform.position = transform.position.RandomPointOnCircle(5f);
					ai.gameObject.AddComponent<AIBasicChase>();
				}
			};
		}
	}
}
