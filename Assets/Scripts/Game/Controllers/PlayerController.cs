using Celeritas.Game.Entities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Game.Controllers
{
	/// <summary>
	/// Routes player input to a targeted ship Entity.
	/// </summary>
	[RequireComponent(typeof(ShipEntity))]
	public class PlayerController : Singleton<PlayerController>, InputActions.IBasicActions
	{
		private InputActions.BasicActions actions = default;
		private Camera _camera;

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public ShipEntity ShipEntity { get; private set; }

		protected override void Awake()
		{
			actions = new InputActions.BasicActions(new InputActions());
			actions.SetCallbacks(this);

			ShipEntity = GetComponent<ShipEntity>();

			_camera = Camera.main;

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
			TractorLogic();
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
				foreach (var weapon in ShipEntity.WeaponEntities)
				{
					weapon.Firing = false;
				}
			}
		}

		public void OnLocomotion(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}

		public void OnBuild(InputAction.CallbackContext context)
		{
			if (context.canceled)
			{
				if (CameraStateManager.IsInState(CameraStateManager.States.PLAY))
				{
					CameraStateManager.ChangeTo(CameraStateManager.States.BUILD);
				}
				else
				{
					CameraStateManager.ChangeTo(CameraStateManager.States.PLAY);
				}
			}
		}

		public void OnAction(InputAction.CallbackContext context)
		{
			ShipEntity.UseActions();
		}

		ShipEntity tractorTarget;
		int tractorRange = 10; // radius
		float tractorForceMultiplier = 25;
		bool tractorActive = false;
		float tractorForceCap = 10;
		float tractorDeadZoneRadius = 1; // if an object is this close to the cursor, tractor will stop applying force

		/// <summary>
		/// Pull tractorTarget around if tractor beam is active.
		/// </summary>
		private void TractorLogic()
		{
			if (tractorActive)
			{
				if (tractorTarget != null) // worth checking as not all clicks will be in range of valid target
				{
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

					// move target towards mouse w force proportional to distance
					//float distance = Vector2.Distance(tractorTarget.transform.position, mousePos);

					Vector2 dirToPull = mousePos - tractorTarget.transform.position;

					if (dirToPull.magnitude <= tractorDeadZoneRadius)
						return;

					Vector2 toApply = dirToPull * tractorForceMultiplier * dirToPull.magnitude;
					if (toApply.magnitude > tractorForceCap)
						toApply = toApply.normalized * tractorForceCap;

					tractorTarget.Rigidbody.AddForce(toApply);

				}
			}
		}

		public void OnTractorBeam(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				tractorActive = true;
				
				// consider factoring in mouse velocity for fun
				//Vector2 mousePos = context.ReadValue<Vector2>();
				Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

				// if no tractor target, find closest target within mouse range (if possible), and set it as tractorTarget
				if (tractorTarget == null)
				{
					// Note: duplicated logic from GravityWell

					// find all entities within radius, around the cursor
					List<Collider2D> withinRange = new List<Collider2D>();
					ContactFilter2D filter = new ContactFilter2D();
					filter.NoFilter();
					Physics2D.OverlapCircle(mousePos, tractorRange, filter, withinRange);

					float lowestDistance = tractorRange + 1;

					// find closest entity that is a ship
					foreach (Collider2D collider in withinRange)
					{
						Rigidbody2D body = collider.attachedRigidbody;
						if (body == null)
							continue;

						ShipEntity ship = body.GetComponent<ShipEntity>();
						if (ship == null)
							continue;

						float distance = Vector2.Distance(ship.transform.position, mousePos);
						if (distance < lowestDistance)
						{
							lowestDistance = distance;
							tractorTarget = ship;
						}
					}
				}
			}

			if (context.canceled)
			{ 
				tractorTarget = null;
				tractorActive = false;
			}
		}
	}
}
