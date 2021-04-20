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
	internal class DefaultColor32GraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultColor32GraphicDrawer : IGraphicDrawer
		{
			private bool hasValue;
			private Color32 color;

			public bool CanDraw()
			{
				return hasValue;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
				AssetIconsGUI.DrawColor(drawRect, color, style, selected);
			}

			public void SetValue(object value)
			{
				if (value != null)
				{
					color = (Color32)value;
					hasValue = true;
				}
				else
				{
					hasValue = false;
				}
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
			return new DefaultColor32GraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(Color32);
		}
	}
}

#pragma warning restore
#endif
