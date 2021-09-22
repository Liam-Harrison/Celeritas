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
		[SerializeField, TitleGroup("Tractor Beam Settings")] int MAX_NUMBER_OF_SIMULTANEOUS_TARGETS = 10;


		bool tractorActive = false;
		ITractorBeamTarget[] tractorTargets; // the target the tractor beam is locked onto. Null if no valid target in range or tractor not active.
		Object tractorBeamEffectPrefab;
		private Camera _camera;

		[ReadOnly, TitleGroup("Used By Modules")] public float TargetMassMultiplier = 1; // multiplies, this is used for Module Logic Only
		[ReadOnly, TitleGroup("Used By Modules")] bool usesAreaOfEffect = false;
		[ReadOnly, TitleGroup("Used By Modules")] float radiusOfEffect = 1f;

		public Object TractorBeamEffectPrefab { set => TractorBeamEffectPrefab = value; }

		protected override void Awake()
		{
			_camera = Camera.main;
			tractorBeamEffectPrefab = Resources.Load("TractorBeamEffect");
			tractorTargets = new ITractorBeamTarget[MAX_NUMBER_OF_SIMULTANEOUS_TARGETS];
			tractorGraphicalEffects = new Object[MAX_NUMBER_OF_SIMULTANEOUS_TARGETS];
			UseAreaOfEffect(true, 10); // just for testing, change back

			base.Awake();
		}

		protected void Update()
		{
			TractorLogic();
		}

		/// <summary>
		/// Set Tractor Beam to use / not use its area of effect logic
		/// </summary>
		/// <param name="active">true = set tractor beam to use area of effect</param>
		/// <param name="radiusOfEffect">Radius of the AoE (will grab all non-player entities within this radius of the target)</param>
		public void UseAreaOfEffect(bool active, float radiusOfEffect)
		{
			usesAreaOfEffect = active;
		}

		Object[] tractorGraphicalEffects;

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

				// if using area of effect, process max of 10 targets. Otherwise only process 1.
				int maxNumberOfTargetsToProcess = 1;
				if (usesAreaOfEffect) maxNumberOfTargetsToProcess = MAX_NUMBER_OF_SIMULTANEOUS_TARGETS;


				// if no tractor target, find closest target within mouse range (if possible), and set it as tractorTarget
				if (tractorTargets[0] == null)
				{

					if (_camera == null) // if you run directly from the Game scene, this avoids null errors.
						Awake();


					// find all entities within radius, around the cursor
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					List<Collider2D> withinRange = new List<Collider2D>();
					ContactFilter2D filter = new ContactFilter2D();
					filter.NoFilter();
					Physics2D.OverlapCircle(mousePos, TRACTOR_RANGE, filter, withinRange);

					float LowestDistanceFound = TRACTOR_RANGE + 1;

					SortedList<float, ITractorBeamTarget> targets = new SortedList<float, ITractorBeamTarget>();

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

						foreach (KeyValuePair<float, ITractorBeamTarget> kvp in targets) { Debug.Log($"{kvp.Value},{kvp.Key}"); }
						Debug.Log(targets.Count);

						if (target is ShipEntity || target is Asteroid) {
							if (!targets.ContainsKey(distance))
								targets.Add(distance, target);
						}

						/*
						if (distance < LowestDistanceFound)
						{
							LowestDistanceFound = distance;
							tractorTargets[0] = target;
						}*/
					}

					int i = 0;
					foreach (KeyValuePair<float, ITractorBeamTarget> kvp in targets)
					{
						tractorTargets[i] = kvp.Value;

						i++;
						if (i > maxNumberOfTargetsToProcess)
							break;
					}

				}

				if (tractorTargets != null)
				{
					CombatHUD.Instance.TractorAimingLine.SetActive(true);
					CombatHUD.Instance.TractorAimingLine.GetComponent<AimingLine>().TargetToAimAt = tractorTargets[0].Rigidbody;

					int i = 0;
					foreach (ITractorBeamTarget t in tractorTargets)
					{
						if (t == null)
							break;
						// add graphical effect
						tractorGraphicalEffects[i] = Instantiate(tractorBeamEffectPrefab, t.Rigidbody.transform);
						
						// todo: scale effect depending on size of ship.
						i++;
					}

				}
			}

			if (context.canceled)
			{ 
				tractorTargets = new ITractorBeamTarget[MAX_NUMBER_OF_SIMULTANEOUS_TARGETS];
				tractorActive = false;
				foreach (Object o in tractorGraphicalEffects)
				{
					if (o != null) Destroy(o);
				}

				CombatHUD.Instance.TractorAimingLine.SetActive(false);
			}

		}

		/// <summary>
		/// Pull tractorTarget around if tractor beam is active.
		/// </summary>
		private void TractorLogic()
		{
			if (tractorActive)
			{
				foreach (ITractorBeamTarget t in tractorTargets)
				{
					if (t == null)
						break;
					if (t.Rigidbody == null)
						continue;

					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					Vector2 dirToPull = mousePos - t.Rigidbody.transform.position;

					// don't pull target if it's close to mouse (ie, in 'deadzone')
					if (dirToPull.magnitude <= TRACTOR_DEAD_ZONE_RADIUS)
						return;

					// move target towards mouse w force proportional to distance ^ 2 ( == dirToPull * dirToPull.magnitude)

					// og logic, here just in case
					//Vector2 toApply = dirToPull * TRACTOR_FORCE_MULTIPLIER * dirToPull.magnitude * TractorForceModifier / tractorTarget.Rigidbody.mass;

					float effectiveMass = t.Rigidbody.mass * TargetMassMultiplier;
					if (effectiveMass == 0) // no dividing by zero
						effectiveMass = 0.01f;

					Vector2 toApply = dirToPull * dirToPull.magnitude * TRACTOR_FORCE_MULTIPLIER / effectiveMass;
					if (toApply.magnitude > TRACTOR_FORCE_CAP)
						toApply = toApply.normalized * TRACTOR_FORCE_CAP;

					t.Rigidbody.AddForce(toApply);
				}
				/*
				if (tractorTargets != null && tractorTargets[0].Rigidbody != null)
				{
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					Vector2 dirToPull = mousePos - tractorTargets[0].Rigidbody.transform.position;

					// don't pull target if it's close to mouse (ie, in 'deadzone')
					if (dirToPull.magnitude <= TRACTOR_DEAD_ZONE_RADIUS)
						return;

					// move target towards mouse w force proportional to distance ^ 2 ( == dirToPull * dirToPull.magnitude)

					// og logic, here just in case
					//Vector2 toApply = dirToPull * TRACTOR_FORCE_MULTIPLIER * dirToPull.magnitude * TractorForceModifier / tractorTarget.Rigidbody.mass;

					float effectiveMass = tractorTargets[0].Rigidbody.mass * TargetMassMultiplier;
					if (effectiveMass == 0) // no dividing by zero
						effectiveMass = 0.01f;

					Vector2 toApply = dirToPull * dirToPull.magnitude * TRACTOR_FORCE_MULTIPLIER / effectiveMass;
					if (toApply.magnitude > TRACTOR_FORCE_CAP)
						toApply = toApply.normalized * TRACTOR_FORCE_CAP;

					tractorTargets[0].Rigidbody.AddForce(toApply);

				}*/
			}
		}

	}
}
