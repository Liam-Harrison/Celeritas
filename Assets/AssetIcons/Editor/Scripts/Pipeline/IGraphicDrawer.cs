//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEngine;

namespace AssetIcons.Editors.Pipeline
{
	/// <summary>
	/// <para>A drawer for arbitrary objects.</para>
	/// </summary>
	/// <example>
	/// <para>Below is an example of a graphic drawer that draws a colour based on a <c>bool</c> value.</para>
	/// <code>
	/// using AssetIcons.Editors;
	/// using AssetIcons.Editors.Pipeline;
	/// using System;
	/// using UnityEngine;
	/// 
	/// private class BooleanGraphicDrawer : IGraphicDrawer
	/// {
	/// 	private bool hasValue;
	/// 	private bool lastValue;
	/// 
	/// 	public bool CanDraw()
	/// 	{
	/// 		return hasValue;
	/// 	}
	/// 
	/// 	public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
	/// 	{
	/// 		// Transform the rect of the icon in the Project window to the rect of the actual graphic.
	/// 		var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
	/// 
	/// 		// Using AssetIconsGUI allows us to draw stuff using the user-defined style.
	/// 
	/// 		if (lastValue)
	/// 		{
	/// 			// Draw a green square
	/// 			AssetIconsGUI.DrawColor(drawRect, new Color32(0, 255, 0, 255), style, selected);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Draw a red square
	/// 			AssetIconsGUI.DrawColor(drawRect, new Color32(255, 0, 0, 255), style, selected);
	/// 		}
	/// 	}
	/// 
	/// 	public void SetValue(object value)
	/// 	{
	/// 		if (value != null)
	/// 		{
	/// 			lastValue = (bool)value;
	/// 			hasValue = true;
	/// 		}
	/// 		else
	/// 		{
	/// 			hasValue = false;
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	public interface IGraphicDrawer
	{
		/// <summary>
		/// <para>Sets the current value of this graphic drawer.</para>
		/// </summary>
		/// <param name="value">An <see cref="object"/> that the graphic drawer should draw.</param>
		void SetValue(object value);

		/// <summary>
		/// <para>Determines whether this <see cref="IGraphicDrawer"/> is usable to draw a graphic.</para>
		/// </summary>
		/// <returns>
		/// <para><c>true</c> if this <see cref="IGraphicDrawer"/> is valid for drawing; otherwise <c>false</c>.</para>
		/// </returns>
		bool CanDraw();

		/// <summary>
		/// <para>Renders the last value of this <see cref="IGraphicDrawer"/>.</para>
		/// </summary>
		/// <param name="rect">The area in which to draw.</param>
		/// <param name="selected">Whether the rendered graphic should appear selected or not.</param>
		/// <param name="style">A style to use to render the graphic.</param>
		void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style);
	}
}

#pragma warning restore
#endif
