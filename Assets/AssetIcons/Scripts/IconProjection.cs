//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

namespace AssetIcons
{
	/// <summary>
	/// <para>An enum that represents camera projections when rendering Prefabs.</para>
	/// </summary>
	/// <example>
	/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconProjection"/> for styling.</para>
	/// <para><see cref="IconColor"/> accepts a <see cref="string"/> "color" for styling.</para>
	/// <para>Below is an example of the <see cref="IconAspect"/> being to force graphics to stretch across the icon area.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon(projection: IconProjection.Orthographic)]
	/// 	public GameObject ItemIcon { get; }
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="AssetIconsStyle"/>
	public enum IconProjection
	{
		/// <summary>
		/// <para>Represents a perspective camera projection.</para>
		/// </summary>
		/// <remarks>
		/// <para>This is the default value for the styling of <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		/// <seealso cref="Orthographic"/>
		Perspective,

		/// <summary>
		/// <para>Represents an orthographic camera projection.</para>
		/// </summary>
		/// <seealso cref="Perspective"/>
		Orthographic
	}
}

#pragma warning restore
