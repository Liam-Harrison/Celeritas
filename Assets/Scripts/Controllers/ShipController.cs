using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Celeritas.Extensions;

namespace Celeritas.Controllers
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipController : MonoBehaviour, Actions.IBasicActions
	{
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings"), Title("Speeds")]
		private float rotationPerSec;
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings"), PropertySpace]
		private float forwardForcePerSec;
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings")]
		private float sideForcePerSec;
		[SerializeField, PropertyRange(0, 1000), TabGroup("Settings")]
		private float backForcePerSec;

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

		private Vector2 locomotion;
		private Vector3 velocity;
		private Vector3 target;

		private void Update()
		{
			Vector3 up = transform.up, right = transform.right;

			velocity = up * ((Mathf.Max(locomotion.y, 0) * forwardForcePerSec) + (Mathf.Min(locomotion.y, 0) * backForcePerSec)) * Time.smoothDeltaTime;
			velocity += right * locomotion.x * sideForcePerSec * Time.smoothDeltaTime;

			Rigidbody.AddForce(velocity, ForceMode2D.Force);

			target = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			target = target.RemoveAxes(z: true);

			var dot = Vector3.Dot(up, (target - transform.position).normalized);

			if (dot < aimDeadzone)
			{
				Rigidbody.AddTorque(-dot * 2 * rotationPerSec * Time.smoothDeltaTime, ForceMode2D.Force);
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + velocity);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, (target - transform.position).normalized);
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
