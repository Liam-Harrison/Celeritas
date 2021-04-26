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

		protected override void Awake()
		{
			EntityDataManager.OnLoadedAssets += () =>
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
			};

			base.Awake();
		}

		private void OnPlayerActionAdded(GameAction action)
		{
			CombatHUDManager.Instance.AbilityBar.LinkAction(action);
		}

		private void OnPlayerActionRemoved(GameAction action)
		{
			CombatHUDManager.Instance.AbilityBar.UnlinkAction(action);
		}
	}
}
