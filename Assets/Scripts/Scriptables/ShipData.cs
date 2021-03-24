using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a ship.
	/// </summary>
	[CreateAssetMenu(fileName = "New Ship", menuName = "Celeritas/New Ship", order = 10)]
	public class ShipData : EntityData
	{
		[SerializeField, TitleGroup("Movement")]
		protected MovementSettings movementSettings;

		/// <summary>
		/// The movement settings for this ship.
		/// </summary>
		public MovementSettings MovementSettings { get => movementSettings; }

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

	[System.Serializable]
	public struct MovementSettings
	{
		[SerializeField, MinMaxSlider(0, 300, showFields: true), Title("Rotation")]
		public Vector2 torquePerSec;

		[SerializeField]
		public AnimationCurve rotationCurve;

		[SerializeField, PropertyRange(0, 100)]
		public float angularDrag;

		[SerializeField, PropertyRange(0, 180)]
		public float rotationMaximum;

		[SerializeField, PropertyRange(0, 1000), PropertySpace, Title("Translation")]
		public float forwardForcePerSec;

		[SerializeField, PropertyRange(0, 1000)]
		public float sideForcePerSec;

		[SerializeField, PropertyRange(0, 1000)]
		public float backForcePerSec;

		[SerializeField, PropertyRange(0, 100)]
		public float mass;

		[SerializeField, PropertyRange(0, 1), Title("Aiming")]
		public float aimDeadzone;
	}
}
