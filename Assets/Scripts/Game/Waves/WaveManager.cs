using Celeritas.Extensions;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public class WaveManager : Singleton<WaveManager>
	{
		private List<int> WaveLootValues;
		private uint nextWave;

		protected override void Awake()
		{
			nextWave = 0;
			//TODO: read WaveLootValues from file.
			WaveLootValues = new List<int>() { 5, 7, 10 };
		}

		[SerializeField]
		public WaveData[] data;
		private int waveAll = 0;
		[SerializeField]
		public int MinShips = 5;



		/// <summary>
		/// Start a wave.
		/// </summary>
		public void StartWave()
		{
			Debug.Log("StartWave");
			List<ShipData> NewWave = new List<ShipData>();
			for (int i = 0; i < 30; i++)
			{
				Debug.Log("Generate Wave: #" + i);
				GenerateWave(NewWave);
				Debug.Log("Ships: " + NewWave.Count);
				if (NewWave.Count >= MinShips)
				{
					break;
				}
			}

			//If adding wave shapes, modify here
			foreach (ShipData ship in NewWave)
			{
				EnemyManager.Instance.SpawnShip<Celeritas.AI.AIBasicChase>(ship, PlayerController.Instance.ShipEntity.gameObject.transform.position.RandomPointOnCircle(10f));
			}
			nextWave++;
		}

		System.Random random = new System.Random();

		private void GenerateWave(List<ShipData> Wave)
		{
			float waveLootTotal = WaveLootValues[0];
			int waveType = waveAll; //FIX WHEN LOGIC KNOWN omfg
			int randShip;
			if (data[waveType].IsBoss)
			{
				Wave.Add(data[waveType].BossShip);
				waveLootTotal -= data[waveType].BossShip.Prefab.GetComponent<ShipEntity>().LootConfig.Gain;
			}
			while (waveLootTotal > 0)
			{
				randShip = random.Next(0, data[waveType].ShipPool.Length);
				Wave.Add(data[waveType].ShipPool[randShip]);
				waveLootTotal -= data[waveType].ShipPool[randShip].Prefab.GetComponent<ShipEntity>().LootConfig.Gain;
			}
			Debug.Log("Loot Total: " + waveLootTotal);
		}

		/// <summary>
		/// Tells the wave manager that all the enemy ships are destroyed.
		/// Initiates post-wave protocols. (UI popups, mainly.)
		/// </summary>
		public void AllEnemyShipsDestroyed()
		{
			Debug.Log("AllEnemyShipsDestroyed!");
			//for testing - lets you summon infinite waves
			nextWave = 0;
		}

	}
}

