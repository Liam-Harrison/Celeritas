//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>A collection of utility methods for drawing graphics with AssetIcons.</para>
	/// </summary>
	public static class AssetIconsGUIUtility
	{
		/// <summary>
		/// <para>Remaps a <see cref="Rect"/> to conform with Unity's UI control positions.</para>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="Rect"/> repositioned over the original icon.</para>
		/// </returns>
		/// <param name="rect">The <see cref="Rect"/> in which the item is drawn.</param>
		/// <param name="maxSize">The max size of the <see cref="Rect"/>.</param>
		public static Rect AreaToIconRect(Rect rect, float maxSize = 64.0f)
		{
			bool isSmall = IsIconSmall(rect);

			if (isSmall)
			{
				rect.width = rect.height;
			}
			else
			{
				rect.height = rect.width;
#if UNITY_2019_3_OR_NEWER
				rect.width += 1.0f;
#endif
			}

			if (rect.width <= maxSize && rect.height <= maxSize)
			{
#if UNITY_5_5
				if (isSmall)
				{
					rect = new Rect (rect.x + 3, rect.y, rect.width, rect.height);
				}
#elif UNITY_5_6_OR_NEWER
				if (isSmall && !IsTreeView(rect))
				{
					rect = new Rect(rect.x + 3, rect.y, rect.width, rect.height);
				}
#endif
			}
			else
			{
				float offset = (rect.width - maxSize) * 0.5f;
				rect = new Rect(rect.x + offset, rect.y + offset, maxSize, maxSize);
			}

			return rect;
		}

		/// <summary>
		/// <para>Determines if the <see cref="Rect"/> should be drawn using a small icon.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if the icon is small; otherwise, <c>false</c>.</para>
		/// </returns>
		/// <param name="rect">The rect to check if it's small.</param>
		private static bool IsIconSmall(Rect rect)
		{
			return rect.width > rect.height;
		}

		/// <summary>
		/// <para>Determines if the <see cref="Rect"/> is being drawn in Tree View.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if is the specified <see cref="Rect"/> is in <c>TreeView</c>; otherwise, <c>false</c>.</para>
		/// </returns>
		/// <param name="rect">The <see cref="Rect"/> used to check if it's being drawn in treeview.</param>
		private static bool IsTreeView(Rect rect)
		{
			return (rect.x - 16) % 14 == 0;
		}
	}
}

#pragma warning restore
#endif
