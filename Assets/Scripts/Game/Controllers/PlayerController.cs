using Celeritas.Game.Entities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Game.Controllers
{
	/// <summary>
	/// Routes player input to a targeted ship Entity.
	/// </summary>
	[RequireComponent(typeof(PlayerShipEntity))]
	public class PlayerController : Singleton<PlayerController>, InputActions.IBasicActions
	{
		private InputActions.BasicActions actions = default;
		private Camera _camera;

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public PlayerShipEntity PlayerShipEntity { get; private set; }

		private Object tractorBeamEffectPrefab;

		protected override void Awake()
		{
			actions = new InputActions.BasicActions(new InputActions());
			actions.SetCallbacks(this);

			PlayerShipEntity = GetComponent<PlayerShipEntity>();

			_camera = Camera.main;

			tractorBeamEffectPrefab = Resources.Load("TractorBeamEffect");

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
			if (GameStateManager.Instance.GameState == GameState.BUILD)
			{
				PlayerShipEntity.Target = transform.position + PlayerShipEntity.Forward * 50f;
				PlayerShipEntity.Translation = Vector3.zero;
			}
			else
			{
				var target = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				PlayerShipEntity.Target = Vector3.ProjectOnPlane(target, Vector3.forward);
				PlayerShipEntity.Translation = locomotion;
				TractorLogic();
			}
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				foreach (var weapon in PlayerShipEntity.WeaponEntities)
				{
					weapon.Firing = true;
				}
			}

			if (context.canceled)
			{
				foreach (var weapon in PlayerShipEntity.WeaponEntities)
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
			if (context.canceled && !WaveManager.Instance.WaveActive)
			{
				if (GameStateManager.Instance.GameState == GameState.BACKGROUND)
				{
					GameStateManager.Instance.SetGameState(GameState.BUILD);
				}
				else
				{
					GameStateManager.Instance.SetGameState(GameState.BACKGROUND);
				}
			}
		}

		public void OnAction(InputAction.CallbackContext context)
		{
			PlayerShipEntity.UseActions();
		}

		int TRACTOR_RANGE = 10; // radius the tractor beam can reach, to lock onto a target
		float TRACTOR_FORCE_MULTIPLIER = 0.5f;
		float TRACTOR_FORCE_CAP = 100; // max force tractor beam can apply
		float TRACTOR_DEAD_ZONE_RADIUS = 1; // if an object is this close to the cursor, tractor will stop applying force

		bool tractorActive = false; 
		ITractorBeamTarget tractorTarget; // the target the tractor beam is locked onto. Null if no valid target in range or tractor not active.

		/// <summary>
		/// Pull tractorTarget around if tractor beam is active.
		/// </summary>
		private void TractorLogic()
		{
			if (tractorActive)
			{
				if (tractorTarget != null && tractorTarget.Rigidbody != null)
				{
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					Vector2 dirToPull = mousePos - tractorTarget.Rigidbody.transform.position;

					// don't pull target if it's close to mouse (ie, in 'deadzone')
					if (dirToPull.magnitude <= TRACTOR_DEAD_ZONE_RADIUS) 
						return;

					// move target towards mouse w force proportional to distance ^ 2 ( == dirToPull * dirToPull.magnitude)
					Vector2 toApply = dirToPull * TRACTOR_FORCE_MULTIPLIER * dirToPull.magnitude;
					if (toApply.magnitude > TRACTOR_FORCE_CAP)
						toApply = toApply.normalized * TRACTOR_FORCE_CAP;

					tractorTarget.Rigidbody.AddForce(toApply);

				}
			}
		}

		Object tractorGraphicalEffect;

		/// <summary>
		/// Toggles tractorActive on/off when corrosponding input is pressed/released.
		/// Also finds the closest entity within range, sets it as tractorTarget
		/// </summary>
		/// <param name="context"></param>
		public void OnTractorBeam(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				// toggle tractorActive on
				tractorActive = true;

				// if no tractor target, find closest target within mouse range (if possible), and set it as tractorTarget
				if (tractorTarget == null)
				{
					// Note: duplicated logic from GravityWell

					// find all entities within radius, around the cursor
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					List<Collider2D> withinRange = new List<Collider2D>();
					ContactFilter2D filter = new ContactFilter2D();
					filter.NoFilter();
					Physics2D.OverlapCircle(mousePos, TRACTOR_RANGE, filter, withinRange);

					float lowestDistance = TRACTOR_RANGE + 1;

					// loop through all entities, set the closest ship (within range) as tractorTarget
					foreach (Collider2D collider in withinRange)
					{
						Rigidbody2D body = collider.attachedRigidbody;
						if (body == null)
							continue;

						ITractorBeamTarget target = body.GetComponent<ShipEntity>();
						if (target != null && (ShipEntity)target == PlayerShipEntity) // can't tractor beam yourself
							continue;

						if (target == null) // if not a ship, check if its an asteroid
							target = body.GetComponent<Asteroid>();

						if (target == null) // if not a ship or asteroid, move on to next target
							continue;

						float distance = Vector2.Distance(target.Rigidbody.transform.position, mousePos);
						if (distance < lowestDistance)
						{
							lowestDistance = distance;
							tractorTarget = target;
						}
					}
				}

				if (tractorTarget != null)
				{
					// add graphical effect
					tractorGraphicalEffect = Instantiate(tractorBeamEffectPrefab, tractorTarget.Rigidbody.transform);
					CombatHUD.Instance.TractorAimingLine.SetActive(true);
					CombatHUD.Instance.TractorAimingLine.GetComponent<AimingLine>().TargetToAimAt = tractorTarget.Rigidbody;
					// todo: scale effect depending on size of ship.
				}
			}

			if (context.canceled)
			{ 
				tractorTarget = null;
				tractorActive = false;
				if (tractorGraphicalEffect != null)
				{
					Destroy(tractorGraphicalEffect);
					CombatHUD.Instance.TractorAimingLine.SetActive(false);
				}
			}
		}

		public void OnNewWave(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				WaveManager.Instance.StartRandomWave();
			}
		}
		public void OnFinalWave(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				WaveManager.Instance.StartFinalWave();
			}
		}



		public void OnToggleTutorial(InputAction.CallbackContext context)
		{
			CombatHUD.Instance.OnToggleTutorial();
		}
	}
}
