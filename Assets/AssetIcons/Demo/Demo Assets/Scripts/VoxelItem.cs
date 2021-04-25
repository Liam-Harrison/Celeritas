//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

using AssetIcons;
using UnityEngine;

namespace AssetIcons.Demo
{
	/// <summary>
	/// <para>A sample <see cref="ScriptableObject"/> that draws prefab with a background.</para>
	/// </summary>
	/// <seealso cref="Prop"/>
	public sealed class VoxelItem : ScriptableObject
	{
		/// <summary>
		/// <para>A prefab to draw in the center of the icon.</para>
		/// </summary>
		[AssetIcon(width: "85%", height: "85%", maxSize: 128, projection: IconProjection.Orthographic)]
		public GameObject Icon;

		/// <summary>
		/// <para>A whole-icon background to draw on the icon.</para>
		/// </summary>
		[AssetIcon(maxSize: 128, layer: -1)]
		public Sprite Background;

		/// <summary>
		/// <para>A small tag to draw in the bottom-right of their icon.</para>
		/// </summary>
		[AssetIcon(width: "28%", height: "20%", maxSize: 128, x: "-8%", y: "8%", anchor: IconAnchor.BottomRight, display: "Width > 24")]
		public Color ItemColor;
	}
}

#pragma warning restore
