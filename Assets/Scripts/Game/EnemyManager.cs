using Celeritas.AI;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages all enemies creation, destruction and lifetime.
	/// </summary>
	public class EnemyManager : Singleton<EnemyManager>
	{
		private readonly List<ShipEntity> ships = new List<ShipEntity>(128);

		/// <summary>
		/// Get a list of all current enemy ships.
		/// </summary>
		public IReadOnlyList<ShipEntity> Ships { get => ships.AsReadOnly(); }

		protected override void Awake()
		{
			base.Awake();
		}

		/// <summary>
		/// Spawn a new enemy ship.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="AIBase"/> to include with this enemy ship.</typeparam>
		/// <param name="ship">The ship data object to create.</param>
		/// <param name="position">The position to place this ship at.</param>
		/// <returns>Returns the created ship.</returns>
		public ShipEntity SpawnShip(ShipData ship, Vector3 position)
		{
			return SpawnShip(ship, position, Quaternion.identity);
		}

		/// <summary>
		/// Spawn a new enemy ship.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="AIBase"/> to include with this enemy ship.</typeparam>
		/// <param name="ship">The ship data object to create.</param>
		/// <param name="position">The position to place this ship at.</param>
		/// <param name="rotation">The rotation to place this ship with.</param>
		/// <returns>Returns the created ship.</returns>
		public ShipEntity SpawnShip(ShipData ship, Vector3 position, Quaternion rotation)
		{
			if (!EntityDataManager.Instance.Loaded)
			{
				Debug.LogError($"Attempted to spawn a ship before {nameof(EntityDataManager)} has been loaded!");
				return null;
			}

			var enemy = EntityDataManager.InstantiateEntity<ShipEntity>(ship, position, rotation);
			enemy.OnKilled += OnShipDestroyed;
			ships.Add(enemy);
			return enemy;
		}

		private void OnShipDestroyed(Entity entity)
		{
			var ship = entity as ShipEntity;
			ships.Remove(ship);

			if (ships.Count < 1 && WaveManager.Instance != null)
			{
				WaveManager.Instance.AllEnemyShipsDestroyed();
			}
		}
	}
}
