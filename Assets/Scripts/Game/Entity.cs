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

			OnEntityCreated();

			if (hasDefaultEffects)
			{
				foreach (var effect in defaultEffects)
				{
					Debug.Log(effect);
					Debug.Log(effects);
					effects.Add(effect);
				}
			}
		}

		protected virtual void OnDestroy()
		{
			if (!IsInitalized)
				return;

			OnEntityDestroyed();
		}

		protected virtual void Update()
		{
			if (!IsInitalized)
				return;

			OnEntityUpdated();
		}

		/// <summary>
		/// Update effects for this entity when created.
		/// </summary>
		public void OnEntityCreated()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.CreateEntity(this, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when destroyed.
		/// </summary>
		public void OnEntityDestroyed()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.DestroyEntity(this, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when hit.
		/// </summary>
		/// <param name="other">The other entity.</param>
		public void OnEntityHit(Entity other)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.HitEntity(this, other, wrapper.Level);
			}
		}

		/// <summary>
		/// Update effects for this entity when updated.
		/// </summary>
		public void OnEntityUpdated()
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.UpdateEntity(this, wrapper.Level);
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
}
