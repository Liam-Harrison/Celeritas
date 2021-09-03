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
		private List<int> WaveLootValues;
		private int waveIndex;

		protected override void Awake()
		{
			waveIndex = 0;
			WaveLootValues = new List<int>() { 3, 5, 7, 9, 11, 20, 25, 30, 35, 40, 50, 60, 70, 80, 90, 150 };
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
			StartWaveInternal(wave);
		}

		/// <summary>
		/// Start a wave.
		/// </summary>
		public void StartWave()
		{
			if (Time.time - timeOfLastWave < MIN_TIME_BETWEEN_WAVES) // todo: hide button if not usable
			{
				Debug.Log("StartWave button in cooldown... Active in: " + (MIN_TIME_BETWEEN_WAVES - (Time.time - timeOfLastWave))+" seconds.");
				return;
			}

			StartWaveInternal(data[waveIndex]);

			waveIndex++;
			if (waveIndex >= data.Length)
			{
				waveIndex = 0;
			}
		}

		private void StartWaveInternal(WaveData wave)
		{
			timeOfLastWave = Time.time;
			WaveActive = true;
			ships[wave] = new List<ShipEntity>();

			List<ShipData> NewWave = new List<ShipData>();

			GenerateWave(NewWave);

			foreach (ShipData ship in NewWave)
			{
				var spawned = EnemyManager.Instance.SpawnShip<AIBasicChase>(ship, PlayerController.Instance.PlayerShipEntity.transform.position.RandomPointOnCircle(55f));
				ships[wave].Add(spawned);
			}

			OnWaveStarted?.Invoke();

			GameStateManager.Instance.SetGameState(GameState.WAVE);
		}

		public void StartFinalWave()
		{
			waveIndex = data.Length - 1;
			StartWave();
		}

		private Dictionary<WaveData, List<ShipEntity>> ships = new Dictionary<WaveData, List<ShipEntity>>();

		public IReadOnlyDictionary<WaveData, List<ShipEntity>> Waves { get => ships; }

		private void GenerateWave(List<ShipData> wave)
		{
			float waveLootTotal = WaveLootValues[waveIndex % WaveLootValues.Count];

			if (data[waveIndex].IsBoss)
			{
				wave.Add(data[waveIndex].BossShip);
				waveLootTotal -= data[waveIndex].BossShip.Prefab.GetComponent<ShipEntity>().LootConfig.Gain;
			}

			while (waveLootTotal > 0)
			{
				var randShip = UnityEngine.Random.Range(0, data[waveIndex].ShipPool.Length);
				wave.Add(data[waveIndex].ShipPool[randShip]);
				waveLootTotal -= data[waveIndex].ShipPool[randShip].Prefab.GetComponent<ShipEntity>().LootConfig.Gain;
			}
		}

		/// <summary>
		/// Tells the wave manager that all the enemy ships are destroyed.
		/// Initiates post-wave protocols. (UI popups, mainly.)
		/// </summary>
		public void AllEnemyShipsDestroyed()
		{
			WaveActive = false;
			ships.Clear();

			if (waveIndex >= data.Length)
				waveIndex = 0;

			OnWaveEnded?.Invoke();
			GameStateManager.Instance.SetGameState(GameState.BACKGROUND);
		}
	}
}

