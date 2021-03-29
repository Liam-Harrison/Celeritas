using Celeritas.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public interface IEffectManager
	{
		public IReadOnlyList<EffectCollection> Effects { get; }

		public SystemTargets TargetType { get; }

		EffectCollection[] EffectsCopy { get; }

		void AddEffect(EffectCollection effect);

		void AddEffectRange(IList<EffectCollection> effects);

		void ClearEffects();
	}

	public class EffectManager : IEffectManager
	{
		private readonly List<EffectCollection> effects = new List<EffectCollection>();
		private SystemTargets targetType;

		public EffectManager(SystemTargets target, IList<EffectCollection> effects = null)
		{
			targetType = target;
			AddEffectRange(effects);
		}

		/// <summary>
		/// Current effects in this collection.
		/// </summary>
		public IReadOnlyList<EffectCollection> Effects { get => effects.AsReadOnly(); }

		/// <summary>
		/// Get a copy of effects in this collection.
		/// </summary>
		public EffectCollection[] EffectsCopy { get => effects.ToArray(); }

		/// <summary>
		/// The target type of these effects.
		/// </summary>
		public SystemTargets TargetType { get => targetType; }

		/// <summary>
		/// Add a new effect collection.
		/// </summary>
		/// <param name="effect">The effect collection to add.</param>
		public void AddEffect(EffectCollection effect)
		{
			if (!effect.Targets.HasFlag(TargetType))
			{
				Debug.LogError($"Tried to add an effect (<color=\"orange\">{effect.Title}</color>) to an entity whose type (<color=\"orange\">{TargetType}</color>) is not " +
					$"supported by the effect collection (<color=\"orange\">{effect.Targets}</color>), If the system supports this type add it as a target to the collection. Otherwise remove the effect collection.");
			}
			else if (effect.Stacks || !effects.Contains(effect))
			{
				effects.Add(effect);
			}
			else
			{
				Debug.LogError($"Tried to add effect colltion which does not stack (<color=\"orange\">{effect.Title}</color>) to an entity who already has this system.");
			}
		}

		/// <summary>
		/// Add a collection of effects.
		/// </summary>
		/// <param name="effects">The collection of effects to add.</param>
		public void AddEffectRange(IList<EffectCollection> effects)
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
	}
}
