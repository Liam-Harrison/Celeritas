using Celeritas.Extensions;
using Celeritas.Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Controllers
{
	/// <summary>
	/// This class provides basic movement for a ship.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipController : Entity, Actions.IBasicActions
	{
		[SerializeField, MinMaxSlider(0, 300, showFields: true), TabGroup("Settings"), Title("Rotation")]
		private Vector2 torquePerSec;
		[SerializeField, TabGroup("Settings")]
		private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings"), PropertySpace, Title("Acceleration")]
		private float forwardForcePerSec;
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings")]
		private float sideForcePerSec;
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings")]
		private float backForcePerSec;

		[SerializeField, PropertyRange(0, 180), TabGroup("Settings"), Title("Maximums")]
		private float rotationMaximum;

		[SerializeField, PropertyRange(0, 1), TabGroup("Settings"), Title("Aiming")]
		private float aimDeadzone;

		/// <summary>
		/// Access this ships Rigidbody.
		/// </summary>
		public Rigidbody2D Rigidbody { get; private set; }

		private Actions.BasicActions actions = default;
		private new Camera camera;

		private void Awake()
		{
			actions = new Actions.BasicActions(new Actions());
			actions.SetCallbacks(this);

			Rigidbody = GetComponent<Rigidbody2D>();

			camera = Camera.main;
		}

		private void OnEnable()
		{
			actions.Enable();
		}

		private void OnDisable()
		{
			actions.Disable();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + velocity);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position + target);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + Up);
		}

		private Vector2 locomotion;
		private Vector3 velocity;
		private Vector3 target;

		private void Update()
		{
			Translation();
			Rotation();
		}

		private void Translation()
		{
			velocity = Up * ((Mathf.Max(locomotion.y, 0) * forwardForcePerSec) + (Mathf.Min(locomotion.y, 0) * backForcePerSec)) * Time.smoothDeltaTime;
			velocity += Right * locomotion.x * sideForcePerSec * Time.smoothDeltaTime;

			Rigidbody.AddForce(velocity, ForceMode2D.Force);
		}

		private void Rotation()
		{
			target = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			target = Vector3.ProjectOnPlane(target, Vector3.forward);
			target = (target - transform.position).normalized;

			var dot = Vector3.Dot(Up, target);

			if (dot < aimDeadzone)
			{
				var torque = Mathf.Lerp(torquePerSec.x, torquePerSec.y, rotationCurve.Evaluate(Mathf.InverseLerp(1, -1, dot))) * Time.smoothDeltaTime;
				if (Vector3.Dot(Right, target) >= 0)
					torque = -torque;

				var absAngular = Mathf.Abs(Rigidbody.angularVelocity);
				if (absAngular < rotationMaximum || (absAngular < 1 && Mathf.Sign(torque) != Mathf.Sign(Rigidbody.angularVelocity)))
				{
					Rigidbody.AddTorque(torque, ForceMode2D.Force);
				}
			}
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				// On Mouse Down
			}
			else if (context.canceled)
			{
				// On Mouse Up
			}
		}

		public void OnLocomotion(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}
	}
}
