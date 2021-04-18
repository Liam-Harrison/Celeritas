using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a ship.
	/// </summary>
	[InlineEditor]
	[CreateAssetMenu(fileName = "New Ship", menuName = "Celeritas/New Ship", order = 10)]
	public class ShipData : EntityData
	{
		[SerializeField, TitleGroup("Movement")]
		protected MovementSettings movementSettings;

		[SerializeField, TitleGroup("Starting Health")] private uint startingHealth;

		[SerializeField, TitleGroup("Starting Shield")] private uint startingShield;

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
		[MinMaxSlider(0, 300, showFields: true), Title("Rotation")]
		public Vector2 torquePerSec;

		[SerializeField]
		public AnimationCurve rotationCurve;

		[PropertyRange(0, 100)]
		public float angularDrag;

		[PropertyRange(0, 180)]
		public float rotationMaximum;

		[PropertyRange(0, 1000), PropertySpace, Title("Translation")]
		public float forwardForcePerSec;

		[PropertyRange(0, 1000)]
		public float sideForcePerSec;

		[PropertyRange(0, 1000)]
		public float backForcePerSec;

		[PropertyRange(0, 100)]
		public float mass;

		[PropertyRange(0, 1), Title("Aiming")]
		public float aimDeadzone;
	}
}
