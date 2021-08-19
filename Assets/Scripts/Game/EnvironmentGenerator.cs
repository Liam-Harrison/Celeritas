using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Game
{
	/// <summary>
	/// Used to automatically generate the environment around the player.
	/// Stored in Gameplay Managers (In the game scene)
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

		/// <summary>
		/// Asteroids will spawn at least this far away from PlayerSpawner.transform.position (player spawn)
		/// </summary>
		[SerializeField, PropertyRange(5, 25)]
		int minDistanceFromPlayer;

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
				SpawnAsteroidsInChunk(chunk, true);
			}
		}

		private void OnDisable()
		{
			EntityDataManager.OnCreatedChunk -= SpawnAsteroidsInChunk;
		}

		private void SpawnAsteroidsInChunk(Chunk chunk)
		{
			SpawnAsteroidsInChunk(chunk, false);
		}

		private void SpawnAsteroidsInChunk(Chunk chunk, bool avoidSpawn)
		{
			SpawnAsteroidsWithRandomLayout(numberOfRandomAsteroids, chunk, avoidSpawn);
			for (int i = 0; i < numberOfAsteroidClusters; i++)
			{
				SpawnAsteroidsInCluster(asteroidNumberPerCluster, chunk, avoidSpawn);
			}
		}

		/// <summary>
		/// Spawns asteroids randomly in a box outlined by its bottom left and top right corner.
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn</param>
		private void SpawnAsteroidsWithRandomLayout(int amountToSpawn, Chunk chunk, bool avoidSpawn)
		{
			for (int i = 0; i < amountToSpawn; i++)
			{
				SpawnAsteroid(GetRandomLocationInChunk(chunk, avoidSpawn));
			}
		}

		/// <summary>
		/// Spawns the specified amount of asteroids in a cluster
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn.</param>
		/// <param name="chunk">The chunk this asteroid will belong to.</param>
		private void SpawnAsteroidsInCluster(int amountToSpawn, Chunk chunk, bool avoidSpawn)
		{
			Vector3 position = GetRandomLocationInChunk(chunk, avoidSpawn);
			for (int i = 0; i < amountToSpawn; i++)
			{
				SpawnAsteroid(position);
			}
		}

		/// <summary>
		/// For getting a random location in a chunk
		/// greater than mindistance from player
		/// </summary>
		/// <param name="chunk"></param>
		/// <returns>A random location, minDistance away from the player (if avoidSpawn == true)</returns>
		private Vector3 GetRandomLocationInChunk(Chunk chunk, bool avoidSpawn) {

			if (PlayerSpawner.Instance == null || !avoidSpawn)
			{
				return chunk.GetRandomPositionInChunk();
			}

			Vector3 playerPosition = PlayerSpawner.Instance.transform.position;
			Vector3 toReturn;
			float distanceToPlayer;
			int count = 0; 
			do
			{
				toReturn = chunk.GetRandomPositionInChunk();
				distanceToPlayer = Vector3.Distance(toReturn, playerPosition);
				count++;
			}
			while (distanceToPlayer < minDistanceFromPlayer && count < 10);

			return toReturn;
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
