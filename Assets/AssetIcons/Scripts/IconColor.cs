//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#pragma warning disable

namespace AssetIcons
{
	/// <summary>
	/// <para>Tints the drawn graphic a color.</para>
	/// </summary>
	/// <example>
	/// <para>The <see cref="AssetIconAttribute"/> accepts a <see cref="string"/> "color" for styling.</para>
	/// <para><see cref="IconColor"/> contains example <see cref="string"/>s that can be used when tinting in the <see cref="AssetIconAttribute"/>.</para>
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
	/// <seealso cref="AssetIconAttribute"/>
	/// <seealso cref="AssetIconsStyle"/>
	public static class IconColor
	{
		/// <summary>
		/// <para>No tinting.</para>
		/// </summary>
		/// <remarks>
		/// <para>This is the default value for the styling of <see cref="AssetIconAttribute"/>.</para>
		/// </remarks>
		public const string White = "#ffffff";

		/// <summary>
		/// <para>Tints the drawn graphic black.</para>
		/// </summary>
		public const string Black = "#000000";

		/// <summary>
		/// <para>Tints the drawn graphic gray.</para>
		/// </summary>
		public const string Gray = "#888888";

		/// <summary>
		/// <para>Tints the drawn graphic blue.</para>
		/// </summary>
		public const string Blue = "#446fc2";

		/// <summary>
		/// <para>Tints the drawn graphic cyan.</para>
		/// </summary>
		public const string Cyan = "#4c88ad";

		/// <summary>
		/// <para>Tints the drawn graphic green.</para>
		/// </summary>
		public const string Green = "#4cad4f";

		/// <summary>
		/// <para>Tints the drawn graphic yellow.</para>
		/// </summary>
		public const string Yellow = "#e3d96b";

		/// <summary>
		/// <para>Tints the drawn graphic orange.</para>
		/// </summary>
		public const string Orange = "#de9a47";

		/// <summary>
		/// <para>Tints the drawn graphic red.</para>
		/// </summary>
		public const string Red = "#de5147";

		/// <summary>
		/// <para>Tints the drawn graphic purple.</para>
		/// </summary>
		public const string Purple = "#8b47de";

		/// <summary>
		/// <para>Tints the drawn graphic magenta.</para>
		/// </summary>
		public const string Magenta = "#bd48bb";
	}
}

#pragma warning restore
