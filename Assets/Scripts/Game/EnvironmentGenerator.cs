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

		private void OnEnable()
		{
			EntityDataManager.OnCreatedChunk += SpawnAsteroidsInChunk;

			if (EntityDataManager.Instance == null || !EntityDataManager.Instance.Loaded)
				EntityDataManager.OnLoadedAssets += SpawnAsteroids;
			else
				SpawnAsteroids();
		}

		private void SpawnAsteroids()
		{
			foreach (var chunk in EntityDataManager.ChunkManager.Chunks)
			{
				SpawnAsteroidsInChunk(chunk);
			}
		}

		private void OnDisable()
		{
			EntityDataManager.OnCreatedChunk -= SpawnAsteroidsInChunk;
		}

		private void SpawnAsteroidsInChunk(Chunk chunk)
		{
			SpawnAsteroidsWithRandomLayout(numberOfRandomAsteroids, chunk);
			for (int i = 0; i < numberOfAsteroidClusters; i++)
			{
				SpawnAsteroidsInCluster(asteroidNumberPerCluster, chunk);
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
				SpawnAsteroid(chunk.GetRandomPositionInChunk());
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
				SpawnAsteroid(chunk.GetRandomPositionInChunk());
			}
		}

		private void RandomiseAsteroidScale(Asteroid asteroid, float minScale, float maxScale)
		{
			float scale = Random.Range(minScale, maxScale);
			asteroid.transform.localScale = new Vector3(scale, scale, scale);
		}

		private Asteroid SpawnAsteroid(Vector3 position)
		{
			Asteroid asteroid = EntityDataManager.InstantiateEntity<Asteroid>(asteroidPrefab, position, Random.rotation);
			RandomiseAsteroidScale(asteroid, 0.5f, 2f);
			return asteroid;
		}
	}
}
