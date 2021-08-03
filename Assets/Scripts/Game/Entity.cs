using Celeritas.Extensions;
using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
using Celeritas.Game.Interfaces;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// The Entity class provides basic functionality for on-screen objects.
	/// </summary>
	public abstract class Entity : SerializedMonoBehaviour, IPooledObject
	{
		private const float SCHEDULE_DIE_INVINCIBLE_TIME = 0.5f;

		[SerializeField, Title("Entity Settings"), DisableInPlayMode]
		private bool hasDefaultEffects;

		[SerializeField, ShowIf(nameof(hasDefaultEffects)), DisableInPlayMode]
		private EffectWrapper[] defaultEffects;

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
		public IReadOnlyList<GameAction> Actions { get => actions.AsReadOnly(); }

		private readonly List<GameAction> actions = new List<GameAction>();

		/// <summary>
		/// The effects currently on this ship.
		/// </summary>
		public EffectManager EntityEffects { get; private set; }

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

			if (forceIsPlayer)
				IsPlayer = true;
			else if (owner != null)
				IsPlayer = owner.IsPlayer;

			EntityEffects = new EffectManager(this, TargetType);
			EntityEffects.AddEffectRange(effects);

			OnActionAdded = null;
			OnActionRemoved = null;
			OnKilled = null;

			if (hasDefaultEffects)
			{
				EntityEffects.AddEffectRange(defaultEffects);
			}
		}

		public virtual void OnEntityKilled()
		{
			EntityEffects.KillEntity();
			OnKilled?.Invoke(this);

			if (Chunk != null)
				Chunk.RemoveEntity(this);
		}

		public virtual void OnSpawned()
		{

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
			//if (Dying)
			//	return;

			if (Time.time < LastScheduledDie + SCHEDULE_DIE_INVINCIBLE_TIME)
				return;

			Dying = true;
			EntityEffects.OnEntityBeforeDie();

			if (Dying)
			{
				EntityDataManager.KillEntity(this);
			}
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
			else
				KillEntity();
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
		}

		/// Logic for this entity being damaged by another entity
		/// </summary>
		/// <param name="attackingEntity">the entity that is attacking this entity</param>
		/// <param name="damage">the amount of damage being done</param>
		public virtual void TakeDamage(Entity attackingEntity, int damage = 0)
		{
			// by default, entities have no health, so this does nothing. Will be overridden by children.
		}

		protected float collisionDamageMultiplier = 10;

		/// <summary>
		/// Damages other entity with collision damage. Damage self with 50% of damage, too.
		/// Designed to be put into OnEntityHit if you want an entity to apply collision damage.
		/// </summary>
		/// <param name="ownerRigidBody">the rigidBody of 'this'.</param>
		/// <param name="other">the entity being hit</param>
		protected void ApplyCollisionDamage(Rigidbody2D ownerRigidBody, Entity other)
		{
			// calculate collision damage
			if (other is ITractorBeamTarget)
			{
				// momentum is velocity * mass
				float force = ownerRigidBody.velocity.magnitude * ownerRigidBody.mass * collisionDamageMultiplier;

				other.TakeDamage(this, (int)force);

				// take half damage yourself
				TakeDamage(this, (int)force / 2);
			}
		}
	}
}
