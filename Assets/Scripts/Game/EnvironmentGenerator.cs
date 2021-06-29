using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
	/// <summary>
	/// Used to automatically generate the environment around the player.
	/// </summary>
	class EnvironmentGenerator: Singleton<EnvironmentGenerator>
	{
		private const float UPDATE_FREQ = 2;

		[SerializeField]
		private EntityData asteroidPrefab;

		[SerializeField, PropertyRange(0, 20), Title("Asteroid Generation Variables")]
		int numberOfRandomAsteroids;

		[SerializeField,PropertyRange(0, 25)]
		private int numberOfAsteroidClusters;

		[SerializeField, PropertyRange(0, 20)]
		private int asteroidNumberPerCluster;

		[SerializeField, PropertyRange(0, 100)]
		private int asteroidClusterSpacing;

		[SerializeField, DisableInPlayMode]
		private Vector2 chunkSize = new Vector2(20, 20);

		[SerializeField]
		private float unloadDistance = 400;

		private float unloadDistanceSqr;

		private ChunkManager ChunkManager;

		private new Camera camera;

		private float lastUpdate;

		private void Start()
		{
			ChunkManager = new ChunkManager(chunkSize);
			unloadDistanceSqr = unloadDistance * unloadDistance;

			camera = Camera.main;
		}

		private void Update()
		{
			if (!EntityDataManager.Instance.Loaded)
				return;

			if (Time.unscaledTime < lastUpdate + (1f / UPDATE_FREQ))
				return;

			lastUpdate = Time.unscaledTime;

			var middle = ChunkManager.GetOrCreateChunk(camera.transform.position);

			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					var pos = middle.Index;
					pos += new Vector2Int(i, j);

					var chunk = ChunkManager.GetOrCreateChunk(pos);

					if (chunk.Entites.Count == 0)
						SpawnAsteroidsInChunk(chunk);
				}
			}

			var toRemove = new HashSet<Chunk>();
			foreach (var chunk in ChunkManager.Chunks)
			{
				var delta = (chunk.Center - camera.transform.position).sqrMagnitude;
				if (delta > unloadDistanceSqr)
				{
					toRemove.Add(chunk);
				}
			}

			foreach (var chunk in toRemove)
			{
				ChunkManager.UnloadChunk(chunk);
			}
		}

		private void SpawnAsteroidsInChunk(Chunk chunk)
		{
			SpawnAsteroidsWithRandomLayout(numberOfRandomAsteroids, chunk);
			for (int i = 0; i < numberOfAsteroidClusters; i++)
			{
				SpawnAsteroidsInCluster(asteroidNumberPerCluster, chunk);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (ChunkManager == null)
				return;

			Gizmos.color = Color.green;
			foreach (var chunk in ChunkManager.Chunks)
			{
				Gizmos.DrawWireCube(chunk.Center, new Vector3(chunk.Size.x, chunk.Size.y, 1));
			}
		}

		/// <summary>
		/// Spawns asteroids randomly in a box outlined by its bottom left and top right corner.
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn</param>
		/// <param name="lowerLeftCorner">bottom left boundary of where asteroids will spawn</param>
		/// <param name="upperRightCorner">upper right boundary of where asteroids will spawn</param>
		private void SpawnAsteroidsWithRandomLayout(int amountToSpawn, Chunk chunk)
		{
			for (int i = 0; i < amountToSpawn; i++)
			{
				SpawnAsteroid(chunk, chunk.GetRandomPositionInChunk());
			}
		}

		/// <summary>
		/// Spawns the specified amount of asteroids in a cluster
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn.</param>
		/// <param name="chunk">The chunk this asteroid will belong to.</param>
		private void SpawnAsteroidsInCluster(int amountToSpawn, Chunk chunk)
		{
			for (int i = 0; i < amountToSpawn; i++)
			{
				SpawnAsteroid(chunk, chunk.GetRandomPositionInChunk());
			}
		}

		private void RandomiseAsteroidScale(Asteroid asteroid, float minScale, float maxScale)
		{
			float scale = Random.Range(minScale, maxScale);
			asteroid.transform.localScale = new Vector3(scale, scale, scale);
		}

		private Asteroid SpawnAsteroid(Chunk chunk, Vector3 position)
		{
			Asteroid asteroid = EntityDataManager.InstantiateEntity<Asteroid>(asteroidPrefab);

			asteroid.transform.position = position;
			asteroid.transform.rotation = Random.rotation;
			RandomiseAsteroidScale(asteroid, 0.5f, 2f);

			chunk.AddEntity(asteroid);

			return asteroid;
		}
	}
}
