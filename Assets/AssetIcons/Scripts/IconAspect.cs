//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

using UnityEngine;

namespace AssetIcons
{
	/// <summary>
	/// <para>An enum that represents how the aspect ratio of a graphic should be used.</para>
	/// </summary>
	/// <example>
	/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAspect"/> for styling.</para>
	/// <para>Below is an example of the <see cref="IconAspect"/> being force graphics to fit into the icon area.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon(aspect: IconAspect.Fit)]
	/// 	public Sprite ItemIcon { get; }
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="AssetIconsStyle"/>
	public enum IconAspect
	{
		/// <summary>
		/// <para>The rendered graphic should fit inside the <see cref="Rect"/> without any stretching.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAspect"/> for styling.</para>
		/// <para>Below is an example of the <see cref="IconAspect"/> being force graphics to fit into the icon area.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(aspect: IconAspect.Fit)]
		/// 	public Sprite ItemIcon { get; }
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// <para>This is the default value for the <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		/// <seealso cref="Envelop"/>
		/// <seealso cref="Stretch"/>
		Fit,

		/// <summary>
		/// <para>The rendered graphic should envelop the <see cref="Rect"/> without any stretching.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAspect"/> for styling.</para>
		/// <para>Below is an example of the <see cref="IconAspect"/> being used to force graphics to envelop the icon area.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(aspect: IconAspect.Envelop)]
		/// 	public Sprite ItemIcon { get; }
		/// }
		/// </code>
		/// </example>
		/// <seealso cref="Fit"/>
		/// <seealso cref="Stretch"/>
		Envelop,

		/// <summary>
		/// <para>The rendered graphic will stretch to the <see cref="Rect"/> dimensions.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAspect"/> for styling.</para>
		/// <para>Below is an example of the <see cref="IconAspect"/> being to force graphics to stretch across the icon area.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(aspect: IconAspect.Stretch)]
		/// 	public Sprite ItemIcon { get; }
		/// }
		/// </code>
		/// </example>
		/// <seealso cref="Fit"/>
		/// <seealso cref="Envelop"/>
		Stretch
	}
}

#pragma warning restore
