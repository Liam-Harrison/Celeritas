//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

using UnityEngine;
using UnityEngine.Serialization;

namespace AssetIcons.Demo
{
	/// <summary>
	/// <para>A sample <see cref="ScriptableObject"/> that draws a <see cref="Sprite"/> icon and a <see cref="Texture2D"/> background.</para>
	/// </summary>
	/// <seealso cref="VoxelItem"/>
	public sealed class Prop : ScriptableObject
	{
		/// <summary>
		/// <para>An icon to render the for this <see cref="ScriptableObject"/>.</para>
		/// </summary>
		[AssetIcon(aspect: IconAspect.Fit, maxSize: 64)]
		[FormerlySerializedAs("icon")]
		public Sprite Icon;

		/// <summary>
		/// <para>An icon to render the for this <see cref="ScriptableObject"/>'s background.</para>
		/// </summary>
		[AssetIcon(layer: -1)]
		[FormerlySerializedAs("border")]
		public Texture2D Border;
	}
}

#pragma warning restore
