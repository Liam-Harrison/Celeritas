using Celeritas.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Modifier system target entity types.
	/// </summary>
	[System.Flags]
	public enum SystemTargets
	{
		None = 0,
		Projectile = 1,
		Module = 2,
		Ship = 4,
		Asteroid = 5,
		Loot = 6,
		Weapon = 8,
	}

	/// <summary>
	/// Contains the instanced information for a modifier.
	/// </summary>
	public abstract class ModifierSystem : ScriptableObject
	{
		[SerializeField, Title("Generic")]
		protected string title;

		/// <summary>
		/// The title of the modifier.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// Does this modifier stack with other copies of itself.
		/// </summary>
		public abstract bool Stacks { get; }

		/// <summary>
		/// Get the targets for this system.
		/// </summary>
		public abstract SystemTargets Targets { get; }

		/// <summary>
		/// The tooltip for this system.
		/// </summary>
		public abstract string GetTooltip(ushort level);

		/// <summary>
		/// Check if this system includes a specific target.
		/// </summary>
		/// <param name="target">The target to check for.</param>
		/// <returns>Returns true if the target is present, otherwise false.</returns>
		public bool ContainsTarget(SystemTargets target)
		{
			return Targets.HasFlag(target);;
		}
	}
}
