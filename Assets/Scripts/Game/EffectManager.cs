using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Wraps an effect collection and allows you to modify the level of the collection.
	/// </summary>
	[System.Serializable]
	public class EffectWrapper
	{
		[Title("Effect Settings"), PropertyRange(0, 5)]
		public int Level;
		public EffectCollection EffectCollection;
	}

	/// <summary>
	/// Manages an list of effect collections and their systems.
	/// </summary>
	public class EffectManager: IEnumerable<EffectWrapper>
	{
		private readonly List<EffectWrapper> effects = new List<EffectWrapper>();
		private SystemTargets targetType;
		private Entity owner;
		private readonly HashSet<EffectWrapper> requests = new HashSet<EffectWrapper>();
		private bool processing = false;

		public EffectManager(Entity owner, SystemTargets target, IList<EffectWrapper> effects = null)
		{
			this.owner = owner;
			targetType = target;

			AddEffectRange(effects);
		}

		/// <summary>
		/// Current effects in this collection.
		/// </summary>
		public IReadOnlyList<EffectWrapper> EffectWrappers { get => effects; }

		/// <summary>
		/// Get a copy of effects in this collection.
		/// </summary>
		public EffectWrapper[] EffectWrapperCopy { get => effects.ToArray(); }

		/// <summary>
		/// The amount of effects in this manager.
		/// </summary>
		public int Length { get => effects.Count; }

		/// <summary>
		/// Does this effect manager have no effects.
		/// </summary>
		public bool IsEmpty { get => Length == 0; }

		/// <summary>
		/// The target type of these effects.
		/// </summary>
		public SystemTargets TargetType { get => targetType; }

		/// <summary>
		/// Add a new effect collection.
		/// </summary>
		/// <param name="wrapper">The effect collection to add.</param>
		public void AddEffect(EffectWrapper wrapper)
		{
			if (!wrapper.EffectCollection.Targets.HasFlag(TargetType))
			{
				Debug.LogError($"Tried to add an effect (<color=\"orange\">{wrapper.EffectCollection.Title}</color>) to an entity whose type (<color=\"orange\">{TargetType}</color>) is not " +
					$"supported by the effect collection (<color=\"orange\">{wrapper.EffectCollection.Targets}</color>), If the system supports this type add it as a target to the collection. Otherwise remove the effect collection.");
			}
			else
			{
				if (effects.Contains(wrapper) == false)
				{
					effects.Add(wrapper);

					if (!owner.Instanced)
					{
						processing = true;

						wrapper.EffectCollection.OnAdded(owner, wrapper);

						processing = false;
						HandleRemoveRequests();
					}
				}
				else
				{
					processing = true;

					wrapper.EffectCollection.OnReset(owner, wrapper);

					processing = false;
					HandleRemoveRequests();
				}
			}
		}

		private void AddEffectWithoutNotify(EffectWrapper wrapper, ref HashSet<EffectWrapper> added, ref HashSet<EffectWrapper> reset)
		{
			if (!wrapper.EffectCollection.Targets.HasFlag(TargetType))
			{
				Debug.LogError($"Tried to add an effect (<color=\"orange\">{wrapper.EffectCollection.Title}</color>) to an entity whose type (<color=\"orange\">{TargetType}</color>) is not " +
					$"supported by the effect collection (<color=\"orange\">{wrapper.EffectCollection.Targets}</color>), If the system supports this type add it as a target to the collection. Otherwise remove the effect collection.");
			}
			else
			{
				if (effects.Contains(wrapper) == false)
				{
					effects.Add(wrapper);
					added.Add(wrapper);
				}
				else
				{
					reset.Add(wrapper);
				}
			}
		}

		/// <summary>
		/// Add a collection of effects.
		/// </summary>
		/// <param name="effects">The collection of effects to add.</param>
		public void AddEffectRange(IList<EffectWrapper> effects)
		{
			if (effects == null)
				return;

			var added = new HashSet<EffectWrapper>();
			var reset = new HashSet<EffectWrapper>();

			foreach (var effect in effects)
			{
				AddEffectWithoutNotify(effect, ref added, ref reset);
			}

			if (owner.Instanced == false)
			{
				processing = true;

				foreach (var effect in added)
				{
					effect.EffectCollection.OnAdded(owner, effect);
				}

				foreach (var effect in reset)
				{
					effect.EffectCollection.OnReset(owner, effect);
				}

				processing = false;
				HandleRemoveRequests();
			}
		}

		/// <summary>
		/// Remove all effects.
		/// </summary>
		public void RemoveAllEffects()
		{
			for (int i = effects.Count - 1; i >= 0; i--)
			{
				RemoveEffect(effects[i]);
			}
		}

		/// <summary>
		/// Remove the listed effect from the collection.
		/// </summary>
		/// <param name="wrapper">The wrapper to remove.</param>
		public void RemoveEffect(EffectWrapper wrapper)
		{
			if (effects.Contains(wrapper))
			{
				if (processing)
				{
					requests.Add(wrapper);
				}
				else
				{
					InternalRemoveEffect(wrapper);
				}
			}
		}

		/// <summary>
		/// Request for this effect to be removed. To be used inside effect functions.
		/// </summary>
		/// <param name="wrapper"></param>
		public void RequestRemovedEffect(EffectWrapper wrapper)
		{
			requests.Add(wrapper);
		}

		/// <summary>
		/// Remove a range of effects from the entity.
		/// </summary>
		/// <param name="wrappers">The effects to remove.</param>
		public void RemoveEffectRange(IList<EffectWrapper> wrappers)
		{
			if (effects == null)
				return;

			foreach (var effect in wrappers)
			{
				RemoveEffect(effect);
			}
		}

		/// <summary>
		/// Update the effects when this entity is updated.
		/// </summary>
		public void UpdateEntity()
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.UpdateEntity(owner, effect);
			}

			HandleRemoveRequests();
			processing = false;
		}

		/// <summary>
		/// Update the effects when this entity is destroyed.
		/// </summary>
		public void KillEntity()
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.KillEntity(owner, effect);
			}

			processing = false;
			HandleRemoveRequests();
		}

		/// <summary>
		/// Update the effects when this entity is scheduled to be destroyed.
		/// </summary>
		public void OnEntityBeforeDie()
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.OnEntityBeforeDie(owner, effect);
			}

			processing = false;
			HandleRemoveRequests();
		}

		/// <summary>
		/// Update the effects when this entity hits another entity.
		/// </summary>
		/// <param name="other">The other entity hit.</param>
		public void EntityHit(Entity other)
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.HitEntity(owner, other, effect);
			}

			processing = false;
			HandleRemoveRequests();
		}

		/// <summary>
		/// Update the effects when this entity fires a projectile.
		/// </summary>
		/// <param name="projectile">The fired projectile.</param>
		public void EntityFired(ProjectileEntity projectile)
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.OnFired(owner as WeaponEntity, projectile, effect);
			}

			processing = false;
			HandleRemoveRequests();
		}

		/// <summary>
		/// Update the effects when this entity changes level.
		/// </summary>
		/// <param name="previous">The previous level of this entity.</param>
		public void EntityLevelChanged(int previous)
		{
			processing = true;
			foreach (var effect in effects)
			{
				effect.EffectCollection.OnLevelChanged(owner, previous, effect.Level, effect);
			}

			processing = false;
			HandleRemoveRequests();
		}

		public IEnumerator<EffectWrapper> GetEnumerator()
		{
			return ((IEnumerable<EffectWrapper>)EffectWrapperCopy).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return EffectWrapperCopy.GetEnumerator();
		}

		private void InternalRemoveEffect(EffectWrapper wrapper)
		{
			processing = true;

			effects.Remove(wrapper);
			wrapper.EffectCollection.OnRemoved(owner, wrapper);

			processing = false;
			HandleRemoveRequests();
		}

		private void HandleRemoveRequests()
		{
			var todo = new HashSet<EffectWrapper>(requests);
			requests.Clear();

			if (todo.Count == 0)
				return;

			foreach (var request in todo)
			{
				InternalRemoveEffect(request);
			}
		}
	}
}
