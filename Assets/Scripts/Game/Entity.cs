using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
using Celeritas.Game.Interfaces;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// The Entity class provides basic functionality for on-screen objects.
	/// </summary>
	public abstract class Entity : SerializedMonoBehaviour, IPooledObject<Entity>
	{
		private const float SCHEDULE_DIE_INVINCIBLE_TIME = 0.5f;

		[SerializeField, Title("Entity Settings"), DisableInPlayMode]
		private bool hasDefaultEffects;

		[SerializeField, ShowIf(nameof(hasDefaultEffects)), DisableInPlayMode]
		private EffectWrapper[] defaultEffects;

		[SerializeField, Title("Audio Effects", "(eg. on hit, on destroy)"), DisableInPlayMode]
		private bool hasAudio;

		[SerializeField, DisableInPlayMode]
		private EventReference onDeath;

		[SerializeField, DisableInPlayMode]
		private EventReference onSpawn;

		[SerializeField, Title("Graphical Effects", "(eg. on hit, on destroy)"), DisableInPlayMode]
		private bool hasGraphicalEffects;

		public ObjectPool<Entity> OwningPool { get; set; }

		/// <summary>
		/// Effect that will play when the entity is destroyed.
		/// If null, no effect will play
		/// Note that entities despawning is different to them being destroyed (eg projectiles exceeding their lifetime)
		/// OnDestroy effects will not play when an entity is despawned, only when they are destroyed/killed.
		/// </summary>
		[SerializeField, Title("On Destroy Effect", "Time is in seconds"), ShowIf(nameof(hasGraphicalEffects)), DisableInPlayMode]
		private GameObject onDestroyEffectPrefab;

		/// <summary>
		/// How long the on destroy effect needs to be played.
		/// </summary>
		[SerializeField, ShowIf(nameof(hasGraphicalEffects)), DisableInPlayMode]
		private float timeToPlayOnDestroyEffect;

		/// <summary>
		/// Effect that will play when the entity is hit
		/// </summary>
		[SerializeField, Title("On Hit Effect", "Time is in seconds"), ShowIf(nameof(hasGraphicalEffects)), DisableInPlayMode]
		private GameObject onHitEffectPrefab;

		/// <summary>
		/// How long the on hit effect will play for
		/// </summary>
		[SerializeField, ShowIf(nameof(hasGraphicalEffects)), DisableInPlayMode]
		private float timeToPlayOnHitEffect;

		/// <summary>
		/// Does this entity belong to the player?
		/// </summary>
		public bool IsPlayer { get; private set; }

		/// <summary>
		/// Is this entity scheduled to die.
		/// </summary>
		public bool Dying { get; set; } = false;

		/// <summary>
		/// The last time this entity was scheduled to die.
		/// </summary>
		public float LastScheduledDie { get; private set; }

		/// <summary>
		/// Get or set the entities game up direction vector.
		/// </summary>
		public Vector3 Forward
		{
			get => transform.up.RemoveAxes(z: true);
			set => transform.up = value;
		}

		/// <summary>
		/// Get or set the entities game right direction vector.
		/// </summary>
		public Vector3 Right
		{
			get => transform.right.RemoveAxes(z: true);
			set => transform.right = value;
		}

		/// <summary>
		/// Get or set the entities world position.
		/// </summary>
		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value;
		}

		/// <summary>
		/// Is this entity initalized.
		/// </summary>
		public bool IsInitalized { get; protected set; } = false;

		/// <summary>
		/// Is this entity instanced.
		/// </summary>
		public bool Instanced { get; private set; }

		/// <summary>
		/// The data associated with this entity.
		/// </summary>
		public virtual EntityData Data { get; protected set; }

		/// <summary>
		/// The time this entity was created.
		/// </summary>
		public float Spawned { get; private set; }

		/// <summary>
		/// How long in seconds since this entity was spawned.
		/// </summary>
		public float TimeAlive { get => Time.time - Spawned; }

		/// <summary>
		/// The owner of this entity.
		/// </summary>
		public Entity Owner { get; set; }

		/// <summary>
		/// The target type of this entity.
		/// </summary>
		public abstract SystemTargets TargetType { get; }

		/// <summary>
		/// The chunk this entity belongs to, if it is attatched to a chunk.
		/// </summary>
		public Chunk Chunk { get; set; }

		/// <summary>
		/// Invoked when this Entity is destroyed.
		/// </summary>
		public event Action<Entity> OnKilled;

		/// <summary>
		/// Get all the actions on this entity.
		/// </summary>
		public List<GameAction> Actions { get => actions; }

		private readonly List<GameAction> actions = new List<GameAction>();

		/// <summary>
		/// The effects currently on this ship.
		/// </summary>
		public EffectManager EntityEffects { get; private set; }

		/// <summary>
		/// The components on this entity.
		/// </summary>
		public Components Components { get; private set; }

		/// <summary>
		/// Invoked when an action is added.
		/// </summary>
		public event Action<GameAction> OnActionAdded;

		/// <summary>
		/// Invoked when an action is removed.
		/// </summary>
		public event Action<GameAction> OnActionRemoved;

		/// <summary>
		/// The subheader of this entity.
		/// </summary>
		public virtual string Subheader { get; } = "Missing Subheader";

		/// <summary>
		/// Does this entity have audio effects.
		/// </summary>
		public bool HasAudio { get => hasAudio; }


		/// <summary>
		/// Determines if this is the player ship
		/// </summary>
		public bool PlayerShip { get; set; }

		/// <summary>
		/// The floating text attatched to this ship, if any.
		/// </summary>
		public FloatingText AttatchedFloatingText { get; set; }

		/// <summary>
		/// Initalize this entity.
		/// </summary>
		/// <param name="data">The data to attatch this entity to.</param>
		/// <param name="owner">The owner to attatch this entity to, optional.</param>
		/// <param name="effects">The effects to add to this entity, optional.</param>
		/// <param name="forceIsPlayer">Force this entity to belong to the player, optional.</param>
		public virtual void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			Data = data;
			Spawned = Time.time;
			Owner = owner;
			IsInitalized = true;
			Instanced = instanced;
			PlayerShip = false;

			if (forceIsPlayer)
				IsPlayer = true;
			else if (owner != null)
				IsPlayer = owner.IsPlayer;

			Components = new Components();
			EntityEffects = new EffectManager(this, TargetType);
			EntityEffects.AddEffectRange(effects);

			OnActionAdded = null;
			OnActionRemoved = null;
			OnKilled = null;

			if (hasDefaultEffects)
			{
				EntityEffects.AddEffectRange(defaultEffects);
			}

			ShowDamageOnEntity = true;
			ShowDamageLocation = this.transform.position;

		}

		public virtual void OnEntityKilled()
		{
			EntityEffects.KillEntity();
			OnKilled?.Invoke(this);

			if (hasGraphicalEffects && onDestroyEffectPrefab != null)
			{
				GameObject effect = Instantiate(onDestroyEffectPrefab, transform.position, transform.rotation, transform.parent);
				effect.transform.localScale = transform.localScale;
				Destroy(effect, timeToPlayOnDestroyEffect); // destroy after X seconds give effect time to play
			}

			if (hasAudio)
			{
				if (!onDeath.IsNull)
					RuntimeManager.PlayOneShot(onDeath);
			}

			if (Chunk != null)
				Chunk.RemoveEntity(this);
		}

		public virtual void OnSpawned()
		{
			if (hasAudio)
			{
				if (!onSpawn.IsNull)
					RuntimeManager.PlayOneShot(onSpawn);
			}
		}

		public virtual void OnDespawned()
		{
			EntityEffects.RemoveAllEffects();
			RemoveAllActions();
			IsInitalized = false;
			Dying = false;
		}

		/// <summary>
		/// Schedule this entity to be killed.
		/// </summary>
		public virtual void KillEntity()
		{
			if (Dying)
				return;

			if (Time.time < LastScheduledDie + SCHEDULE_DIE_INVINCIBLE_TIME)
				return;

			Dying = true;
		}

		/// <summary>
		/// Destroy entity without firing events.
		/// </summary>
		public virtual void UnloadEntity()
		{
			EntityDataManager.UnloadEntity(this);
		}

		protected virtual void Update()
		{
			if (this == null || !IsInitalized)
				return;

			if (!Dying)
				EntityEffects.UpdateEntity();

			if (Dying)
				DoKillEvents();
		}

		private void DoKillEvents()
		{
			Dying = true;
			EntityEffects.OnEntityBeforeDie();

			if (Dying)
			{
				EntityDataManager.KillAndUnloadEntity(this);
			}
		}

		/// <summary>
		/// Stop this entities scheduled death.
		/// </summary>
		public virtual void StopEntityDeath()
		{
			Dying = false;
		}

		/// <summary>
		/// Add a new action onto this entity.
		/// </summary>
		/// <param name="action">The action to add.</param>
		public void AddAction(GameAction action)
		{
			if (!action.Targets.HasFlag(TargetType))
			{
				Debug.LogError($"Tried to add an action (<color=\"orange\">{action.Data.Title}</color>) to an entity whose type (<color=\"orange\">{TargetType}</color>) is not " +
					$"supported by the action (<color=\"orange\">{action.Targets}</color>).");
				return;
			}
			actions.Add(action);
		}

		/// <summary>
		/// Add an action of a certain data type to this entity.
		/// </summary>
		/// <param name="data">The action data type to add.</param>
		public GameAction AddAction(ActionData data)
		{
			var created = Activator.CreateInstance(data.ActionType);
			if (created is GameAction action)
			{
				action.Initialize(data, IsPlayer, this);
				actions.Add(action);
				OnActionAdded?.Invoke(action);
				return action;
			}

			Debug.LogError($"Could not create a script instance from action data <color=\"orange\">{data.Title}</color>", data);
			return null;
		}

		/// <summary>
		/// Remove an action from the entity.
		/// </summary>
		/// <param name="action">The action to remove.</param>
		public void RemoveAction(GameAction action)
		{
			if (actions.Contains(action))
			{
				actions.Remove(action);
				OnActionRemoved?.Invoke(action);
			}
		}

		/// <summary>
		/// Remove all actions on this entity.
		/// </summary>
		public void RemoveAllActions()
		{
			for (int i = actions.Count - 1; i >= 0; i--)
			{
				RemoveAction(actions[i]);
			}
		}

		/// <summary>
		/// Activate all actions on this entity.
		/// </summary>
		public void UseActions()
		{
			foreach (var action in actions)
			{
				UseAction(action);
			}
		}

		private void UseAction(GameAction action)
		{
			action.ExecuteAction(this);
		}

		/// <summary>
		/// Update effects for this entity when we hit something.
		/// </summary>
		/// <param name="other">The other entity hit.</param>
		public virtual void OnEntityHit(Entity other)
		{
			EntityEffects.EntityHit(other);

			if (hasGraphicalEffects && onHitEffectPrefab != null)
			{
				GameObject effect = Instantiate(onHitEffectPrefab, transform.position, transform.rotation, transform.parent);
				effect.transform.localScale = transform.localScale;
				Destroy(effect, timeToPlayOnHitEffect); // destroy after X seconds give effect time to play
			}
		}

		/// Logic for this entity being damaged by another entity
		/// </summary>
		/// <param name="attackingEntity">the entity that is attacking this entity</param>
		/// <param name="damage">the amount of damage being done</param>
		public virtual void TakeDamage(Entity attackingEntity, float damage = 0)
		{
			// by default, entities have no health, so this does nothing. Will be overridden by children.
		}

		protected float collisionDamageMultiplier = 5; // multiplier for all collision damage
		protected float playerCollisionDamageMultiplier = 0.15f; // multiplier for reducing player damage

		// max damage player can take from 1 collision, as a fraction of their health
		protected float playerCollisionDamageCapMultiplier = 0.30f;

		/// <summary>
		/// Damages other entity with collision damage.
		/// Designed to be put into OnEntityHit if you want an entity to apply collision damage.
		/// </summary>
		/// <param name="ownerRigidBody">the rigidBody of 'this'.</param>
		/// <param name="other">the entity being hit</param>
		protected void ApplyCollisionDamage(Rigidbody2D ownerRigidBody, Entity other)
		{
			// calculate collision damage
			if (other is ITractorBeamTarget)
			{
				ITractorBeamTarget target = other as ITractorBeamTarget;

				// other take damage
				float multiplier = collisionDamageMultiplier;
				if (other.PlayerShip)
					multiplier *= playerCollisionDamageMultiplier;

				float velocityDifference = Mathf.Abs((ownerRigidBody.velocity - target.Rigidbody.velocity).magnitude); // should always be positive but just in case
				float averageMass = ((ownerRigidBody.mass + target.Rigidbody.mass) / 2);

				// momentum is velocity * mass
				float force = velocityDifference * averageMass * multiplier;
				if ((int)force == 0)
					return;

				if (other.PlayerShip && force > (target.Health.MaxValue * playerCollisionDamageCapMultiplier))
				{
					//Debug.Log($"{force} exceeds max of {playerCollisionDamageCapMultiplier} * {target.Health.MaxValue}, so will be reduced to {target.Health.MaxValue * playerCollisionDamageCapMultiplier}");
					force = target.Health.MaxValue * playerCollisionDamageCapMultiplier;
				}

				other.TakeDamage(this, force);
			}
		}

		/// <summary>
        /// Used to determine if floating text will be shown on top of the entity.
        /// </summary>
		public bool ShowDamageOnEntity { get; set; }

		public Vector3 ShowDamageLocation { get; set; }

		/// <summary>
		/// Displays floating text at the projectile's location.
		/// </summary>
		public void ShowDamage(float damage)
		{
			CombatHUD.Instance.PrintFloatingText(this, damage);
		}
	}
}
