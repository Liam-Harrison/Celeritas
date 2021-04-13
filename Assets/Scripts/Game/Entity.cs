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
	public abstract class Entity : MonoBehaviour, IEffectManager
	{
		[SerializeField]
		private bool hasDefaultEffects;

		[SerializeField, ShowIf(nameof(hasDefaultEffects))]
		private EffectWrapper[] defaultEffects;

		[SerializeField]
		protected int damage = 0;

		/// <summary>
		/// The entity's health data
		/// </summary>
		public EntityHealth Health { get; protected set; }

		/// <summary>
		/// How much damage this entity does to another
		/// when it hits
		/// </summary>
		public int Damage { get => damage; set => damage = value; }

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
		public virtual void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null)
		{
			Data = data;
			Spawned = Time.time;
			IsInitalized = true;

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
		protected void OnEntityHit(Entity other)
		{
			// note: 'this' is hitting the other entity

			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.HitEntity(this, other, wrapper.Level);
			}

			DamageEntity(other);
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

		/// <summary>
		/// damages other entity with this entity's 'damage' amount.
		/// virtual so this method may be overridden by child classes (eg, projectile)
		/// </summary>
		/// <param name="other">The entity being damaged</param>
		protected virtual void DamageEntity(Entity other)
		{
			if (other.Health != null)
			{
				other.Health.Damage(damage);
				if (other.Health.IsDead())
					other.Died = true;
			}
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
	public class EntityHealth
	{
		[SerializeField, PropertyRange(1, 100), Title("Max Health")]
		private uint maxHealth;

		[SerializeField, PropertyRange(1, 100), Title("Current Health")]
		private int currentHealth;

		/// <summary>
		/// The entity's maximum health
		/// </summary>
		public uint MaxHealth { get => maxHealth; }

		/// <summary>
		/// The entity's current health
		/// </summary>
		public int CurrentHealth { get => currentHealth; }

		public EntityHealth(uint startingHealth) {
			maxHealth = startingHealth;
			currentHealth = (int)startingHealth;
		}

		/// <summary>
		/// Checks whether the entity is dead, returns true if so
		/// </summary>
		/// <returns>true if entity is dead (ie, current health == 0)</returns>
		public bool IsDead() {
			if (currentHealth < 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// damages entity's health equal to the passed amount
		/// </summary>
		/// <param name="amount">Amount to damage entity with</param>
		public void Damage(int amount) {
			currentHealth -= amount;
		}
	}
}
