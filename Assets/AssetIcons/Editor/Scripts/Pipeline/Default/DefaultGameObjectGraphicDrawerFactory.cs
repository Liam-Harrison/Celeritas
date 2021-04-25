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
	internal class DefaultGameObjectGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class DefaultGameObjectGraphicDrawer : IGraphicDrawer
		{
			private GameObject currentValue;

			public bool CanDraw()
			{
				return currentValue != null;
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);

				if (currentValue != null)
				{
					var col = AssetIconsGUI.BackgroundColor;
					col = new Color(col.r, col.g, col.b, 0);

					var setup = new AssetIconsCameraSetup()
					{
						BackgroundColor = col,
						TransparentBackground = true,
						Orthographic = style.Projection == IconProjection.Orthographic,

						PreviewDirection = new Vector3(-1.0f, -1.0f, -1.0f)
					};

					var thumbnail = AssetIconsRenderCache.GetTexture(setup, currentValue);

					AssetIconsGUI.DrawTexture(rect, thumbnail, style, selected);
				}
			}

			public void SetValue(object value)
			{
				currentValue = value as GameObject;
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
			return new DefaultGameObjectGraphicDrawer();
		}

		public bool IsValidFor(Type type)
		{
			return type == typeof(GameObject);
		}
	}
}

#pragma warning restore
#endif
