using Celeritas.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a modifier.
	/// </summary>
	public abstract class ModifierData : ScriptableObject
	{
		[SerializeField, Title("Generic")]
		protected string title;

		/// <summary>
		/// The title of the modifier.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// Called when the target is created.
		/// </summary>
		/// <param name="target">The target entity.</param>
		public abstract void OnEntityCreated(Entity target);

		/// <summary>
		/// Called when the target is updated.
		/// </summary>
		/// <param name="target">The target entity.</param>
		public abstract void OnEntityUpdated(Entity target);

		/// <summary>
		/// Called when the target is destroyed.
		/// </summary>
		/// <param name="target">The target entity.</param>
		public abstract void OnEntityDestroyed(Entity target);

		/// <summary>
		/// Called when the target is hit or hits another entity.
		/// </summary>
		/// <param name="target">The target entity.</param>
		/// <param name="other">The other entity hit.</param>
		public abstract void OnEntityHit(Entity target, Entity other);
	}
}
