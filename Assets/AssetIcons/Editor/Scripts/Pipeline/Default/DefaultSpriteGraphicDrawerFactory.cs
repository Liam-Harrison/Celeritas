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
	internal class DefaultSpriteGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultSpriteGraphicDrawer : IGraphicDrawer
		{
			private Sprite currentValue;

			public bool CanDraw()
			{
				return currentValue != null;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
				AssetIconsGUI.DrawSprite(drawRect, currentValue, style, selected);
			}

			public void SetValue(object value)
			{
				currentValue = value as Sprite;
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
			return new DefaultSpriteGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(Sprite);
		}
	}
}

#pragma warning restore
#endif
