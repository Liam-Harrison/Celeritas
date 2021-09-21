using Celeritas;
using Celeritas.Game;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.Controllers
{
	/// <summary>
	/// Logic for the Tractor Beam
	/// Currently relies on the PlayerController to call it when the player right clicks.
	/// </summary>
	class TractorBeamController : Singleton<TractorBeamController>
	{
		
		[SerializeField, TitleGroup("Tractor Beam Settings")] int TRACTOR_RANGE = 10; // radius the tractor beam can reach, to lock onto a target
		[SerializeField, TitleGroup("Tractor Beam Settings")] float TRACTOR_FORCE_MULTIPLIER = 20f;
		[SerializeField, TitleGroup("Tractor Beam Settings")] float TRACTOR_FORCE_CAP = 1000; // max force tractor beam can apply
		[SerializeField, TitleGroup("Tractor Beam Settings")] float TRACTOR_DEAD_ZONE_RADIUS = 0.1f; // if an object is this close to the cursor, tractor will stop applying force

		bool tractorActive = false;
		ITractorBeamTarget tractorTarget; // the target the tractor beam is locked onto. Null if no valid target in range or tractor not active.
		Object tractorBeamEffectPrefab;
		private Camera _camera;

		[ReadOnly, TitleGroup("Used By Modules")] public float TargetMassMultiplier = 1; // multiplies, this is used for Module Logic Only

		public Object TractorBeamEffectPrefab { set => TractorBeamEffectPrefab = value; }

		protected override void Awake()
		{
			_camera = Camera.main;
			tractorBeamEffectPrefab = Resources.Load("TractorBeamEffect");
			base.Awake();
		}

		protected void Update()
		{
			TractorLogic();
		}

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
					
					// og logic, here just in case
					//Vector2 toApply = dirToPull * TRACTOR_FORCE_MULTIPLIER * dirToPull.magnitude * TractorForceModifier / tractorTarget.Rigidbody.mass;

					float effectiveMass = tractorTarget.Rigidbody.mass * TargetMassMultiplier;
					if (effectiveMass == 0) // no dividing by zero
						effectiveMass = 0.01f;

					Vector2 toApply = dirToPull * dirToPull.magnitude * TRACTOR_FORCE_MULTIPLIER / effectiveMass;
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
		public void OnTractorBeam(InputAction.CallbackContext context, PlayerShipEntity playerShip)
		{
			if (context.performed)
			{
				// toggle tractorActive on
				tractorActive = true;

				// if no tractor target, find closest target within mouse range (if possible), and set it as tractorTarget
				if (tractorTarget == null)
				{
					if (_camera == null) // if you run directly from the Game scene, this avoids null errors.
						Awake();

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
						if (target != null && (ShipEntity)target == playerShip) // can't tractor beam yourself
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

	}
}
