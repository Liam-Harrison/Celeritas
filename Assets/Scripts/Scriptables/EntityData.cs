using Celeritas.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Provides common functionality for entity data types.
	/// </summary>
	public abstract class EntityData : SerializedScriptableObject
	{
		[SerializeField, Title("Common")] protected string title;
		[SerializeField, AssetList] protected GameObject prefab;

		private void OnEnable()
		{
			EntityInstance = Prefab.GetComponent<Entity>();
			EntityInstance.Initalize(this, instanced: true);
		}

		/// <summary>
		/// The title of the module.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// Get the prefab for the module.
		/// </summary>
		public GameObject Prefab { get => prefab; }

		/// <summary>
		/// The Entity component on the instanced prefab of this object.
		/// </summary>
		public Entity EntityInstance { get; private set; }

		/// <summary>
		/// The tooltip of this entity data.
		/// </summary>
		public abstract string Tooltip { get; }
	}
}
