using Celeritas.Game;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Provides common functionality for entity data types.
	/// </summary>
	public abstract class EntityData : SerializedScriptableObject
	{
		[SerializeField, Title("Common")] protected string title;
		[SerializeField, PropertyRange(1, 100)] protected int capacityHint = 1;
		[SerializeField, AssetList] protected GameObject prefab;

		private void OnEnable()
		{
			EntityInstance = Prefab.GetComponent<Entity>();
			EntityInstance.Initalize(this, instanced: true);
			EntityType = EntityInstance.GetType();
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
		/// The type of the attatched entity.
		/// </summary>
		public Type EntityType { get; private set; }

		/// <summary>
		/// The reccomended capcaity for this entity.
		/// </summary>
		public int CapacityHint { get => capacityHint; }

		/// <summary>
		/// The tooltip of this entity data.
		/// </summary>
		public abstract string Tooltip { get; }
	}
}
