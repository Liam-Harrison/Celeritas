using Celeritas.Game.Actions;
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
				EntityDataManager.OnLoadedAssets += () =>
				{
					CreatePlayerShip();
				};
			}
		}

		private void CreatePlayerShip()
		{
			var ship = EntityDataManager.InstantiateEntity<ShipEntity>(this.ship, forceIsPlayer: true);
			ship.gameObject.AddComponent<PlayerController>();
			ship.transform.position = transform.position;
			transform.parent = ship.transform;
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
