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
	internal class DefaultTextureGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultTextureGraphicDrawer : IGraphicDrawer
		{
			private Texture currentValue;

			public bool CanDraw()
			{
				return currentValue != null;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
				AssetIconsGUI.DrawTexture(drawRect, currentValue, style, selected);
			}

			public void SetValue(object value)
			{
				currentValue = value as Texture;
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
			return new DefaultTextureGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return typeof(Texture).IsAssignableFrom(type);
		}
	}
}

#pragma warning restore
#endif
