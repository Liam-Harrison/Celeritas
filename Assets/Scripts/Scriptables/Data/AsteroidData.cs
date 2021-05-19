using Celeritas.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Data
{

	/// <summary>
	/// Contains the instance information for an asteroid
	/// </summary>
	[CreateAssetMenu(fileName = "New Asteroid", menuName = "Celeritas/New Asteroid", order = 50)]
	public class AsteroidData : EntityData
	{
		[SerializeField]
		private uint health;

		public uint Health { get => health; }

		public override string Tooltip => "An asteroid! What will happen if you destroy it?";
	}
}
