//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

using UnityEngine;

namespace AssetIcons
{
	/// <summary>
	/// <para>A model that represents the styles defined in the <see cref="AssetIconAttribute"/> constructor.</para>
	/// </summary>
	/// <seealso cref="AssetIconAttribute"/>
	public sealed class AssetIconsStyle
	{
		/// <summary>
		/// <para>An expression that's evaluated to determine the width of the icon.</para>
		/// </summary>
		/// <seealso cref="Height"/>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Display"/>
		public string Width { get; set; }

		/// <summary>
		/// <para>An expression that's evaluated to determine the height of the icon.</para>
		/// </summary>
		/// <seealso cref="Width"/>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Display"/>
		public string Height { get; set; }

		/// <summary>
		/// <para>An expression that's evaluated to determine a horizontal offset of the icon.</para>
		/// </summary>
		/// <seealso cref="Width"/>
		/// <seealso cref="Height"/>
		/// <seealso cref="Y"/>
		public string X { get; set; }

		/// <summary>
		/// <para>An expression that's evaluated to determine a vertical offset of the icon.</para>
		/// </summary>
		/// <seealso cref="Width"/>
		/// <seealso cref="Height"/>
		/// <seealso cref="X"/>
		public string Y { get; set; }

		/// <summary>
		/// <para>A value used to determine the max size of the icon.</para>
		/// </summary>
		/// <remarks>
		/// <para>100% <see cref="Width"/> will have a width less than or equal to <see cref="MaxSize"/>.</para>
		/// </remarks>
		public int MaxSize { get; set; }

		/// <summary>
		/// <para>An anchor that all difference in scale is orientated around.</para>
		/// </summary>
		/// <seealso cref="Width"/>
		/// <seealso cref="Height"/>
		public IconAnchor Anchor { get; set; }

		/// <summary>
		/// <para>A value used to determine the aspect of the icon.</para>
		/// </summary>
		public IconAspect Aspect { get; set; }

		/// <summary>
		/// <para>An expression that's evaluated to whether the icon should be displayed.</para>
		/// </summary>
		/// <seealso cref="Width"/>
		/// <seealso cref="Height"/>
		public string Display { get; set; }

		/// <summary>
		/// <para>A tint to apply to the icon.</para>
		/// </summary>
		/// <seealso cref="IconColor"/>
		public string Tint { get; set; }

		/// <summary>
		/// <para>A value used to determine the layer of the icon.</para>
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// <para>A font style to use on all rendered text.</para>
		/// </summary>
		/// <seealso cref="TextAnchor"/>
		public FontStyle FontStyle { get; set; }

		/// <summary>
		/// <para>An anchor for all rendered text.</para>
		/// </summary>
		/// <seealso cref="FontStyle"/>
		public IconAnchor TextAnchor { get; set; }

		/// <summary>
		/// <para>A camera projection for all rendered Prefabs.</para>
		/// </summary>
		public IconProjection Projection { get; set; }

		/// <summary>
		/// <para>Default constructor for <see cref="AssetIconsStyle"/>.</para>
		/// </summary>
		/// <remarks>
		/// <para>The default values from this constructor are the same as the default values of the <see cref="AssetIconAttribute"/> constructor.</para>
		/// </remarks>
		public AssetIconsStyle()
		{
			Width = "100%";
			Height = "100%";
			X = "0";
			Y = "0";
			MaxSize = int.MaxValue;
			Anchor = IconAnchor.Center;
			Aspect = IconAspect.Fit;
			Display = "true";
			Tint = IconColor.White;
			Layer = 0;
			FontStyle = FontStyle.Normal;
			TextAnchor = IconAnchor.Center;
			Projection = IconProjection.Perspective;
		}
	}
}

#pragma warning restore
