using Celeritas.Game;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{

	/// <summary>
	/// Used to automatically generate the environment around the player
	/// Currently (4/5) only spawns asteroids.
	/// todo: despawn asteroids a certain radius from the player
	/// todo: use object pool to create/reuse asteroids.
	/// </summary>
	class EnvironmentGenerator: Singleton<EnvironmentGenerator>
	{
		/// <summary>
		/// Number of random asteroids per block. 1 block has the same dimensions as the player's screen.
		/// </summary>
		[SerializeField, PropertyRange(0, 20), Title("Asteroid Generation variables -- per block (screen)")]
		int numberOfRandomAsteroids; // per block
		[SerializeField,PropertyRange(0, 25)]
		int numberOfAsteroidClusters;
		[SerializeField, PropertyRange(0, 20)]
		int asteroidNumberPerCluster;
		[SerializeField, PropertyRange(0, 100)]
		int asteroidClusterSpacing; // how far away asterids in the cluster are from one-another
		

		// keeps track of what 'blocks' have had asteroids spawned into them.
		// blocks are referenced by their lower left Vector3.
		private List<Rect> spawnedBlocks;

		private EnvironmentManager manager;

		private void Start()
		{
			manager = EnvironmentManager.Instance;
			spawnedBlocks = new List<Rect>();
		}

		private void Update()
		{
			// check current block
			var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
			var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
			CheckBlock(lowerLeft, upperRight);

			// loop through current block and all adjacent blocks
			// ie: * * *
			//     * p *
			//     * * *
			// where p = player. Will loop through all * blocks + player, making sure they have all spawned.
			// each block has the same dimensions as the screen.
			for (int i = -1; i <= 1; i++)
			{
				float horisontalAddition = Screen.width * i;

				for (int j = -1; j <= 1; j++)
				{
					float verticalAddition = Screen.height * j;
					lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(horisontalAddition, verticalAddition , 0));
					upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + horisontalAddition, Screen.height + verticalAddition));

					CheckBlock(lowerLeft, upperRight);
				}
			}
			
		}

		/// <summary>
		/// Checks the block defined by the passed lower left and upper right coordinates.
		/// If it hasn't been spawned yet, it will spawn asteroids in it.
		/// </summary>
		/// <param name="lowerLeft">lower left boundary of 'block' we are checking</param>
		/// <param name="upperRight">upper right boundary of 'block' we are checking</param>
		private void CheckBlock(Vector3 lowerLeft, Vector3 upperRight)
		{
			var middle = new Vector3((lowerLeft.x + upperRight.x)/2, (lowerLeft.y + upperRight.y)/2, 0);

			bool thisBlockHasBeenGenerated = false;

			foreach (Rect spawned in spawnedBlocks)
			{

				if (spawned.Contains(middle))
				{
					thisBlockHasBeenGenerated = true;
					return;
				}
			}
			if (thisBlockHasBeenGenerated == false)
			{
				Debug.Log("SPAWNING ASTEROIDS");
				SpawnAsteroidsBetweenBounds(lowerLeft, upperRight);
			}
		}

		////////////// Spawn asteroid logic ///////////////////////////
		///

		private void SpawnAsteroidsBetweenBounds(Vector3 lowerLeft, Vector3 upperRight)
		{
			spawnedBlocks.Add(BoundsToRect(lowerLeft, upperRight));
			
			SpawnAsteroidsWithRandomLayout(numberOfRandomAsteroids, lowerLeft, upperRight);
			for (int i = 0; i < numberOfAsteroidClusters; i++)
			{
				SpawnAsteroidsInCluster(asteroidNumberPerCluster, asteroidClusterSpacing, lowerLeft, upperRight);
			}
			
		}

		private Rect BoundsToRect(Vector3 lowerLeft, Vector3 upperRight)
		{
			return new Rect(lowerLeft.x, lowerLeft.y, upperRight.x - lowerLeft.x, upperRight.y - lowerLeft.y);
		}

		private int CountAsteroidsInBounds(Vector3 lowerLeft, Vector3 upperRight)
		{
			Rect rectangle = new Rect(lowerLeft.x, upperRight.y, upperRight.x - lowerLeft.x, upperRight.y - lowerLeft.y);
			int count = 0;
			foreach (Asteroid asteroid in EntityDataManager.Instance.Asteroids)
			{
				if (rectangle.Contains(asteroid.transform.position))
					count++;
			}
			return count;
		}

		/// <summary>
		/// Spawns asteroids randomly in a box outlined by its bottom left and top right corner.
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn</param>
		/// <param name="lowerLeftCorner">bottom left boundary of where asteroids will spawn</param>
		/// <param name="upperRightCorner">upper right boundary of where asteroids will spawn</param>
		private void SpawnAsteroidsWithRandomLayout(int amountToSpawn, Vector3 lowerLeftCorner, Vector3 upperRightCorner)
		{
			for (int i = 0; i < amountToSpawn; i++)
			{
				float xCoordinate = Random.Range(lowerLeftCorner.x, upperRightCorner.x);
				float yCoordinate = Random.Range(lowerLeftCorner.y, upperRightCorner.y);

				var asteroid = manager.SpawnAsteroid(new Vector3(xCoordinate, yCoordinate, 0));
				asteroid.transform.rotation = Random.rotation;
				RandomiseAsteroidScale(asteroid, 0.5f, 2f);
			}
		}

		/// <summary>
		/// Spawns 'amount' asteroids in a cluster
		/// </summary>
		/// <param name="amountToSpawn">number of asteroids to spawn</param>
		/// <param name="clusterVariation">Higher numbers = looser clusters. Lower = dense clusters</param>
		/// <param name="lowerLeftCorner">bottom left boundary of where asteroids will spawn</param>
		/// <param name="upperRightCorner">upper right boundary of where asteroids will spawn</param>
		private void SpawnAsteroidsInCluster(int amountToSpawn, int clusterVariation, Vector3 lowerLeftCorner, Vector3 upperRightCorner)
		{
			float xCoordinate = Random.Range(lowerLeftCorner.x + clusterVariation, upperRightCorner.x - clusterVariation);
			float yCoordinate = Random.Range(lowerLeftCorner.y + clusterVariation, upperRightCorner.y - clusterVariation);

			for (int i = 0; i < amountToSpawn; i++)
			{
				xCoordinate = xCoordinate + Random.Range(-clusterVariation/2, clusterVariation/2);
				yCoordinate = yCoordinate + Random.Range(-clusterVariation/2, clusterVariation/2);

				var asteroid = manager.SpawnAsteroid(new Vector3(xCoordinate, yCoordinate, 0));
				asteroid.transform.rotation = Random.rotation;
				RandomiseAsteroidScale(asteroid, 0.5f, 2f);
			}
		}

		private void RandomiseAsteroidScale(Asteroid asteroid, float minScale, float maxScale)
		{
			float scale = Random.Range(minScale, maxScale);
			asteroid.transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}
