using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Data
{

	/// <summary>
	/// Contains the instance information for an asteroid
	/// </summary>
	[CreateAssetMenu(fileName = "New Asteroid", menuName = "Celeritas/New Asteroid", order = 50)]
	public class AsteroidData : EntityData
	{
		[SerializeField, TitleGroup("Asteroid"), MinMaxSlider(0, 200)]
		private Vector2 health;

		public Vector2 Health { get => health; }

		public override string Tooltip => "An asteroid! What will happen if you destroy it?";
	}
}
