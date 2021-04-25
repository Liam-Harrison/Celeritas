//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Pipeline.Default
{
	internal class DefaultGUIStyleGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private static GUIContent text = new GUIContent("Style");

		private class DefaultGUIStyleGraphicDrawer : IGraphicDrawer
		{
			private GUIStyle currentValue;

			public bool CanDraw()
			{
				return currentValue != null;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);

				if (currentValue != null)
				{
					currentValue.Draw(rect, text, 0, selected);
				}
			}

			public void SetValue(object value)
			{
				currentValue = value as GUIStyle;
			}
		}

		public int Priority
		{
			get
			{
				return 100;
			}
		}

		public IGraphicDrawer CreateDrawer()
		{
			return new DefaultGUIStyleGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(GUIStyle);
		}
	}
}

#pragma warning restore
#endif
