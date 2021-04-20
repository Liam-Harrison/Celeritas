using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using Sirenix.Utilities.Editor;

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
		[TableMatrix(HorizontalTitle = "Left of ship", VerticalTitle = "Back of ship", SquareCells = true, DrawElementMethod = nameof(DrawModulePreview))]
		public ModuleData[,] HullModules = new ModuleData[BaseLayoutResolution, BaseLayoutResolution];

        
		private ModuleData DrawModulePreview(Rect rect, ModuleData value, int x, int y)
		{
            if (HullLayout[x,y] == true) {
                if (value != null)
                {
                    Texture2D preview = textureFromSprite(value.icon);
                    value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, preview, typeof(ModuleData));
                } else {
                    value = (ModuleData)SirenixEditorFields.UnityPreviewObjectField(rect, value, typeof(ModuleData));
                }
            } else {
                value = null;
            }
			return value;
		}

		private static Texture2D textureFromSprite(Sprite sprite)
		{
			if (sprite.rect.width != sprite.texture.width)
			{
				Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
				Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
															 (int)sprite.textureRect.y,
															 (int)sprite.textureRect.width,
															 (int)sprite.textureRect.height);
				newText.SetPixels(newColors);
				newText.filterMode = FilterMode.Point;
				newText.Apply();
				return newText;
			}
			else
				return sprite.texture;
		}

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
