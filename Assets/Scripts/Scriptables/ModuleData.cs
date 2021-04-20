using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using AssetIcons;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a module.
	/// </summary>
	[CreateAssetMenu(fileName = "New Module", menuName = "Celeritas/New Module", order = 20)]
	public class ModuleData : EntityData
	{
		[HorizontalGroup("Module Info", Width = 50)]
		[BoxGroup("Module Info/Icon")]
		[SerializeField, PreviewField, HideLabel]
		[AssetIcon(maxSize: 50)]
		public Sprite icon;

		[HorizontalGroup("Module Info", Width = 50)]
		[BoxGroup("Module Info/Background")]
		[SerializeField, PreviewField, HideLabel]
		[AssetIcon(layer: -1)]
		public Sprite background;

		[HorizontalGroup("Module Info")]
		[BoxGroup("Module Info/Description")]
		[SerializeField, TextArea, HideLabel]
		private string description;


		[SerializeField] protected ModuleSize size;

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ModuleEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ModuleEntity)} attatched to it.", this);
			}
		}
	}
}
