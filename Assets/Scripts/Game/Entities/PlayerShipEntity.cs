using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// An extention on ShipEntity that controls/manages player specific data
	/// </summary>
	public class PlayerShipEntity : ShipEntity
	{
		private static Vector2Int BaseInventorySize = new Vector2Int(8, 4);

		[SerializeField, Range(1, 50), TitleGroup("Camera", "Size settings for various modes.")]
		private float selectionViewSize = 8;

		[SerializeField, Range(1, 50), TitleGroup("Camera", "Size settings for various modes.")]
		private float gameViewSize = 25;

		[SerializeField, Range(1, 50), TitleGroup("Camera", "Size settings for various modes.")]
		private float buildViewSize = 30;

		[Title("Inventory"), SerializeField, OnValueChanged(nameof(onLayoutResolutionChange))]
		private Vector2Int inventorySize = BaseInventorySize;

		[SerializeField, TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawModulePreview))]
		private ModuleData[,] inventory = new ModuleData[BaseInventorySize.x, BaseInventorySize.y];

		[SerializeField, Title("Hull")]
		private HullManager hullManager;

		/// <summary>
		/// The view size for the selection screen.
		/// </summary>
		public float SelectionViewSize { get => selectionViewSize; }

		/// <summary>
		/// The view size for gameplay.
		/// </summary>
		public float GameViewSize { get => gameViewSize; }

		/// <summary>
		/// The ships hull data.
		/// </summary>
		public HullManager HullManager { get => hullManager; }

		/// <summary>
		/// The inventory of the players ship.
		/// </summary>
		public List<ModuleData> Inventory { get; private set; } = new List<ModuleData>();

		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			foreach (var item in inventory)
			{
				if (item == null)
					continue;
				
				Inventory.Add(item);
			}

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);

			PlayerShip = true;
		}

		private ModuleData DrawModulePreview(Rect rect, ModuleData value, int x, int y)
		{
#if UNITY_EDITOR
			if (value != null)
			{
				Texture2D preview = value.Icon.ToTexture2D();
				value = (ModuleData)Sirenix.Utilities.Editor.SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
			}
			else
			{
				value = (ModuleData)Sirenix.Utilities.Editor.SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
			}
#endif
			return value;
		}

		private void onLayoutResolutionChange(Vector2Int value)
		{
			inventory = new ModuleData[inventorySize.x, inventorySize.y];
		}

		public override void OnDespawned()
		{
			base.OnDespawned();

			if (HullManager != null && HullManager.Modules != null)
			{
				foreach (var module in HullManager.Modules)
				{
					if (module != null)
						module.RemoveModule();
				}
			}

			var pc = GetComponent<PlayerController>();
			if (pc != null)
			{
				Destroy(pc);
			}
		}
	}
}
