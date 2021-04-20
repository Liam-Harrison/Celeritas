//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AssetIcons
{
	/// <summary>
	/// <para>Used to specify that this member controls the <see cref="ScriptableObject" />'s assets' icon.</para>
	/// </summary>
	/// <example>
	/// <para>In this example, we are using AssetIcons to draw a simple <see cref="Sprite" /> on a <see cref="ScriptableObject" />.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon]
	/// 	public Sprite Icon;
	/// }
	/// </code>
	/// <para>We can also use CSS styling to modify the graphics drawn by this attribute. In this example we are going to remove the max size limit on icons and tint it purple.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon(maxSize: 256)]
	/// 	public Sprite Icon;
	/// 	
	/// 	[AssetIcon(maxSize: 256, layer: -1, tint: "#000000ee")]
	/// 	public Color GenerateBackground()
	/// 	{
	/// 		return Color.black;
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <remarks>
	/// <para>This attribute can be placed on:</para>
	/// <ul>
	/// <li>Fields</li>
	/// <li>Properties</li>
	/// <li>Methods (only parameterless ones)</li>
	/// </ul>
	/// <para>The <see cref="AssetIconAttribute"/> can be placed on the same member multiple times to draw the same graphic multiple times.</para>
	/// </remarks>
	/// <seealso cref="AssetIconsStyle"/>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
		AllowMultiple = true)]
	public sealed class AssetIconAttribute : PropertyAttribute
	{
		private readonly AssetIconsStyle style;
		private readonly string filePath;
		private readonly int lineNumber;

		/// <summary>
		/// <para>A style defined in the constructor of the <see cref="AssetIconAttribute"/>.</para>
		/// </summary>
		public AssetIconsStyle Style
		{
			get
			{
				return style;
			}
		}

		/// <summary>
		/// <para>The local file path where this attribute is implemented.</para>
		/// </summary>
		/// <remarks>
		/// <para>This field only has a value in the editor and will return <c>null</c> outside of the editor.</para>
		/// </remarks>
		/// <seealso cref="LineNumber"/>
		public string FilePath
		{
			get
			{
				return filePath;
			}
		}

		/// <summary>
		/// <para>The line number where this attribute implemented.</para>
		/// </summary>
		/// <remarks>
		/// <para>This field only has a value in the editor and will return <c>-1</c> outside of the editor.</para>
		/// </remarks>
		/// <seealso cref="FilePath"/>
		public int LineNumber
		{
			get
			{
				return lineNumber;
			}
		}

		/// <summary>
		/// <para>Marks a field to be used as a graphic for a custom icon.</para>
		/// </summary>
		/// <param name="width">An expression that's evaluated to determine the width of the icon.</param>
		/// <param name="height">An expression that's evaluated to determine the height of the icon.</param>
		/// <param name="x">An expression that's evaluated to determine a horizontal offset of the icon.</param>
		/// <param name="y">An expression that's evaluated to determine a vertical offset of the icon.</param>
		/// <param name="maxSize">A value used to determine the max size of the icon.</param>
		/// <param name="anchor">An anchor that all difference in scale is orientated around.</param>
		/// <param name="aspect">A value used to determine the aspect of the icon.</param>
		/// <param name="display">An expression that's evaluated to whether the icon should be displayed.</param>
		/// <param name="tint">A tint to apply to the icon.</param>
		/// <param name="layer">A value used to determine the layer of the icon.</param>
		/// <param name="fontStyle">A font style to use on all rendered text.</param>
		/// <param name="textAnchor">An anchor for all rendered text.</param>
		/// <param name="projection">A camera projection for all rendered Prefabs.</param>
		/// <param name="lineNumber">The line number that this attribute is implemented. Indended for compiler usage.</param>
		/// <param name="filePath">The local file path that this attribute is implemented. Indended for compiler usage.</param>
		public AssetIconAttribute(
			string width = "100%",
			string height = "100%",
			string x = "0",
			string y = "0",
			int maxSize = 64,
			IconAnchor anchor = IconAnchor.Center,
			IconAspect aspect = IconAspect.Fit,
			string display = "true",
			string tint = IconColor.White,
			int layer = 0,
			FontStyle fontStyle = FontStyle.Normal,
			IconAnchor textAnchor = IconAnchor.Center,
			IconProjection projection = IconProjection.Perspective,
#if UNITY_EDITOR && (!NET_2_0 && !NET_2_0_SUBSET && UNITY_2017_1_OR_NEWER)
			[CallerLineNumber]
#endif
			int lineNumber = -1,
#if UNITY_EDITOR && (!NET_2_0 && !NET_2_0_SUBSET && UNITY_2017_1_OR_NEWER)
			[CallerFilePath]
#endif
			string filePath = null
		)
		{
			style = new AssetIconsStyle()
			{
				Width = width,
				Height = height,
				X = x,
				Y = y,
				MaxSize = maxSize,
				Anchor = anchor,
				Aspect = aspect,
				Tint = tint,
				Layer = layer,
				FontStyle = fontStyle,
				Projection = projection,
				Display = display,
				TextAnchor = textAnchor,
			};

			this.lineNumber = lineNumber;
			this.filePath = filePath;
		}
	}
}

#pragma warning restore
