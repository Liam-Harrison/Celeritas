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
	internal class DefaultIntegerGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultIntegerGraphicDrawer : IGraphicDrawer
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
					currentValue = value.ToString();
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
			return new DefaultIntegerGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(sbyte)
				|| type == typeof(byte)
				|| type == typeof(ushort)
				|| type == typeof(short)
				|| type == typeof(int)
				|| type == typeof(uint)
				|| type == typeof(long)
				|| type == typeof(ulong);
		}
	}
}

#pragma warning restore
#endif
