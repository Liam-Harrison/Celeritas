using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using AssetIcons;

namespace Celeritas.Scriptables
{
	public enum ShipClass
	{
		Corvette,
		Destroyer,
		Battleship
	}

	/// <summary>
	/// Contains the instanced information for a ship.
	/// </summary>
	[InlineEditor]
	[CreateAssetMenu(fileName = "New Ship", menuName = "Celeritas/New Ship", order = 10)]
	public class ShipData : EntityData
	{
		[SerializeField, TitleGroup("General")]
		protected MovementSettings movementSettings;

		[SerializeField, TitleGroup("General")]
		private ShipClass shipClass;

		[SerializeField, TitleGroup("General"), PreviewField, AssetIcon(maxSize: 50)]
		private Sprite icon;

		[SerializeField, TitleGroup("General"), TextArea]
		private string description;

		[SerializeField, TitleGroup("Starting Settings")]
		private uint startingHealth;

		[SerializeField, TitleGroup("Starting Settings")]
		private uint startingShield;

		/// <summary>
		/// The ship class of this ship.
		/// </summary>
		public ShipClass ShipClass { get => ShipClass; }

		/// <summary>
		/// The movement settings for this ship.
		/// </summary>
		public MovementSettings MovementSettings { get => movementSettings; }

		/// <summary>
		/// How much health the entity starts with (== max health and == current health)
		/// </summary>
		public uint StartingHealth { get => startingHealth; }

		/// <summary>
		/// How much shield the entity starts with
		/// </summary>
		public uint StartingShield { get => startingShield; }

		public override string Tooltip => $"A <color=\"orange\">{ShipClass}</color> class ship.";

		protected virtual void OnValidate()
		{
			if (prefab != null)
			{
				if (prefab.HasComponent<ShipEntity>() == false)
					Debug.LogError($"Assigned prefab must have a {nameof(ShipEntity)} attatched to it.", this);

				if (prefab.HasComponent<Rigidbody2D>() == false)
					Debug.LogError($"Assigned prefab must have a {nameof(Rigidbody2D)} attatched to it.", this);
			}
		}
	}

	/// <summary>
	/// Movement settings for the ship hull. Cannot be changed, see <seealso cref="MovementModifier"/>
	/// on the ship itself to increase or decrease movement.
	/// </summary>
	[System.Serializable]
	public struct MovementSettings
	{
		[MinMaxSlider(0, 3000, showFields: true), Title("Rotation")]
		public Vector2 torquePerSec;

		[SerializeField]
		public AnimationCurve rotationCurve;

		[PropertyRange(0, 100)]
		public float angularDrag;

		[PropertyRange(0, 180)]
		public float rotationMaximum;

		[PropertyRange(0, 100000), PropertySpace, Title("Translation")]
		public float forcePerSec;

		[PropertyRange(0, 50)]
		public float mass;

		[PropertyRange(0, 1), Title("Aiming")]
		public float aimDeadzone;
	}
}
