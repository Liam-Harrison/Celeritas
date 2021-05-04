using Celeritas.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{

	/// <summary>
	/// Used to automatically generate the environment around the player
	/// Currently (4/5) only spawns asteroids.
	/// </summary>
	class EnvironmentGenerator: Singleton<EnvironmentGenerator>
	{
		/*
		private void Awake()
		{
			
		}*/

		private void Start()
		{
			// just seeing if adding an asteroid works, and fixes the destruction issue (spoiler: it does)
			Vector3 testPosition = new Vector3(1, 1, 0);
			EnvironmentManager.Instance.SpawnAsteroid(testPosition);
		}

		private void Update()
		{
			
		}
	}
}
