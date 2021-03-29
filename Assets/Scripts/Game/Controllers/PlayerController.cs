using Celeritas.Game.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Game.Controllers
{
	/// <summary>
	/// Routes player input to a targeted ship Entity.
	/// </summary>
	[RequireComponent(typeof(ShipEntity))]
	public class PlayerController : Singleton<PlayerController>, Actions.IBasicActions
	{
		private Actions.BasicActions actions = default;
		private Camera _camera;

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public ShipEntity ShipEntity { get; private set; }

		protected override void Awake()
		{
			actions = new Actions.BasicActions(new Actions());
			actions.SetCallbacks(this);

			ShipEntity = GetComponent<ShipEntity>();

			_camera = Camera.main;
			_camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;

			base.Awake();
		}

		protected virtual void OnEnable()
		{
			actions.Enable();
		}

		protected virtual void OnDisable()
		{
			actions.Disable();
		}

		private Vector2 locomotion;

		protected void Update()
		{
			var target = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			ShipEntity.Target = Vector3.ProjectOnPlane(target, Vector3.forward);
			ShipEntity.Translation = locomotion;
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				foreach (var weapon in ShipEntity.WeaponEntities)
				{
					weapon.Firing = true;
				}
			}

			if (context.canceled)
			{
				foreach (var weapon in Entity.WeaponEntities)
				{
					weapon.Firing = false;
				}
			}
		}

		public void OnLocomotion(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}
	}
}
