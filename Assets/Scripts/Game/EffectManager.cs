using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Wraps an effect collection and allows you to modify the level of the collection.
	/// </summary>
	[System.Serializable]
	public struct EffectWrapper
	{
		[Title("Effect Settings"), PropertyRange(0, 5)]
		public ushort Level;
		public EffectCollection EffectCollection;
	}

	/// <summary>
	/// Manages an list of effect collections and their systems.
	/// </summary>
	public class EffectManager
	{
		private readonly List<EffectWrapper> effects = new List<EffectWrapper>();
		private SystemTargets targetType;
		private Entity owner;

		public EffectManager(Entity owner, SystemTargets target, IList<EffectWrapper> effects = null)
		{
			this.owner = owner;
			targetType = target;

			AddEffectRange(effects);
		}

		/// <summary>
		/// Current effects in this collection.
		/// </summary>
		public IReadOnlyList<EffectWrapper> EffectWrappers { get => effects.AsReadOnly(); }

		/// <summary>
		/// Get a copy of effects in this collection.
		/// </summary>
		public EffectWrapper[] EffectWrapperCopy { get => effects.ToArray(); }

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
			else if (wrapper.EffectCollection.Stacks || !effects.Contains(wrapper))
			{
				effects.Add(wrapper);
				wrapper.EffectCollection.OnAdded(owner, wrapper.Level);
			}
			else
			{
				Debug.LogError($"Tried to add effect colltion which does not stack (<color=\"orange\">{wrapper.EffectCollection.Title}</color>) to an entity who already has this system.");
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

			foreach (var effect in effects)
			{
				AddEffect(effect);
			}
		}

		/// <summary>
		/// Remove all effects.
		/// </summary>
		public void ClearEffects()
		{
			effects.Clear();
		}

		/// <summary>
		/// Remove the listed effect from the collection.
		/// </summary>
		/// <param name="wrapper">The wrapper to remove.</param>
		public void RemoveEffect(EffectWrapper wrapper)
		{
			if (effects.Contains(wrapper))
			{
				effects.Remove(wrapper);
				wrapper.EffectCollection.OnRemoved(owner, wrapper.Level);
			}
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
			foreach (var effect in effects)
			{
				effect.EffectCollection.UpdateEntity(owner, effect.Level);
			}
		}

		/// <summary>
		/// Update the effects when this entity is destroyed.
		/// </summary>
		public void DestroyedEntity()
		{
			foreach (var effect in effects)
			{
				effect.EffectCollection.DestroyEntity(owner, effect.Level);
			}
		}

		/// <summary>
		/// Update the effects when this entity hits another entity.
		/// </summary>
		/// <param name="other">The other entity hit.</param>
		public void EntityHit(Entity other)
		{
			foreach (var effect in effects)
			{
				effect.EffectCollection.HitEntity(owner, other, effect.Level);
			}
		}

		/// <summary>
		/// Update the effects when this entity fires a projectile.
		/// </summary>
		/// <param name="projectile">The fired projectile.</param>
		public void EntityFired(ProjectileEntity projectile)
		{
			foreach (var effect in effects)
			{
				effect.EffectCollection.OnFired(owner as WeaponEntity, projectile, effect.Level);
			}
		}

		/// <summary>
		/// Increase the level of the effects attatched to this entity.
		/// </summary>
		public void IncreaseEffectLevel()
		{
			for (int i = 0; i < effects.Count; i++)
			{
				var wrapper = effects[i];

				var level = wrapper.Level;
				var changed = (ushort)Mathf.Clamp(level + 1, 0, Constants.MAX_EFFECT_LEVEL);

				if (level != changed)
				{
					wrapper.Level = changed;
					effects[i] = wrapper;
					effects[i].EffectCollection.OnLevelChanged(owner, level, changed);
				}
			}
		}

		/// <summary>
		/// Decrease the level of the effects attatched to this entity.
		/// </summary>
		public void DecreaseEffectLevel()
		{
			for (int i = 0; i < effects.Count; i++)
			{
				var wrapper = effects[i];

				var level = wrapper.Level;
				var changed = (ushort)Mathf.Clamp(level - 1, 0, Constants.MAX_EFFECT_LEVEL);

				if (level != changed)
				{
					wrapper.Level = changed;
					effects[i] = wrapper;
					effects[i].EffectCollection.OnLevelChanged(owner, level, changed);
				}
			}
		}
	}
}
