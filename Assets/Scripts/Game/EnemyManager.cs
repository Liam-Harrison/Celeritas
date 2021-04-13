using Celeritas.AI;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages all enemies creation, destruction and lifetime.
	/// </summary>
	public class EnemyManager : Singleton<EnemyManager>
	{
		[SerializeField, Title("AI Ship Spawner")]
		private ShipData[] aiShips;

		private readonly List<ShipEntity> ships = new List<ShipEntity>(128);

		/// <summary>
		/// Get a list of all current enemy ships.
		/// </summary>
		public IReadOnlyList<ShipEntity> Ships { get => ships.AsReadOnly(); }

		protected override void Awake()
		{
			base.Awake();

			EntityDataManager.OnLoadedAssets += () =>
			{
				foreach (var ship in aiShips)
				{
					SpawnShip<AIBasicChase>(ship, transform.position.RandomPointOnCircle(5f));
				}
			};
		}

		/// <summary>
		/// Spawn a new enemy ship.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="AIBase"/> to include with this enemy ship.</typeparam>
		/// <param name="ship">The ship data object to create.</param>
		/// <param name="position">The position to place this ship at.</param>
		/// <returns>Returns the created ship.</returns>
		public ShipEntity SpawnShip<T>(ShipData ship, Vector3 position) where T : AIBase
		{
			return SpawnShip<T>(ship, position, Quaternion.identity);
		}

		/// <summary>
		/// Spawn a new enemy ship.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="AIBase"/> to include with this enemy ship.</typeparam>
		/// <param name="ship">The ship data object to create.</param>
		/// <param name="position">The position to place this ship at.</param>
		/// <param name="rotation">The rotation to place this ship with.</param>
		/// <returns>Returns the created ship.</returns>
		public ShipEntity SpawnShip<T>(ShipData ship, Vector3 position, Quaternion rotation) where T: AIBase
		{
			if (!EntityDataManager.Instance.Loaded)
			{
				Debug.LogError($"Attempted to spawn a ship before {nameof(EntityDataManager)} has been loaded!");
				return null;
			}

			var enemy = EntityDataManager.InstantiateEntity<ShipEntity>(ship);
			enemy.transform.position = position;
			enemy.transform.rotation = rotation;
			enemy.AttatchToAI(enemy.gameObject.AddComponent<T>());
			enemy.OnDestroyed += OnShipDestroyed;
			ships.Add(enemy);
			return enemy;
		}

		private void OnShipDestroyed(Entity entity)
		{
			var ship = entity as ShipEntity;
			ships.Remove(ship);
		}
	}
}
