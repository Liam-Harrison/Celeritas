using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Spawns and manages a player ship and its controller.
	/// </summary>
	public class PlayerSpawner : MonoBehaviour
	{
		[SerializeField, Title("Settings")] private ShipData ship;

		private void Awake()
		{
			EntityManager.OnLoadedAssets += () =>
			{
				var s = EntityManager.InstantiateEntity<ShipEntity>(ship);
				s.gameObject.AddComponent<PlayerController>();
			};
		}
	}
}
