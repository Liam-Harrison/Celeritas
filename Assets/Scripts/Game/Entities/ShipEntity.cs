using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a ship.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipEntity : Entity
	{
		/// <summary>
		/// The attatched ship data.
		/// </summary>
		public ShipData ShipData { get; private set; }

		/// <summary>
		/// The ships Rigidbody.
		/// </summary>
		public Rigidbody2D Rigidbody { get; private set; }

		/// <summary>
		/// The current aim target direction for this ship.
		/// </summary>
		public Vector3 Target { get; set; }

		/// <summary>
		/// The current translation input for this ship.
		/// </summary>
		public Vector3 Translation { get; set; }

		/// <summary>
		/// The current velocity of this ship.
		/// </summary>
		public Vector3 Velocity { get; private set; }

		/// <summary>
		/// Initalize this entity.
		/// </summary>
		/// <param name="data"></param>
		public override void Initalize(ScriptableObject data)
		{
			Rigidbody = GetComponent<Rigidbody2D>();
			ShipData = data as ShipData;

			ApplyRigidbodySettings();

			base.Initalize(data);
		}

		protected virtual void Update()
		{
			if (!IsInitalized)
				return;

			TranslationLogic();
			RotationLogic();
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Velocity);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position + Target);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + Up);
		}

		private void TranslationLogic()
		{
			Velocity = Up * ((Mathf.Max(Translation.y, 0) * ShipData.MovementSettings.forwardForcePerSec) +
							(Mathf.Min(Translation.y, 0) * ShipData.MovementSettings.backForcePerSec)) * Time.smoothDeltaTime;

			Velocity += Right * Translation.x * ShipData.MovementSettings.sideForcePerSec * Time.smoothDeltaTime;

			Rigidbody.AddForce(Velocity, ForceMode2D.Force);
		}

		private void RotationLogic()
		{
			var dot = Vector3.Dot(Up, Target);

			if (dot < ShipData.MovementSettings.aimDeadzone)
			{
				var torquePerSec = ShipData.MovementSettings.torquePerSec;
				var torque = Mathf.Lerp(torquePerSec.x, torquePerSec.y, ShipData.MovementSettings.rotationCurve.Evaluate(Mathf.InverseLerp(1, -1, dot))) * Time.smoothDeltaTime;

				if (Vector3.Dot(Right, Target) >= 0)
					torque = -torque;

				var absAngular = Mathf.Abs(Rigidbody.angularVelocity);
				if (absAngular < ShipData.MovementSettings.rotationMaximum || (absAngular < 1 && Mathf.Sign(torque) != Mathf.Sign(Rigidbody.angularVelocity)))
				{
					Rigidbody.AddTorque(torque, ForceMode2D.Force);
				}
			}
		}

		protected void ApplyRigidbodySettings()
		{
			Rigidbody.gravityScale = 0;
			Rigidbody.angularDrag = ShipData.MovementSettings.angularDrag;
			Rigidbody.mass = ShipData.MovementSettings.mass;
		}
	}
}
