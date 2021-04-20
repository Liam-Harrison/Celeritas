//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	internal class AssetDrawer
	{
		private readonly IAssetGraphic[] graphics;
		private readonly IGraphicDrawer[] drawers;

		public AssetDrawer(IAssetGraphic[] graphics, IGraphicDrawer[] drawers)
		{
			this.graphics = graphics;
			this.drawers = drawers;
		}

		/// <summary>
		/// <para>Determines whether this <see cref="AssetDrawer"/> is usable to draw a graphic.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if this drawer is valid for drawing; otherwise <c>false</c>.</para>
		/// </returns>
		public bool CanDraw()
		{
			int length = drawers.Length;
			for (int i = 0; i < length; i++)
			{
				var graphic = graphics[i];
				var drawer = drawers[i];

				if (drawer == null)
				{
					continue;
				}

				drawer.SetValue(graphic.Graphic);
				if (drawer.CanDraw())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// <para>Renders all of the elements in this <see cref="AssetDrawer"/>.</para>
		/// </summary>
		/// <param name="rect">The area in which to draw.</param>
		/// <param name="selected">Whether the rendered graphics should appear selected.</param>
		public void Draw(Rect rect, bool selected)
		{
			int length = drawers.Length;
			for (int i = 0; i < length; i++)
			{
				var graphic = graphics[i];
				var drawer = drawers[i];

				if (drawer == null)
				{
					continue;
				}

				drawer.SetValue(graphic.Graphic);
				drawer.Draw(rect, selected, graphic.Style);
			}
		}
	}
}

#pragma warning restore
#endif
