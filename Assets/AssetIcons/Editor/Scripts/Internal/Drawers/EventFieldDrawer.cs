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
	[CustomPropertyDrawer(typeof(EventField), true)]
	internal class EventFieldDrawer : PropertyDrawer
	{
#if UNITY_2017_3_OR_NEWER
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			var internalValueProperty = property.FindPropertyRelative("internalValue");
			return EditorGUI.CanCacheInspectorGUI(internalValueProperty);
		}
#endif

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				var internalValueProperty = property.FindPropertyRelative("internalValue");

				EditorGUI.PropertyField(position, internalValueProperty, label, true);
			}

			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
				// var target = AdvancedGUI.GetPropertyObject<EventField> (property);
				// target.InvokeChanged();
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var internalValueProperty = property.FindPropertyRelative("internalValue");
			return EditorGUI.GetPropertyHeight(internalValueProperty, label, true);
		}
	}
}

#pragma warning restore
#endif
