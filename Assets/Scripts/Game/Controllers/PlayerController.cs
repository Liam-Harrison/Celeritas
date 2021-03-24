using Celeritas.Extensions;
using Celeritas.Game.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Game.Controllers
{
	public class PlayerController : MonoBehaviour, Actions.IBasicActions
	{
		private Actions.BasicActions actions = default;
		private Camera _camera;
		private ShipEntity entity;

		protected virtual void Awake()
		{
			actions = new Actions.BasicActions(new Actions());
			actions.SetCallbacks(this);
			entity = GetComponent<ShipEntity>();
			_camera = Camera.main;
			_camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
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
			target = Vector3.ProjectOnPlane(target, Vector3.forward);
			entity.Target = (target - transform.position).normalized;

			entity.Translation = locomotion;
		}

		public void OnFire(InputAction.CallbackContext context)
		{

		}

		public void OnLocomotion(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}
	}
}
