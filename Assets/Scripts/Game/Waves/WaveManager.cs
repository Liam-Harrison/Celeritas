using Celeritas.AI;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public class WaveManager : Singleton<WaveManager>
	{
		protected override void Awake()
		{
		}

		[SerializeField]
		public WaveData[] data;

		[SerializeField]
		public int MinShips = 1;

		[SerializeField, TitleGroup("Enemy Spawn Settings")]
		private float radiusAwayFromPlayer = 90f; // how far away from the player the enemies will spawn

		[SerializeField, TitleGroup("Enemy Spawn Settings")]
		private int maxNumberOfTriesToAvoidAsteroid = 3; // how many times a spawn location will be re-attempted if it is too close to an asteroid

		[SerializeField, TitleGroup("Enemy Spawn Settings")]
		private float radiusAwayFromAsteroids = 10; // how far away (minimum) from asteroids enemies will try to spawn

		public bool WaveActive { get; private set; }

		public static event Action OnWaveEnded;
		public static event Action OnWaveStarted;

		public void StartWave(WaveData wave)
		{
			GenerateWave(wave);
		}

		/// <summary>
		/// Start a random wave.
		/// </summary>
		public void StartRandomWave()
		{
			GenerateWave(data[UnityEngine.Random.Range(0, data.Length - 1)]);
		}

		public void StartFinalWave()
		{
			StartWave(data[data.Length - 1]);
		}

		private Dictionary<WaveData, List<ShipEntity>> ships = new Dictionary<WaveData, List<ShipEntity>>();

		public IReadOnlyDictionary<WaveData, List<ShipEntity>> Waves { get => ships; }

		private void GenerateWave(WaveData wave)
		{
			WaveActive = true;
			ships[wave] = new List<ShipEntity>();

			foreach (ShipData ship in wave.ShipPool)
			{
				//Debug.Log("Spawned: " + ship.Title);

				Vector3 spawnPosition = getShipSpawnPoint(maxNumberOfTriesToAvoidAsteroid);
				
				var spawned = EnemyManager.Instance.SpawnShip(ship, spawnPosition);
				ships[wave].Add(spawned);
			}

			OnWaveStarted?.Invoke();
			GameStateManager.Instance.SetGameState(GameState.WAVE);
		}

		/// <summary>
		/// Returns a ship spawn location, at 'radiusAwayFromPlayer' away from the player
		/// and 'radiusAwayFromAsteroids' away from asteroids
		/// Used to generate a spawn point for an enemy ship, when a wave starts
		/// </summary>
		/// <returns>A spawn point for the enemy ship</returns>
		private Vector3 getShipSpawnPoint(int maxNumberOfTries)
		{
			List<Collider2D> withinRange = new List<Collider2D>();
			ContactFilter2D filter = new ContactFilter2D();
			filter.NoFilter();
			Vector3 possiblePosition = PlayerController.Instance.PlayerShipEntity.transform.position.RandomPointOnCircle(radiusAwayFromPlayer);

			// try maxNumberOfTries to find an appropriate spawn point
			for (int i = 0; i < maxNumberOfTries; i ++)
			{
				Physics2D.OverlapCircle(possiblePosition, radiusAwayFromAsteroids, filter, withinRange);
				bool tooCloseToAsteroid = false;

				// loop through each entity within the radius. If an asteroid is within the radius, position is invalid, retry
				foreach (Collider2D collider in withinRange)
				{
					if (collider.attachedRigidbody == null)
						continue;
					if (collider.attachedRigidbody.GetComponent<Asteroid>() != null) // an asteroid was found
					{
						tooCloseToAsteroid = true;
						//Debug.Log($"Location was too close to asteroid. position: {possiblePosition}");
						break;
					}
				}
				if (!tooCloseToAsteroid)
				{
					// successfully found a valid position for ship to spawn
					//Debug.Log($" {i} successfully found position away from asteroid");
					return possiblePosition;
				}

				// try different position for next loop
				possiblePosition = PlayerController.Instance.PlayerShipEntity.transform.position.RandomPointOnCircle(radiusAwayFromPlayer);
			}
			Debug.LogWarning($"Enemy spawn: tried {maxNumberOfTries} times, failed to find position away from asteroid");
			return possiblePosition; // if here, it tried X times to find a non conflicting point, but failed
		}

		/// <summary>
		/// Tells the wave manager that all the enemy ships are destroyed.
		/// Initiates post-wave protocols. (UI popups, mainly.)
		/// </summary>
		public void AllEnemyShipsDestroyed()
		{
			WaveActive = false;
			ships.Clear();

			OnWaveEnded?.Invoke();
			GameStateManager.Instance.SetGameState(GameState.BACKGROUND);
		}
	}
}

