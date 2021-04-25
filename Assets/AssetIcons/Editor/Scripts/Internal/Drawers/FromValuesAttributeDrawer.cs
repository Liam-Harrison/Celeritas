//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom property drawer for the <see cref="FromValuesAttribute"/>.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(FromValuesAttribute))]
	internal class FromValuesAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!DrawProperty(position, property, label))
			{
				var valueProperty = property.FindPropertyRelative("internalValue");
				if (valueProperty != null)
				{
					DrawProperty(position, valueProperty, label);
				}
				else
				{
					EditorGUI.HelpBox(position, "Unsupported Type", MessageType.Error);
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		private bool DrawProperty(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				var fromValues = (FromValuesAttribute)attribute;

				int index = -1;
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						for (int i = 0; i < fromValues.Values.Length; i++)
						{
							int value = (int)fromValues.Values[i];

							if (property.intValue == value)
							{
								index = i;
								break;
							}
						}

						int newIntValue = EditorGUI.Popup(position, label, index, fromValues.Names);
						if (newIntValue != index)
						{
							property.intValue = (int)fromValues.Values[newIntValue];
						}
						return true;


					case SerializedPropertyType.String:
						for (int i = 0; i < fromValues.Values.Length; i++)
						{
							string value = (string)fromValues.Values[i];

							if (property.stringValue == value)
							{
								index = i;
								break;
							}
						}

						int newStringValue = EditorGUI.Popup(position, label, index, fromValues.Names);
						if (newStringValue != index)
						{
							property.stringValue = (string)fromValues.Values[newStringValue];
						}
						return true;

					case SerializedPropertyType.Enum:
						for (int i = 0; i < fromValues.Values.Length; i++)
						{
							int value = (int)fromValues.Values[i];

							if (property.enumValueIndex == value)
							{
								index = i;
								break;
							}
						}

						int newEnumValue = EditorGUI.Popup(position, label, index, fromValues.Names);
						if (newEnumValue != index)
						{
							property.enumValueIndex = (int)fromValues.Values[newEnumValue];
						}
						return true;

					case SerializedPropertyType.Float:
						for (int i = 0; i < fromValues.Values.Length; i++)
						{
							float value = (float)fromValues.Values[i];

							if (Mathf.Abs(property.floatValue - value) < 0.01f)
							{
								index = i;
								break;
							}
						}

						int newFloatValue = EditorGUI.Popup(position, label, index, fromValues.Names);
						if (newFloatValue != index)
						{
							property.floatValue = (float)fromValues.Values[newFloatValue];
						}
						return true;

					default:
						return false;
				}
			}
		}
	}
}

#pragma warning restore
#endif
