using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Provides common functionality for entity data types.
	/// </summary>
	public abstract class EntityData : ScriptableObject
	{
		[SerializeField, Title("Common")] protected string title;
		[SerializeField] protected GameObject prefab;

		/// <summary>
		/// The title of the module.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// Get the prefab for the module.
		/// </summary>
		public GameObject Prefab { get => prefab; }
	}
}
