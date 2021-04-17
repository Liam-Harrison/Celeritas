using UnityEngine;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Contains a collection of Unity monobehaviour extensions.
	/// </summary>
	public static class MonoExtensions
	{
		/// <summary>
		/// Check if this has the specified component attatched to it.
		/// </summary>
		/// <typeparam name="T">The component type to check for.</typeparam>
		/// <param name="gameObject">The target GameObject.</param>
		/// <returns>Returns true if component was found, otherwise false.</returns>
		public static bool HasComponent<T>(this GameObject gameObject) where T: Component
		{
			return gameObject.GetComponent<T>() != null;
		}

		/// <summary>
		/// Check if this has the specified component attatched to it.
		/// </summary>
		/// <typeparam name="T">The component type to check for.</typeparam>
		/// <param name="component">The target component.</param>
		/// <returns>Returns true if component was found, otherwise false.</returns>
		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.GetComponent<T>() != null;
		}
	}
}
