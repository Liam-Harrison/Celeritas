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
		[SerializeField, TitleGroup("Entity Settings")] protected string title;

		[SerializeField, AssetList, TitleGroup("Entity Settings")] protected GameObject prefab;

		[SerializeField, PropertyRange(1, 100), TitleGroup("Entity Settings")]
		protected int capacityHint = 1;

		[SerializeField, TitleGroup("Chunking")]
		protected bool useChunking = false;

		private void OnEnable()
		{
			if (prefab != null)
			{
				EntityInstance = Prefab.GetComponent<Entity>();
				EntityInstance.Initalize(this, instanced: true);
				EntityType = EntityInstance.GetType();
			}
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
		/// Use the chunking system for this entity.
		/// </summary>
		public bool UseChunking { get => useChunking; }

		/// <summary>
		/// The tooltip of this entity data.
		/// </summary>
		public abstract string Tooltip { get; }
	}
}
