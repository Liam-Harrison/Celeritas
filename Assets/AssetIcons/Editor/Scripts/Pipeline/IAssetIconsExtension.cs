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
	/// <para>The interface that allows users to extend AssetIcons with new features.</para>
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
	/// <seealso cref="IAssetIconPipeline"/>
	/// <seealso cref="IGraphicDrawerFactory"/>
	public interface IAssetIconsExtension
	{
		/// <summary>
		/// <para>Called during AssetIcons' startup sequence.</para>
		/// </summary>
		/// <example>
		/// <para>Here is an example of what an entrypoint to an AssetIcons extension looks like.</para>
		/// <code>
		/// using AssetIcons.Editors.Pipeline;
		/// using UnityEngine;
		/// 
		/// public class BooleanAssetIconsExtension : IAssetIconsExtension
		/// {
		/// 	public void Initialise(IAssetIconPipeline pipeline)
		/// 	{
		/// 		Debug.Log("Entrypoint for AssetIcons extensions.");
		/// 	}
		/// }
		/// </code>
		/// </example>
		/// <param name="pipeline">The <see cref="IAssetIconPipeline" /> that AssetIcons will be using.</param>
		/// <seealso cref="IAssetIconsExtension"/>
		void Initialise(IAssetIconPipeline pipeline);
	}
}

#pragma warning restore
#endif
