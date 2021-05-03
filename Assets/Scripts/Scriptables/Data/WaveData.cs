using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for an enemy wave.
	/// </summary>
	[CreateAssetMenu(fileName = "New Wave", menuName = "Celeritas/New Wave", order = 50)]
	public class WaveData : ScriptableObject
	{
		[SerializeField, Title("ShipPool")] private ShipData[] shipPool;

		/// <summary>
		/// The pool of potential enemy ships for the wave.
		/// </summary>
		public ShipData[] ShipPool { get => shipPool; }

		[SerializeField, Title("Boss")] private bool isBoss;
		[SerializeField, ShowIf(nameof(isBoss))] private ShipData bossShip;

		public bool IsBoss { get => isBoss; }
		public ShipData BossShip { get => bossShip; }
	}
}
