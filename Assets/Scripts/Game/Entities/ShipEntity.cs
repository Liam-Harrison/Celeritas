using Celeritas.AI;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	[System.Serializable]
	public enum DropType
	{
		Ship,
		Boss,
		Asteroid
	}

	/// <summary>
	/// The game entity for a ship.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipEntity : Entity, ITractorBeamTarget
	{
		[SerializeField, Title("Modules")]
		protected List<Module> modules = new List<Module>();

		[SerializeField, Title("Settings")]
		protected MovementModifier movementModifier;

		[SerializeField]
		protected EntityStatBar health;

		[SerializeField]
		protected EntityStatBar shield;

		[SerializeField, Title("Loot")]
		private LootConfig lootConfig;

		[SerializeField, DisableInPlayMode]
		private DropType dropType;

		[SerializeField, Title("Effects")]
		protected ParticleSystem[] engineEffects;

		[SerializeField]
		protected float collisionDamageMod = 1;

		/// <summary>
		/// The entity's health data
		/// </summary>
		public EntityStatBar Health { get => health; }

		/// <summary>
		/// The entity's shield data
		/// </summary>
		public EntityStatBar Shield { get => shield; }

		/// <summary>
		/// The movement modifier used by this ship.
		/// </summary>
		public MovementModifier MovementModifier { get => movementModifier; }

		/// <summary>
		/// The loot drop rate of the ship.
		/// </summary>
		public LootConfig LootConfig { get => lootConfig; }

		/// <summary>
		/// The modules attatched to this ship.
		/// </summary>
		public List<Module> Modules { get => modules; }

		/// <summary>
		/// The item drop information of the ship.
		/// </summary>
		public DropType DropType { get => dropType; }

		/// <summary>
		/// Get all the module entities attatched to this ship.
		/// </summary>
		public List<ModuleEntity> ModuleEntities
		{
			get
			{
				var list = new List<ModuleEntity>();
				foreach (var module in Modules)
				{
					if (module.HasModuleAttatched)
						list.Add(module.AttatchedModule);
				}
				return list;
			}
		}

		/// <summary>
		/// Get all the weapon entities attatched to this ship.
		/// </summary>
		public List<WeaponEntity> WeaponEntities
		{
			get
			{
				var list = new List<WeaponEntity>();
				foreach (var module in Modules)
				{
					if (module.HasModuleAttatched && module.AttatchedModule is WeaponEntity)
						list.Add(module.AttatchedModule as WeaponEntity);
				}
				return list;
			}
		}
		
		/// <summary>
		/// The attatched ship data.
		/// </summary>
		public ShipData ShipData { get; private set; }

		/// <summary>
		/// The ships Rigidbody.
		/// </summary>
		public Rigidbody2D Rigidbody { get; private set; }

		/// <summary>
		/// The current aim target for this ship.
		/// </summary>
		public Vector3 AimTarget { get; set; }

		/// <summary>
		/// The current translation input for this ship.
		/// </summary>
		public Vector3 Translation { get; set; }

		/// <summary>
		/// The current velocity of this ship.
		/// </summary>
		public Vector3 Velocity { get; private set; }

		/// <summary>
		/// Get the AI attatched to this ship, if any.
		/// </summary>
		public AIBase AttatchedAI { get; private set; }

		/// <summary>
		/// Check if this ship entity has an AI attatched.
		/// </summary>
		public bool HasAIAttatched { get => AttatchedAI != null; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Ship; }

		/// <summary>
		/// Is this ship entity stationary or not.
		/// </summary>
		public bool IsStationary { get; set; }

		/// <summary>
		/// Determines if the ship is currently stunned.
		/// </summary>
		public bool Stunned { get; set; }
		
		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			base.Initalize(data, owner, effects, forceIsPlayer);

			Rigidbody = GetComponent<Rigidbody2D>();
			ShipData = data as ShipData;

			var ai = GetComponent<AIBase>();
			if (ai != null && instanced == false)
				AttatchToAI(ai);

			if (instanced == false)
			{
				health = new EntityStatBar(ShipData.StartingHealth);
				shield = new EntityStatBar(ShipData.StartingShield);

				ApplyRigidbodySettings();

				foreach (var module in modules)
				{
					module.Initalize(this);
				}
			}

			Stunned = false;
			IsStationary = false;

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		public override void OnSpawned()
		{
			if (ShipData != null)
			{
				foreach (var module in modules)
				{
					module.Initalize(this);
				}
			}

			base.OnSpawned();
		}

		public override void OnDespawned()
		{
			GenerateLootDrop();
			base.OnDespawned();
		}

		protected override void Update()
		{
			if (!IsInitalized)
				return;

			EngineLogic();

			base.Update();
		}

		protected virtual void FixedUpdate()
		{
			if (!IsInitalized)
				return;

			if (IsStationary == false)
			{
				TranslationLogic();
				RotationLogic();
			}

			shieldDelayTimer = Mathf.Max(shieldDelayTimer - Time.deltaTime, 0f);
			RegenShield();
		}

		/// <summary>
		/// Register a collision between this entity and the other target entity.
		/// </summary>
		/// <param name="other"></param>
		public override void OnEntityHit(Entity other)
		{
			ApplyCollisionDamage(Rigidbody, other);

			base.OnEntityHit(other);
		}

		[SerializeField]
		Explosion explosionScript;
		/// <summary>
		/// Register damage to this entity and update it internally.
		/// </summary>
		/// <param name="attackingEntity">The entity which has attacked.</param>
		/// <param name="damage">The amount of damage to take.</param>
		public override void TakeDamage(Entity attackingEntity, float damage)
		{
			if (attackingEntity is ProjectileEntity || attackingEntity is ShipEntity || attackingEntity is Asteroid || attackingEntity == this)
			{
				base.TakeDamage(attackingEntity);

				float calculatedDamage = CalculateDamage(damage);

				if (attackingEntity is Asteroid)
				{
					calculatedDamage = calculatedDamage * collisionDamageMod;
				}

				shieldDelayTimer = shieldRegenDelay;

				// if damage will go beyond shields
				if (calculatedDamage > shield.CurrentValue)
				{
					// reduce damage by shield amount, reduce shield to 0
					calculatedDamage -= shield.CurrentValue;
					shield.Damage(shield.CurrentValue);

					// damage health with remaining amount
					health.Damage(calculatedDamage);
				}
				else
				{
					// damage won't go beyond shields
					shield.Damage(calculatedDamage);
				}

				ShowDamage(calculatedDamage);

				if (health.IsEmpty())
				{
					if (explosionScript != null)
						explosionScript.Explode();
					else
						KillEntity();
				}
			}
		}

		/// <summary>
		/// The amount of shields is regenerated per second
		/// </summary>
		private float shieldRegenAmount = 50.0f;
		[SerializeField, Title("Base Shield Regeneration Amount", "The base amount of shields that is regenerated per second.")]
		public float ShieldRegenAmount { get => shieldRegenAmount; set => shieldRegenAmount = value; }


		/// <summary>
		/// The amount of time the shield regeneration is delayed by after taking damage
		/// </summary>
		private float shieldRegenDelay = 4.0f;
		[SerializeField, Title("Base Shield Regeneration Delay", "The timer in which the shield will not regenerate after taking damage.")]
		public float ShieldRegenDelay { get => shieldRegenDelay; set => shieldRegenDelay = value; }

		/// <summary>
        /// Timer for the delayed shield regen
        /// </summary>
		private float shieldDelayTimer = 0.0f;

		/// <summary>
		/// Number of seconds between shield regen.
		/// </summary>
		private float timeBetweenShieldRegen = 1.0f;
		public float TimeBetweenShieldRegen { get => timeBetweenShieldRegen; set => timeBetweenShieldRegen = value; }

		private void RegenShield()
		{
			if (shieldDelayTimer == 0f)
			{
				if (shield.CurrentValue < shield.MaxValue)
				{
					shield.Damage(Mathf.RoundToInt(shieldRegenAmount * Time.smoothDeltaTime * -1));
				}
			}
		}

		/// <summary>
		/// Starts coroutine for stun
		/// </summary>
		public void Stun(float duration)
		{
			StartCoroutine(StunTimer(duration));
		}

		/// <summary>
        /// Coroutine that will set the ship to stationary for duration.
        /// </summary>
		public IEnumerator StunTimer(float duration)
		{
			Stunned = true;
			IsStationary = true;
			yield return new WaitForSeconds(duration);
			IsStationary = false;
			Stunned = false;
		}

		public void Immortality(float duration)
		{
			StartCoroutine(ImmortalityTimer(duration));
		}

		public IEnumerator ImmortalityTimer(float duration)
		{
			float currentDamageModifier = DamageModifierPercentage;
			DamageModifierPercentage = -1.0f;
			Debug.Log(DamageModifierPercentage);
			yield return new WaitForSeconds(duration);
			DamageModifierPercentage = currentDamageModifier;
			Debug.Log(DamageModifierPercentage);
		}

		/// <summary>
		/// Current damage modifer on ship.
		/// Default is 0, negative value = less damage, positive value = takes more damage.
		/// </summary>
		private float damageModifierPercentage = 0;

		public float DamageModifierPercentage { get => damageModifierPercentage; set => damageModifierPercentage = value; }

		/// <summary>
		///	Calculates the amount of damage to apply after the damage modifier has been applied.
		/// </summary>
		/// <param name="damage">The original amount of damage taken.</param>
		/// <returns>The amount of damage to take after the damage modifier has been applied</returns>
		private float CalculateDamage(float damage)
		{
			return damage + (damage * damageModifierPercentage);
		}

		/// <summary>
		/// Does this entity have a currently attatched module entity.
		/// </summary>
		/// <param name="module">The module to check for.</param>
		/// <returns>Returns true if an entity was found with this module data, otherwise false.</returns>
		public bool HasModule(ModuleData module)
		{
			foreach (var item in ModuleEntities)
			{
				if (item.ModuleData == module)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Try to get a module entity attatched to this ship entity.
		/// </summary>
		/// <param name="module">The module to look for.</param>
		/// <param name="entity">The found entity if true, otherwise null.</param>
		/// <returns>Returns true if found, otherwise false.</returns>
		public bool TryGetModuleEntity(ModuleData module, out ModuleEntity entity)
		{
			foreach (var item in ModuleEntities)
			{
				if (item.ModuleData == module)
				{
					entity = item;
					return true;
				}
			}
			entity = null;
			return false;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!IsInitalized)
				return;

			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Velocity);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, AimTarget);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + Forward);
		}

		public void AttatchToAI(AIBase ai)
		{
			AttatchedAI = ai;
			AttatchedAI.OnAttatched();
		}

		private void TranslationLogic()
		{
			Velocity = Vector3.up * ((Mathf.Max(Translation.y, 0) * ShipData.MovementSettings.forcePerSec * movementModifier.Forward) +
							(Mathf.Min(Translation.y, 0) * ShipData.MovementSettings.forcePerSec * movementModifier.Back)) * Time.fixedDeltaTime;

			Velocity += Vector3.right * Translation.x * ShipData.MovementSettings.forcePerSec * movementModifier.Side * Time.fixedDeltaTime;

			Rigidbody.AddForce(Velocity, ForceMode2D.Force);
		}

		private void RotationLogic()
		{
			var dir = (AimTarget - transform.position).normalized;
			var dot = Vector3.Dot(Forward, dir);

			if (dot < ShipData.MovementSettings.aimDeadzone)
			{
				var torquePerSec = ShipData.MovementSettings.torquePerSec * movementModifier.Rotation;
				var torque = Mathf.Lerp(torquePerSec.x, torquePerSec.y, ShipData.MovementSettings.rotationCurve.Evaluate(Mathf.InverseLerp(1, -1, dot))) * Time.smoothDeltaTime;

				if (Vector3.Dot(Right, dir) >= 0)
					torque = -torque;

				var absAngular = Mathf.Abs(Rigidbody.angularVelocity);
				if (absAngular < ShipData.MovementSettings.rotationMaximum || (absAngular < 1 && Mathf.Sign(torque) != Mathf.Sign(Rigidbody.angularVelocity)))
				{
					Rigidbody.AddTorque(torque, ForceMode2D.Force);
				}
			}
			else
			{
				var rot = Quaternion.LookRotation(Vector3.forward, dir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, ShipData.MovementSettings.rotationMaximum * Time.smoothDeltaTime);
			}
		}


		private const int MIN_ENGINE_EMISSION = 8;
		private const int MAX_ENGINE_EMISSION = 50;

		private void EngineLogic()
		{
			var rate = Mathf.Lerp(MIN_ENGINE_EMISSION, MAX_ENGINE_EMISSION, Mathf.Clamp01(Rigidbody.velocity.magnitude / 10));

			if (GameStateManager.Instance.GameState == GameState.BUILD)
				rate = 0;

			foreach (var system in engineEffects)
			{
				var emission = system.emission;
				emission.rateOverTime = rate;
			}
		}

		protected void ApplyRigidbodySettings()
		{
			Rigidbody.gravityScale = 0;
			Rigidbody.angularDrag = ShipData.MovementSettings.angularDrag;
			Rigidbody.mass = ShipData.MovementSettings.mass;
		}

		private void GenerateLootDrop()
		{
			float gain = LootConfig.Gain;
			dropType = DropType.Ship;

			if (gain != 0)
			{
				if (LootController.Instance != null)
					LootController.Instance.LootDrop(gain, dropType, Position);
			}
		}

		public void DeployNuke(GameObject prefab, float damage, float duration)
		{
			GameObject nuke = Instantiate(prefab, this.transform.position, this.transform.rotation);
			nuke.GetComponent<NukeScript>().damage = damage;
			nuke.GetComponent<NukeScript>().delay = duration;
			nuke.GetComponent<NukeScript>().delaySet = true;
		}

		public void DeployMines(GameObject prefab, float damage, float duration, float amount)
		{
			float i = 0;
			while (i < amount)
			{
				var position = new Vector3(this.transform.position.x + Random.Range(amount * -1.5f, amount * 1.5f), this.transform.position.y + Random.Range(amount * -1.5f, amount * 1.5f), this.transform.position.z);
				GameObject landmine = Instantiate(prefab, position, this.transform.rotation);
				landmine.GetComponent<LandmineScript>().initialize(damage, duration);
				i = i + 1;
			}
		}
	}

	/// <summary>
	/// Modifies existing ship paramaters.
	/// </summary>
	[System.Serializable]
	public class MovementModifier
	{
		private const int RANGE = 4;

		[SerializeField, PropertyRange(-1, RANGE), DisableInPlayMode, Title("Movement Modifiers")]
		private float forward = 1;

		[SerializeField, PropertyRange(-1, RANGE), DisableInPlayMode]
		private float side = 1;

		[SerializeField, PropertyRange(-1, RANGE), DisableInPlayMode]
		private float back = 1;

		[SerializeField, PropertyRange(-1, RANGE), DisableInPlayMode]
		private float rotation = 1;

		/// <summary>
		/// The forward modifier of this ship.
		/// </summary>
		public float Forward { get => forward; set => forward = Mathf.Clamp(value, -1, RANGE); }

		/// <summary>
		/// The side modifier of this ship.
		/// </summary>
		public float Side { get => side; set => side = Mathf.Clamp(value, -1, RANGE); }

		/// <summary>
		/// The back modifier of this ship.
		/// </summary>
		public float Back { get => back; set => back = Mathf.Clamp(value, -1, RANGE); }

		/// <summary>
		/// The rotation modifier of this ship.
		/// </summary>
		public float Rotation { get => rotation; set => rotation = Mathf.Clamp(value, -1, RANGE); }
	}

	/// <summary>
	/// Determines the value and chance of item drops.
	/// </summary>
	[System.Serializable]
	public class LootConfig
	{
		private const int RANGE = 100;

		[SerializeField, PropertyRange(0, RANGE), DisableInPlayMode, Title("Item Drop Modifiers")]
		private float gain = 1;

		[SerializeField, DisableInPlayMode, Title("Is Enemy Boss")]
		private bool isBoss = false;

		/// <summary>
		/// The base loot value of the ship.
		/// </summary>
		public float Gain { get => gain; set => gain = Mathf.Clamp(value, 0, RANGE); }

		/// <summary>
		/// Check if enemy is a boss to modify drops.
		/// </summary>
		public bool IsBoss { get => isBoss; set => isBoss = value; }
	}

}
