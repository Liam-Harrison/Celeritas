using UnityEngine;

namespace Celeritas.Scriptables
{
	public class EntityData : ScriptableObject
	{
		[SerializeField] protected string title;
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
