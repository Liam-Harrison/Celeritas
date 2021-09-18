using Celeritas.AI;
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
		protected override void Awake()
		{
		}

		[SerializeField]
		public WaveData[] data;

		[SerializeField]
		public int MinShips = 1;

		public bool WaveActive { get; private set; }

		public static event Action OnWaveEnded;
		public static event Action OnWaveStarted;

		private float MIN_TIME_BETWEEN_WAVES = 5; // for build
		private float timeOfLastWave = 0;

		public void StartWave(WaveData wave)
		{
			GenerateWave(wave);
		}

		/// <summary>
		/// Start a random wave.
		/// </summary>
		public void StartRandomWave()
		{
			//if (Time.time - timeOfLastWave < MIN_TIME_BETWEEN_WAVES) // todo: hide button if not usable
			//{
			//	Debug.Log("StartWave button in cooldown... Active in: " + (MIN_TIME_BETWEEN_WAVES - (Time.time - timeOfLastWave))+" seconds.");
			//	return;
			//}

			GenerateWave(data[UnityEngine.Random.Range(0,data.Length-1)]);

		}

		public void StartFinalWave()
		{
			StartWave(data[data.Length - 1]);
		}

		private Dictionary<WaveData, List<ShipEntity>> ships = new Dictionary<WaveData, List<ShipEntity>>();

		public IReadOnlyDictionary<WaveData, List<ShipEntity>> Waves { get => ships; }

		private void GenerateWave(WaveData wave)
		{
			//timeOfLastWave = Time.time;
			WaveActive = true;
			ships[wave] = new List<ShipEntity>();

			foreach (ShipData ship in wave.ShipPool)
			{
				var spawned = EnemyManager.Instance.SpawnShip(ship, PlayerController.Instance.PlayerShipEntity.transform.position.RandomPointOnCircle(20f));
				ships[wave].Add(spawned);
			}

			OnWaveStarted?.Invoke();
			GameStateManager.Instance.SetGameState(GameState.WAVE);
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

