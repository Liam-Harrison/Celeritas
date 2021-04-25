//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Product;
using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom property drawer for the <see cref="ColorTint"/>.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(ColorTint))]
	internal class ColorTintDrawer : PropertyDrawer
	{
		private static readonly GUIContent spaceContent = new GUIContent(" ");
		private static readonly GUIContent normalContent = new GUIContent("Unselected");
		private static readonly GUIContent selectedContent = new GUIContent("Selected");

		private AssetIconsCompiledStyle previewGraphicStyle;
		private GUIStyle smallLabelRect;

#if UNITY_2017_3_OR_NEWER
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			return false;
		}
#endif

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 118;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (previewGraphicStyle == null)
			{
				previewGraphicStyle = new AssetIconsCompiledStyle(new AssetIconsStyle()
				{
					MaxSize = 64
				});

				smallLabelRect = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
				{
					alignment = TextAnchor.MiddleCenter
				};
			}

			var sliderRect = new Rect(position.x, position.y + 6, position.width, EditorGUIUtility.singleLineHeight);
			var labelRect = EditorGUI.PrefixLabel(sliderRect, spaceContent);
			var previewBackground = new Rect(labelRect.xMin, sliderRect.yMax + 4,
				labelRect.width, position.yMax - 12 - sliderRect.yMax);

			var previewArea = new Rect(
				previewBackground.x + 8, previewBackground.y + 8,
				previewBackground.width - 16, previewBackground.height - 10);

			var normalRect = new Rect(previewArea.x, previewArea.y, (previewArea.width * 0.5f) - 3.0f, previewArea.height - 18.0f);
			var selectedRect = new Rect(normalRect.xMax + 6.0f, previewArea.y, normalRect.width, previewArea.height - 18.0f);

			var normalLabelRect = new Rect(normalRect.x, normalRect.yMax + 2.0f, normalRect.width, 14.0f);
			var selectedLabelRect = new Rect(selectedRect.x, selectedRect.yMax + 2.0f, selectedRect.width, 14.0f);

			var tintStrengthProperty = property.FindPropertyRelative("tintStrength");

			tintStrengthProperty.floatValue = EditorGUI.Slider(sliderRect, label,
				tintStrengthProperty.floatValue, 0.0f, 1.0f);

			if (Event.current.type == EventType.Repaint)
			{
				EditorGUI.HelpBox(previewBackground, " ", MessageType.None);

				AssetIconsGUI.DrawSprite(normalRect, AssetIconsResources.CurrentTheme.SampleImage, previewGraphicStyle, false);
				AssetIconsGUI.DrawSprite(selectedRect, AssetIconsResources.CurrentTheme.SampleImage, previewGraphicStyle, true);
			}

			EditorGUI.LabelField(normalLabelRect, normalContent, smallLabelRect);
			EditorGUI.LabelField(selectedLabelRect, selectedContent, smallLabelRect);

			EditorGUI.EndProperty();
		}
	}
}

#pragma warning restore
#endif
