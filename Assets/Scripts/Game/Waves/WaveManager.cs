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

		//[SerializeField, Title("Waves")] private Wave[] waves;

		/// <summary>
		/// The waves the wave manager is managing.
		/// </summary>
		//public Wave[] Waves { get => waves; }

		[SerializeField]
		public WaveData[] data;

		public void StartWave()
		{
			Debug.Log("StartWave");
			for (int i = 0; i < data[nextWave].EnemyShips.Length; i++)
			{
				Debug.Log("Wave# " + nextWave + " Ship# " + i);
				EnemyManager.Instance.SpawnShip<Celeritas.AI.AIBasicChase>(data[nextWave].EnemyShips[i], PlayerController.Instance.ShipEntity.gameObject.transform.position.RandomPointOnCircle(5f));
			}
			nextWave++;
		}

		private uint nextWave;

		protected override void Awake()
		{
			nextWave = 0;
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

