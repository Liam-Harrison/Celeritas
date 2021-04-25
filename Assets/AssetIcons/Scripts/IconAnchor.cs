//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

namespace AssetIcons
{
	/// <summary>
	/// <para>When changing the height, the anchor will change how much each side of the <see cref="Rect"/> will change.</para>
	/// <para>Using anchors is the fastest and easiest way to position elements in an icon.</para>
	/// </summary>
	/// <example>
	/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
	/// <para>Below is an example of the <see cref="IconAnchor"/> being used to create a bar at the bottom of the assets icon.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon(anchor: IconAnchor.Bottom, height: "10%")]
	/// 	public Color BottomBar { get; }
	/// }
	/// </code>
	/// <para>Anchoring the asset to the bottom of the icon and then adjusting the height results in a bar on the bottom of the icon.</para>
	/// <para>Anchoring the asset to corners of the icon can create handy annotations for assets.</para>
	/// <code>
	/// using AssetIcons;
	/// using UnityEngine;
	/// 
	/// [CreateAssetMenu(menuName = "Item")]
	/// public class Item : ScriptableObject
	/// {
	/// 	[AssetIcon(anchor: IconAnchor.TopLeft, height: "10%", width: "15%")]
	/// 	public Color TopLeftTag { get; }
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="AssetIconsStyle"/>
	public enum IconAnchor
	{
		/// <summary>
		/// <para>Anchored to the center of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "center" anchoring being used to shink the asset.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.Center, width: "85%", height: "85%")]
		/// 	public Color Icon { get; }
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// <para>This is the default value for the styling of <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		Center,

		/// <summary>
		/// <para>Anchored to the top of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "top" anchoring being used to create a bar at the top of the asset.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.Top, height: "10%")]
		/// 	public Color TopBar { get; }
		/// }
		/// </code>
		/// </example>
		Top,

		/// <summary>
		/// <para>Anchored to the bottom of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "bottom" anchoring being used to create a bar at the bottom of the asset.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.Bottom, height: "10%")]
		/// 	public Color BottomBar { get; }
		/// }
		/// </code>
		/// </example>
		Bottom,

		/// <summary>
		/// <para>Anchored to the left of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "left" anchoring being used to create a bar on the left of the asset.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.Left, width: "15%")]
		/// 	public Color LeftBar { get; }
		/// }
		/// </code>
		/// </example>
		Left,

		/// <summary>
		/// <para>Anchored to the right of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "right" anchoring being used to create a bar on the right of the asset.</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.Right, width: "15%")]
		/// 	public Color RightBar { get; }
		/// }
		/// </code>
		/// </example>
		Right,

		/// <summary>
		/// <para>Anchored to the top-left of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "top-left" anchoring being used to create an icon in the top-left of the graphic..</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.TopLeft, width: "20%", height: "10%")]
		/// 	public Color SmallTag { get; }
		/// }
		/// </code>
		/// </example>
		TopLeft,

		/// <summary>
		/// <para>Anchored to the top-right of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "top-right" anchoring being used to create an icon in the top-right of the graphic..</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.TopRight, width: "20%", height: "10%")]
		/// 	public Color SmallTag { get; }
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// <para>It's not recommended to anchor small graphics in the top-right due to Unity Collab icon being drawn there.</para>
		/// </remarks>
		TopRight,

		/// <summary>
		/// <para>Anchored to the bottom-left of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "bottom-left" anchoring being used to create an icon in the bottom-left of the graphic..</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.BottomLeft, width: "20%", height: "10%")]
		/// 	public Color SmallTag { get; }
		/// }
		/// </code>
		/// </example>
		BottomLeft,

		/// <summary>
		/// <para>Anchored to the bottom-right of the graphic.</para>
		/// </summary>
		/// <example>
		/// <para>The <see cref="AssetIconAttribute"/> accepts an <see cref="IconAnchor"/> for styling.</para>
		/// <para>Below is an example of the "bottom-right" anchoring being used to create an icon in the bottom-right of the graphic..</para>
		/// <code>
		/// using AssetIcons;
		/// using UnityEngine;
		/// 
		/// [CreateAssetMenu(menuName = "Item")]
		/// public class Item : ScriptableObject
		/// {
		/// 	[AssetIcon(anchor: IconAnchor.BottomRight, width: "20%", height: "10%")]
		/// 	public Color SmallTag { get; }
		/// }
		/// </code>
		/// </example>
		BottomRight,
	}
}

#pragma warning restore
