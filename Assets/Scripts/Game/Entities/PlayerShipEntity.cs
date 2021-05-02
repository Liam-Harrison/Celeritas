using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

/// <summary>
/// An extention on ShipEntity that controls/manages player specific data
/// </summary>
public class PlayerShipEntity : ShipEntity
{
	[Title("Inventory")]
	[TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawModulePreview))]
	[SerializeField]
	private ModuleData[,] inventory = new ModuleData[8, 2];

	[SerializeField, Title("Hull")]
	private HullManager hullManager;

	/// <summary>
	/// The inventory of the players ship.
	/// </summary>
	public ModuleData[,] Inventory { get => inventory; set => inventory = value; }

	private static ModuleData DrawModulePreview(Rect rect, ModuleData value)
	{
		if (value != null)
		{
			Texture2D preview = value.icon.ToTexture2D();
			value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
		}
		else
		{
			value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
		}
		return value;
	}
}
