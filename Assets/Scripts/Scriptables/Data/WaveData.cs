using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for an enemy wave.
	/// </summary>
	[CreateAssetMenu(fileName = "New Wave", menuName = "Celeritas/New Wave", order = 30)]
	public class WaveData : ModuleData
	{
		[SerializeField, Title("EnemyShips")] private ShipData[] enemyShips;

		/// <summary>
		/// The enemy ships in the wave.
		/// </summary>
		public ShipData[] EnemyShips { get => enemyShips; }
	}
}
