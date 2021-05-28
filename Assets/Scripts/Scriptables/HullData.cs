using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Celeritas.Extensions;
using UnityEngine;
using Celeritas.Game.Entities;

namespace Celeritas.Scriptables
{
	[InlineEditor]
	[CreateAssetMenu(fileName = "HullData", menuName = "Celeritas/New Hull")]
	public class HullData : SerializedScriptableObject
	{
		[SerializeField]
		[InfoBox("This field must be an odd number", InfoMessageType.Error, "@!isOdd(this.LayoutResolution)")]
		[OnValueChanged(nameof(onLayoutResolutionChange))]
		[Range(3, 15)]
		private int LayoutResolution = BaseLayoutResolution;

		private static int BaseLayoutResolution = 9;

		private const float GroupSize = 1f / 3f;

		[HorizontalGroup("Split", GroupSize)]
		[BoxGroup("Split/Ship Layout")]
		[SerializeField]
		[TableMatrix(HorizontalTitle = "Right of ship", VerticalTitle = "Back of ship", SquareCells = true, DrawElementMethod = nameof(onHullLayoutDraw))]
		private bool[,] hullLayout = new bool[BaseLayoutResolution, BaseLayoutResolution];

		[HorizontalGroup("Split", GroupSize)]
		[BoxGroup("Split/Ship Module Origins")]
		[SerializeField]
		[TableMatrix(HorizontalTitle = "Right of ship", VerticalTitle = "Back of ship", SquareCells = true, DrawElementMethod = nameof(DrawModuleOriginPreview))]
		private ModuleData[,] hullModuleOrigins = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];

		// Properties

		/// <summary>
		/// Ship's layout data
		/// </summary>
		public bool[,] HullLayout { get => hullLayout; }

		/// <summary>
		/// Ship's original module position data.
		/// </summary>
		/// <value></value>
		public ModuleData[,] HullModuleOrigins { get => hullModuleOrigins; set => hullModuleOrigins = value; }

		/// <summary>
		/// Removes any data in the 2D arrays
		/// </summary>
		[Button]
		public void ResetData()
		{
			hullLayout = new bool[BaseLayoutResolution, BaseLayoutResolution];
			hullModuleOrigins = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];
		}

		/// <summary>
		/// Removes any module data in the 2D arrays
		/// </summary>
		[Button]
		public void ResetModuleData()
		{
			hullModuleOrigins = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];
		}

		private bool isOdd(int value)
		{
			return value % 2 != 0 ? true : false;
		}

		private ModuleData DrawModuleOriginPreview(Rect rect, ModuleData value, int x, int y)
		{
#if UNITY_EDITOR
			if (HullLayout[x, y] == true)
			{
				if (value != null)
				{
					Texture2D preview = value.Icon.ToTexture2D();
					value = (ModuleData)Sirenix.Utilities.Editor.SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
				}
				else
				{
					value = (ModuleData)Sirenix.Utilities.Editor.SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
				}
			}
			else
			{
				value = null;
			}
#endif
			return value;
		}

		private void onLayoutResolutionChange(int value)
		{
			if (isOdd(value))
			{
				hullLayout = new bool[LayoutResolution, LayoutResolution];
			}
		}

		private static bool onHullLayoutDraw(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

#if UNITY_EDITOR
			UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 0f, 0f, 0f));
#endif
			return value;
		}
	}

}
