using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;

namespace Celeritas.Scriptables
{
    [InlineEditor]
	[CreateAssetMenu(fileName = "HullData", menuName = "Celeritas/New Hull")]
	public class HullData : SerializedScriptableObject
	{
        [InfoBox("This field must be an odd number",InfoMessageType.Error, "@!isOdd(this.LayoutResolution)")]
        [OnValueChanged(nameof(onLayoutResolutionChange))]
        [Range(3, 15)]
        public int LayoutResolution = BaseLayoutResolution;
        
        public static int BaseLayoutResolution = 9;

        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/Ship Layout")]
		[TableMatrix(HorizontalTitle = "Left of ship", VerticalTitle = "Back of ship",SquareCells = true, DrawElementMethod = "onHullLayoutDraw")]
		public bool[,] HullLayout = new bool[BaseLayoutResolution,BaseLayoutResolution];

        [BoxGroup("Split/Ship Modules")]
		[TableMatrix(HorizontalTitle = "Left of ship", VerticalTitle = "Back of ship", SquareCells = true)]
		public ModuleData[,] HullModules = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];


        private bool isOdd(int value) {
            return value % 2 != 0 ? true : false;
        }

        private void onLayoutResolutionChange(int value) {
            if (isOdd(value)) {                
                this.HullLayout = new bool[LayoutResolution ,LayoutResolution];
                this.HullModules = new ModuleData[LayoutResolution, LayoutResolution];
            }
        }

        private static bool onHullLayoutDraw(Rect rect, bool value) {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(1f,1f,1f,1f) : new Color(0f,0f,0f,0f));

            return value;
        }
	}

}
