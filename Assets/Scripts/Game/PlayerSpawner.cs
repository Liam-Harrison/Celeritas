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

		[SerializeField]
		private ActionData action;

		protected void Start()
		{
			if (EntityDataManager.Instance.Loaded)
			{
				CreatePlayerShip();
			}
			else
			{
				EntityDataManager.OnLoadedAssets += CreatePlayerShip;
			}
		}

		private void CreatePlayerShip()
		{
			EntityDataManager.OnLoadedAssets -= CreatePlayerShip;

			ShipEntity ship = ShipSelection.CurrentShip;

			if (ship == null)
			{
				ship = EntityDataManager.InstantiateEntity<ShipEntity>(this.ship, transform.position, forceIsPlayer: true);
			}

			ship.gameObject.AddComponent<PlayerController>();

			transform.parent = ship.transform;
			transform.localPosition = Vector3.zero;

			ship.OnActionAdded += OnPlayerActionAdded;
			ship.OnActionRemoved += OnPlayerActionRemoved;

			if (action != null)
			{
				ship.AddAction(action);
			}
		}

		private void OnPlayerActionAdded(GameAction action)
		{
			CombatHUD.Instance.AbilityBar.LinkAction(action);
		}

		private void OnPlayerActionRemoved(GameAction action)
		{
			CombatHUD.Instance.AbilityBar.UnlinkAction(action);
		}
	}
}
