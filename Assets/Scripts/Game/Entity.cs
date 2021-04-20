using Celeritas.Extensions;
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
	public abstract class Entity : SerializedMonoBehaviour, IEffectManager
	{
		[SerializeField]
		private bool hasDefaultEffects;

		[SerializeField, ShowIf(nameof(hasDefaultEffects))]
		private EffectWrapper[] defaultEffects;

		/// <summary>
		/// Does this entity belong to the player?
		/// </summary>
		public bool IsPlayer { get; private set; }
		private bool dead;

		/// <summary>
		/// Whether or not this entity is dead (ie, destroyed & should be removed from the screen)
		/// </summary>
		public bool Died { get; set; } = false;

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
			get => Vector3.ProjectOnPlane(transform.position, Vector3.forward);
			set => transform.position = value;
		}

		/// <summary>
		/// Is this entity initalized.
		/// </summary>
		public bool IsInitalized { get; protected set; } = false;

		/// <summary>
		/// The data associated with this entity.
		/// </summary>
		public virtual ScriptableObject Data { get; protected set; }

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
		/// Invoked when this Entity is destroyed.
		/// </summary>
		public event Action<Entity> OnDestroyed;

		private EffectManager effectManager;

		public IReadOnlyList<EffectWrapper> EffectWrappers => ((IEffectManager)effectManager).EffectWrappers;

		public EffectWrapper[] EffectWrapperCopy => ((IEffectManager)effectManager).EffectWrapperCopy;

		/// <summary>
		/// Called to initalize this entity with its appropriate data.
		/// </summary>
		/// <param name="data">The data to associate this entity with.</param>
		public virtual void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false)
		{
			Data = data;
			Spawned = Time.time;
			Owner = owner;
			IsInitalized = true;

			if (forceIsPlayer)
				IsPlayer = true;
			else if (owner != null)
				IsPlayer = owner.IsPlayer;

			effectManager = new EffectManager(TargetType);

			ClearEffects();
			AddEffectRange(effects);
			if (hasDefaultEffects)
			{
				AddEffectRange(defaultEffects);
			}

			OnEntityCreated();	
		}

		protected virtual void OnDestroy()
		{
			if (!IsInitalized)
				return;

			OnEntityDestroyed();
		}

		protected virtual void Update()
		{
			if (this == null || !IsInitalized)
				return;

			if (Died)
			{
				Destroy(gameObject);
			}
			else
			{
				OnEntityUpdated();
			}
		}

		/// <summary>
		/// Update effects for this entity when created.
		/// </summary>
		protected void OnEntityCreated()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.CreateEntity(this, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when destroyed.
		/// </summary>
		protected void OnEntityDestroyed()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.DestroyEntity(this, wrapper.Level);
			}
			OnDestroyed?.Invoke(this);
		}

		/// <summary>
		/// Update effects for this entity when hit.
		/// </summary>
		/// <param name="other">The other entity.</param>
		public virtual void OnEntityHit(Entity other)
		{
			// note: 'this' is hitting the other entity

			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.HitEntity(this, other, wrapper.Level);
			}

			other.TakeDamage(this);
		}

		/// <summary>
		/// Update effects for this entity when updated.
		/// </summary>
		protected void OnEntityUpdated()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.UpdateEntity(this, wrapper.Level);
			}
		}

		/// Logic for this entity being damaged by another entity
		/// </summary>
		/// <param name="attackingEntity">the entity that is attacking this entity</param>
		protected virtual void TakeDamage(Entity attackingEntity)
		{
			// by default, entities have no health, so this does nothing. Will be overridden by children.
		}

		public void AddEffect(EffectWrapper wrapper)
		{
			((IEffectManager)effectManager).AddEffect(wrapper);
			wrapper.EffectCollection.OnAdded(this, wrapper.Level);
		}

		public void AddEffectRange(IList<EffectWrapper> wrappers)
		{
			((IEffectManager)effectManager).AddEffectRange(wrappers);

			if (wrappers != null)
			{
				foreach (var effect in wrappers)
				{
					effect.EffectCollection.OnAdded(this, effect.Level);
				}
			}
		}

		public void ClearEffects()
		{
			foreach (var effect in EffectWrapperCopy)
			{
				effect.EffectCollection.OnRemoved(this, effect.Level);
			}

			((IEffectManager)effectManager).ClearEffects();
		}

		public void RemoveEffect(EffectWrapper wrapper)
		{
			((IEffectManager)effectManager).RemoveEffect(wrapper);
			wrapper.EffectCollection.OnRemoved(this, wrapper.Level);
		}
	}

	/// <summary>
	/// Provides information on how much health an entity has
	/// </summary>
	[System.Serializable]
	public class EntityStatBar
	{
		[SerializeField, PropertyRange(1, 100), Title("Max Health")]
		private uint maxValue;

		[SerializeField, PropertyRange(1, 100), Title("Current Health")]
		private int currentValue;

		/// <summary>
		/// The entity's maximum health
		/// </summary>
		public uint MaxValue { get => maxValue; }

		/// <summary>
		/// The entity's current health
		/// </summary>
		public int CurrentValue { get => currentValue; }

		public EntityStatBar(uint startingValue) {
			maxValue = startingValue;
			currentValue = (int)startingValue;
		}

		/// <summary>
		/// Checks whether the stat is empty or not, returns true if so
		/// </summary>
		/// <returns>true if bar is empty (ie, current value == 0). If health, entity is dead.</returns>
		public bool IsEmpty() {
			if (currentValue < 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// damages entity's stat equal to the passed amount
		/// </summary>
		/// <param name="amount">Amount to damage entity with</param>
		public void Damage(int amount) {
			currentValue -= amount;
		}
	}
}
