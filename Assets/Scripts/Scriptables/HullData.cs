using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Celeritas.Extensions;
using UnityEngine;

namespace Celeritas.Scriptables
{
	[InlineEditor]
	[CreateAssetMenu(fileName = "HullData", menuName = "Celeritas/New Hull")]
	public class HullData : SerializedScriptableObject
	{
        [SerializeField]
        [InfoBox("This field must be an odd number",InfoMessageType.Error, "@!isOdd(this.LayoutResolution)")]
        [OnValueChanged(nameof(onLayoutResolutionChange))]
        [Range(3, 15)]
        private int LayoutResolution = BaseLayoutResolution;
        
        private static int BaseLayoutResolution = 9;

        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/Ship Layout")]
        [SerializeField]
		[TableMatrix(HorizontalTitle = "Right of ship", VerticalTitle = "Back of ship",SquareCells = true, DrawElementMethod = "onHullLayoutDraw")]
		private bool[,] hullLayout = new bool[BaseLayoutResolution,BaseLayoutResolution];

        [BoxGroup("Split/Ship Modules")]
        [SerializeField]
		[TableMatrix(HorizontalTitle = "Right of ship", VerticalTitle = "Back of ship", SquareCells = true, DrawElementMethod = nameof(DrawModulePreview))]
		private ModuleData[,] hullModules = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];

        // Properties

        /// <summary>
        /// Ship's layout data
        /// </summary>
        public bool[,] HullLayout { get => hullLayout; }
        
        /// <summary>
        /// Ship's module position data.
        /// </summary>
        /// <value></value>
        public ModuleData[,] HullModules { get => hullModules; }
        
		private ModuleData DrawModulePreview(Rect rect, ModuleData value, int x, int y)
		{
            if (HullLayout[x,y] == true) {
                if (value != null)
                {
                    Texture2D preview = value.icon.ToTexture2D();
                    value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
                } else {
                    value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
                }
            } else {
                value = null;
            }
			return value;
		}

        private bool isOdd(int value) {
            return value % 2 != 0 ? true : false;
        }

        private void onLayoutResolutionChange(int value) {
            if (isOdd(value)) {                
                hullLayout = new bool[LayoutResolution ,LayoutResolution];
                hullModules = new ModuleData[LayoutResolution, LayoutResolution];
            }
        }

        private static bool onHullLayoutDraw(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

#if UNITY_EDITOR
			UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(1f,1f,1f,1f) : new Color(0f,0f,0f,0f));
#endif
			return value;
        }
	}

}
