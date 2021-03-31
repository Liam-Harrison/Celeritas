using Celeritas.Extensions;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// The Entity class provides basic functionality for on-screen objects.
	/// </summary>
	public abstract class Entity : MonoBehaviour, IEffectManager
	{
		// top of the ShipEntity.cs
		[SerializeField]
		private bool hasDefaultEffects;

		[SerializeField, ShowIf(nameof(hasDefaultEffects))]
		private EffectWrapper[] defaultEffects;

		/// <summary>
		/// Get the entities game up direction vector.
		/// </summary>
		public Vector3 Up { get => transform.up.RemoveAxes(z: true); }

		/// <summary>
		/// Get the entities game right direction vector.
		/// </summary>
		public Vector3 Right { get => transform.right.RemoveAxes(z: true); }

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
		/// Get the 2D position of this entity.
		/// </summary>
		public Vector3 Position { get => Vector3.ProjectOnPlane(transform.position, Vector3.forward); }

		/// <summary>
		/// The owner of this entity.
		/// </summary>
		public Entity Owner { get; set; }

		/// <summary>
		/// The target type of this entity.
		/// </summary>
		public abstract SystemTargets TargetType { get; }

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

			OnEntityCreated(this);

			if (hasDefaultEffects)
			{
				foreach (var effect in defaultEffects)
				{
					Debug.Log(effect);
					Debug.Log(effects);
					effects.Add(effect);
				}
			}

			//base.Initalize(data, owner, effects);
		}

		protected virtual void OnDestroy()
		{
			if (!IsInitalized)
				return;

			OnEntityDestroyed(this);
		}

		protected virtual void Update()
		{
			if (!IsInitalized)
				return;

			OnEntityUpdated(this);
		}

		/// <summary>
		/// Update effects for this entity when created.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityCreated(Entity entity)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.CreateEntity(entity, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when destroyed.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityDestroyed(Entity entity)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.DestroyEntity(entity, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when hit.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="other">The other entity.</param>
		public void OnEntityHit(Entity entity, Entity other)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.HitEntity(entity, other, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when updated.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityUpdated(Entity entity)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.UpdateEntity(entity, wrapper.Level);
			}
		}

		public void AddEffect(EffectWrapper wrapper)
		{
			((IEffectManager)effectManager).AddEffect(wrapper);
		}

		public void AddEffectRange(IList<EffectWrapper> wrappers)
		{
			((IEffectManager)effectManager).AddEffectRange(wrappers);
		}

		public void ClearEffects()
		{
			((IEffectManager)effectManager).ClearEffects();
		}
	}
}
