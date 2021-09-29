using Celeritas.Game.Actions;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.Runstart;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Spawns and manages a player ship and its controller.
	/// </summary>
	public class PlayerSpawner : Singleton<PlayerSpawner>
	{
		[PreviewField]
		[SerializeField, Title("Settings")]
		private ShipData ship;

		protected override void OnGameLoaded()
		{
			GameStateManager.Instance.SetGameState(GameState.BACKGROUND);
			ShipEntity ship = ShipSelection.CurrentShip;

			if (ship == null)
			{
				ship = EntityDataManager.InstantiateEntity<ShipEntity>(this.ship, transform.position, forceIsPlayer: true);
			}

			ship.gameObject.AddComponent<PlayerController>();

			transform.parent = ship.transform;
			transform.localPosition = Vector3.zero;
		}
	}
}
