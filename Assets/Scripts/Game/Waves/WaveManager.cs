using Celeritas.AI;
using Celeritas.Extensions;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public class WaveManager : Singleton<WaveManager>
	{
		[SerializeField]
		public WaveData[] data;

		public bool WaveActive { get; private set; }

		private uint nextWave;

		public static event Action OnWaveEnded;
		public static event Action OnWaveStarted;

		private Dictionary<WaveData, List<ShipEntity>> ships = new Dictionary<WaveData, List<ShipEntity>>();

		public IReadOnlyDictionary<WaveData, List<ShipEntity>> Waves { get => ships; }

		protected override void Awake()
		{
			nextWave = 0;
		}

		public void StartWave()
		{
			WaveActive = true;
			var wave = data[nextWave++];
			ships[wave] = new List<ShipEntity>();

			foreach (var ship in wave.EnemyShips)
			{
				var spawned = EnemyManager.Instance.SpawnShip<AIBasicChase>(ship, PlayerController.Instance.PlayerShipEntity.transform.position.RandomPointOnCircle(20f));
				ships[wave].Add(spawned);
			}

			OnWaveStarted?.Invoke();
		}

		/// <summary>
		/// Tells the wave manager that all the enemy ships are destroyed.
		/// Initiates post-wave protocols. (UI popups, mainly.)
		/// </summary>
		public void AllEnemyShipsDestroyed()
		{
			WaveActive = false;
			ships.Clear();

			if (nextWave >= data.Length)
				nextWave = 0;

			OnWaveEnded?.Invoke();
		}

	}
}

