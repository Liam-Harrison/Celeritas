using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
	/// <summary>
	/// Used to keep track of & spawn asteroids.
	/// May be generalised to include more environmental in-game elements, however.
	/// </summary>
	class EnvironmentManager : Singleton<EnvironmentManager>
	{
		[SerializeField]
		private EntityData asteroidPrefab;

		private readonly List<Asteroid> asteroids = new List<Asteroid>(128);


		/// <summary>
		/// Get a list of all asteroids
		/// </summary>
		public IReadOnlyList<Asteroid> Asteroids { get => asteroids.AsReadOnly(); }

		public Asteroid SpawnAsteroid(Vector3 position)
		{
			Asteroid asteroid = EntityDataManager.InstantiateEntity<Asteroid>(asteroidPrefab);
			asteroid.transform.position = position;
			asteroid.OnDestroyed += OnAsteroidDestroyed;
			asteroids.Add(asteroid);
			return asteroid;
		}

		private void OnAsteroidDestroyed(Entity entity)
		{
			Asteroid asteroid = entity as Asteroid;
			asteroids.Remove(asteroid);
		}
	}
}
