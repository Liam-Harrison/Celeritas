using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Allows classes to access the properties of the Effect Manager.
	/// </summary>
	public interface IEffectManager
	{
		public IReadOnlyList<EffectWrapper> EffectWrappers { get; }

		public SystemTargets TargetType { get; }

		EffectWrapper[] EffectWrapperCopy { get; }

		void AddEffect(EffectWrapper wrapper);

		void AddEffectRange(IList<EffectWrapper> wrappers);

		void RemoveEffect(EffectWrapper wrapper);

		void ClearEffects();
	}

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
	public class EffectManager : IEffectManager
	{
		private readonly List<EffectWrapper> effects = new List<EffectWrapper>();
		private SystemTargets targetType;

		public EffectManager(SystemTargets target, IList<EffectWrapper> effects = null)
		{
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
			}
		}
	}
}
