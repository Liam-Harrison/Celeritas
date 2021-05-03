using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
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

		[Title("Inventory")]

		[SerializeField, OnValueChanged(nameof(onLayoutResolutionChange))]
		private Vector2Int inventorySize = BaseInventorySize;

		[SerializeField, TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawModulePreview))]
		private ModuleData[,] inventory = new ModuleData[BaseInventorySize.x, BaseInventorySize.y];

		[SerializeField, Title("Hull")]
		private HullManager hullManager;


		/// <summary>
		/// The ships hull data.
		/// </summary>
		public HullManager HullManager { get => hullManager; }

		/// <summary>
		/// The inventory of the players ship.
		/// </summary>
		public List<ModuleData> Inventory { get; private set; } = new List<ModuleData>();

		private ModuleData DrawModulePreview(Rect rect, ModuleData value, int x, int y)
		{
			if (value != null)
			{
				Texture2D preview = value.Icon.ToTexture2D();
				value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
			}
			else
			{
				value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
			}

			return value;
		}

		private void onLayoutResolutionChange(Vector2Int value)
		{
			inventory = new ModuleData[inventorySize.x, inventorySize.y];
		}
	}
}
