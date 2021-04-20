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
	internal class DefaultDecimalGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultDecimalGraphicDrawer : IGraphicDrawer
		{
			private string currentValue;

			public bool CanDraw()
			{
				return currentValue != null;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
				AssetIconsGUI.DrawText(drawRect, currentValue, style, selected);
			}

			public void SetValue(object value)
			{
				if (value == null)
				{
					currentValue = null;
				}
				else
				{
					currentValue = ((IFormattable)value).ToString("0.0", null);
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
			return new DefaultDecimalGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(float)
				|| type == typeof(double)
				|| type == typeof(decimal);
		}
	}
}

#pragma warning restore
#endif
