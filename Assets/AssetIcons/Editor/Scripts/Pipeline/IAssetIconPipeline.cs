//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Pipeline
{
	/// <summary>
	/// <para>Apart of the public API for extending AssetIcons.</para>
	/// <para>Used to add custom <see cref="IGraphicDrawerFactory" /> to AssetIcons.</para>
	/// </summary>
	/// <example>
	/// <para>Below is an example of an extension to AssetIcons that draws colored boxes based on a <c>bool</c> value.</para>
	/// <code>
	/// using AssetIcons.Editors;
	/// using AssetIcons.Editors.Pipeline;
	/// using System;
	/// using UnityEngine;
	/// 
	/// public class BooleanAssetIconsExtension : IAssetIconsExtension
	/// {
	/// 	public void Initialise(IAssetIconPipeline pipeline)
	/// 	{
	/// 		pipeline.RegisterDrawer(new BooleanGraphicDrawerFactory());
	/// 	}
	/// }
	/// 
	/// public class BooleanGraphicDrawerFactory : IGraphicDrawerFactory
	/// {
	/// 	private class BooleanGraphicDrawer : IGraphicDrawer
	/// 	{
	/// 		private bool hasValue;
	/// 		private bool lastValue;
	/// 
	/// 		public bool CanDraw()
	/// 		{
	/// 			return hasValue;
	/// 		}
	/// 
	/// 		public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
	/// 		{
	/// 			// Transform the rect of the icon in the Project window to the rect of the actual graphic.
	/// 			var drawRect = AssetIconsGUIUtility.AreaToIconRect(rect, style.MaxSize);
	/// 
	/// 			// Using AssetIconsGUI allows us to draw stuff using the user-defined style.
	/// 
	/// 			if (lastValue)
	/// 			{
	/// 				// Draw a green square
	/// 				AssetIconsGUI.DrawColor(drawRect, new Color32(0, 255, 0, 255), style, selected);
	/// 			}
	/// 			else
	/// 			{
	/// 				// Draw a red square
	/// 				AssetIconsGUI.DrawColor(drawRect, new Color32(255, 0, 0, 255), style, selected);
	/// 			}
	/// 		}
	/// 
	/// 		public void SetValue(object value)
	/// 		{
	/// 			if (value != null)
	/// 			{
	/// 				lastValue = (bool)value;
	/// 				hasValue = true;
	/// 			}
	/// 			else
	/// 			{
	/// 				hasValue = false;
	/// 			}
	/// 		}
	/// 	}
	/// 
	/// 	public int Priority
	/// 	{
	/// 		get
	/// 		{
	/// 			// All of AssetIcons' drawers use 100 as a default value.
	/// 			// The higher the priority, the more likely it is it will be used.
	/// 			return 150;
	/// 		}
	/// 	}
	/// 
	/// 	public IGraphicDrawer CreateDrawer()
	/// 	{
	/// 		return new BooleanGraphicDrawer();
	/// 	}
	/// 
	/// 	public bool IsValidFor(Type type)
	/// 	{
	/// 		return type == typeof(bool);
	/// 	}
	/// }
	/// </code>
	/// </example>
	public interface IAssetIconPipeline
	{
		/// <summary>
		/// <para>Create an <see cref="IGraphicDrawer" /> to draw a graphic of <see cref="Type" />.</para>
		/// </summary>
		/// <param name="type"></param>
		/// <returns>
		/// <para>A <see cref="IGraphicDrawer" /> for drawing a graphic of the specified <see cref="Type" />.</para>
		/// </returns>
		/// <seealso cref="IGraphicDrawerFactory"/>
		IGraphicDrawer CreateGraphicDrawer(Type type);

		/// <summary>
		/// <para>Registers a new <see cref="IGraphicDrawerFactory" /> with the <see cref="IAssetIconPipeline" />.</para>
		/// </summary>
		/// <param name="graphicDrawerFactory">The new <see cref="IGraphicDrawerFactory" /> to register.</param>
		/// <seealso cref="IGraphicDrawerFactory"/>
		/// <seealso cref="IGraphicDrawer"/>
		void RegisterDrawer(IGraphicDrawerFactory graphicDrawerFactory);
	}
}

#pragma warning restore
#endif
