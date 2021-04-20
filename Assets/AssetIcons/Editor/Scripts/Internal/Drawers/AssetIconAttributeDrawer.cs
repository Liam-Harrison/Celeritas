//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawers
{
	/// <summary>
	/// <para>A custom decorator drawer for the <see cref="AssetIconAttribute"/>.</para>
	/// <para>The project window repaints whenever the value of the property is changed by this property drawer.</para>
	/// </summary>
	[CustomPropertyDrawer(typeof(AssetIconAttribute), true)]
	internal class AssetIconAttributeDrawer : DecoratorDrawer
	{
		public override float GetHeight()
		{
			return 0.0f;
		}

		public override void OnGUI(Rect position)
		{
			if (Event.current.type == EventType.ExecuteCommand)
			{
				EditorApplication.RepaintProjectWindow();
			}
		}
	}
}

#pragma warning restore
#endif
