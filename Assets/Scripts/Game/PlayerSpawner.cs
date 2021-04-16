using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Extensions;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Celeritas.AI;

namespace Celeritas.Game
{
	/// <summary>
	/// Spawns and manages a player ship and its controller.
	/// </summary>
	public class PlayerSpawner : MonoBehaviour
	{
		[SerializeField, Title("Settings")]
		private ShipData ship;

		private void Awake()
		{
			EntityDataManager.OnLoadedAssets += () =>
			{
				var s = EntityDataManager.InstantiateEntity<ShipEntity>(ship);
				s.gameObject.AddComponent<PlayerController>();
				s.transform.position = transform.position;
				transform.parent = s.transform;
			};
		}
	}
}
