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
		[SerializeField, TitleGroup("Tractor Beam Settings")] int TRACTOR_RANGE_BASE;// = 10; // radius the tractor beam can reach, to lock onto a target
		[SerializeField, TitleGroup("Tractor Beam Settings")] float TRACTOR_FORCE_MULTIPLIER;// = 20f;
		[SerializeField, TitleGroup("Tractor Beam Settings")] float TRACTOR_FORCE_CAP; //= 1000; // max force tractor beam can apply
		[SerializeField, TitleGroup("Tractor Beam Settings")] int MAX_NUMBER_OF_SIMULTANEOUS_TARGETS;// = 99;


		bool tractorActive = false;
		List<ITractorBeamTarget> tractorTargets; // the target the tractor beam is locked onto. Null if no valid target in range or tractor not active.
		Object tractorBeamEffectPrefab;
		List<Object> tractorGraphicalEffects;
		private Camera _camera;

		[ReadOnly, TitleGroup("Used By Modules")] public float TargetMassMultiplier = 1; // multiplies, this is used for Module Logic Only
		[ReadOnly, TitleGroup("Used By Modules")] bool usesAreaOfEffect = false;
		[ReadOnly, TitleGroup("Used By Modules")] float effectiveTractorRange;

		[SerializeField, TitleGroup("Tractor Beam Settings")] AnimationCurve forceToApplyDependingOnDistance;
		[SerializeField, TitleGroup("Tractor Beam Settings")] float distanceFromMouseMultiplier; // used for animation curve evaluation
		[SerializeField, TitleGroup("Tractor Beam Settings")] float tractoredObjectsDrag; // = 0.75f;

		public Object TractorBeamEffectPrefab { set => TractorBeamEffectPrefab = value; }

		protected override void Awake()
		{
			_camera = Camera.main;
			tractorBeamEffectPrefab = Resources.Load("TractorBeamEffect");
			tractorTargets = new List<ITractorBeamTarget>();
			tractorGraphicalEffects = new List<Object>();
			effectiveTractorRange = TRACTOR_RANGE_BASE;

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
		public void UseAreaOfEffect(bool active, float rangeMultiplier = 1)
		{
			usesAreaOfEffect = active;
			effectiveTractorRange *= rangeMultiplier;
		}

		/// <summary>
		/// Toggles tractorActive on/off when corrosponding input is pressed/released.
		/// Also finds the closest entity/ies within range, stores it/them in tractorTarget
		/// </summary>
		/// <param name="context"></param>
		public void OnTractorBeam(InputAction.CallbackContext context, PlayerShipEntity playerShip)
		{
			if (context.performed)
			{
				// toggle tractorActive on
				tractorActive = true;

				// if using area of effect, set max number of targets. Otherwise only process 1.
				int maxNumberOfTargetsToProcess = 1;
				if (usesAreaOfEffect) { maxNumberOfTargetsToProcess = MAX_NUMBER_OF_SIMULTANEOUS_TARGETS; }


				// if no tractor target, find closest target(s) within mouse range (if possible), and set it in tractorTarget
				if (tractorTargets.Count == 0)
				{

					if (_camera == null) // if you run directly from the Game scene, this avoids null errors.
						Awake();


					// find all entities within radius, around the cursor
					Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
					List<Collider2D> withinRange = new List<Collider2D>();
					ContactFilter2D filter = new ContactFilter2D();
					filter.NoFilter();
					Physics2D.OverlapCircle(mousePos, effectiveTractorRange, filter, withinRange);

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

						if (target is ShipEntity || target is Asteroid) { // if in range and valid object, make it a tractor target
							//tractorTargets.Add(target);

							if (!targets.ContainsKey(distance))
								targets.Add(distance, target);
						}
					}

					
					int i = 0;
					foreach (KeyValuePair<float, ITractorBeamTarget> kvp in targets) // sorted list used to grab entities in order of proximity
					{
						if (i >= maxNumberOfTargetsToProcess) // once you have all the targets you want, break
						{
							break;
						}

						tractorTargets.Add(kvp.Value);

						i++;
						
					}

				}

				// do graphical things for all tractored targets
				if (tractorTargets.Count != 0)
				{
					CombatHUD.Instance.TractorAimingLine.SetActive(true);
					CombatHUD.Instance.TractorAimingLine.GetComponent<AimingLine>().TargetToAimAt = tractorTargets[0].Rigidbody;

					foreach (ITractorBeamTarget t in tractorTargets)
					{
						if (t == null)
							break;

						tractorGraphicalEffects.Add(Instantiate(tractorBeamEffectPrefab, t.Rigidbody.transform));
					}

				}
			}

			if (context.canceled)
			{
				foreach (ITractorBeamTarget t in tractorTargets)
					if (t != null) t.Rigidbody.drag = 0.1f;

				tractorTargets.Clear();
				
				tractorActive = false;
				foreach (Object o in tractorGraphicalEffects)
				{
					if (o != null) Destroy(o);
				}
				tractorGraphicalEffects.Clear();

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
					
					float effectiveMass = t.Rigidbody.mass * TargetMassMultiplier;
					if (effectiveMass == 0) // no dividing by zero
						effectiveMass = 0.01f;

					t.Rigidbody.drag = tractoredObjectsDrag;

					Vector2 toApply = dirToPull * forceToApplyDependingOnDistance.Evaluate(dirToPull.magnitude * distanceFromMouseMultiplier) * TRACTOR_FORCE_MULTIPLIER / effectiveMass;

					// old logic, here just in case
					// move target towards mouse w force proportional to distance ^ 2 ( == dirToPull * dirToPull.magnitude)
					//Vector2 toApply = dirToPull * TRACTOR_FORCE_MULTIPLIER * dirToPull.magnitude * TractorForceModifier / tractorTarget.Rigidbody.mass;

					if (toApply.magnitude > TRACTOR_FORCE_CAP)
						toApply = toApply.normalized * TRACTOR_FORCE_CAP;

					t.Rigidbody.AddForce(toApply);
				}
			}
		}

	}
}
