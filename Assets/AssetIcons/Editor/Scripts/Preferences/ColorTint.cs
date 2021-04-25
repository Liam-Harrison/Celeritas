//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEngine;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>A data type used to tint graphics.</para>
	/// </summary>
	/// <seealso cref="ColorTintEventField"/>
	[Serializable]
	public struct ColorTint : IEquatable<ColorTint>
	{
		[SerializeField]
		private float tintStrength;

		/// <summary>
		/// <para>A color to tint rendered graphics.</para>
		/// </summary>
		public Color TintColor
		{
			get
			{
				return new Color(GUI.skin.settings.selectionColor.r,
					GUI.skin.settings.selectionColor.g,
					GUI.skin.settings.selectionColor.b);
			}
		}

		/// <summary>
		/// <para>The strength of the tint.</para>
		/// </summary>
		public float TintStrength
		{
			get
			{
				return tintStrength;
			}
		}

		/// <summary>
		/// <para>Constructs a new instance of the <see cref="ColorTint"/>.</para>
		/// </summary>
		public ColorTint(float tintStrength)
		{
			this.tintStrength = tintStrength;
		}

		/// <summary>
		/// <para>Applies the tint to a source color.</para>
		/// </summary>
		/// <param name="source">The original color to be tinted.</param>
		/// <returns>
		/// <para>A new color with the tint applied.</para>
		/// </returns>
		public Color Apply(Color source)
		{
			return Apply(source, 1.0f);
		}

		/// <summary>
		/// <para>Applies the tint to a source color.</para>
		/// </summary>
		/// <param name="source">The original color to be tinted.</param>
		/// <param name="strength">A multiplier for the strength of the tint to apply.</param>
		/// <returns>
		/// <para>A new color with the tint applied.</para>
		/// </returns>
		public Color Apply(Color source, float strength)
		{
			var selectionColor = GUI.skin.settings.selectionColor;
			selectionColor.a = 1.0f;

			return Color.Lerp(
				source,
				selectionColor,
				TintStrength * strength);
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="ColorTint"/> is equal to an <see cref="object"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="object"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="object"/> is a <see cref="ColorTint"/>s and is equal to this; otherwise <c>false</c>.</para>
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is ColorTint && Equals((ColorTint)obj);
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="ColorTint"/> is equal to another <see cref="ColorTint"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="ColorTint"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="ColorTint"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public bool Equals(ColorTint other)
		{
			return tintStrength == other.tintStrength;
		}

		/// <summary>
		/// <para>Returns a unique hash for this of <see cref="ColorTint"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>An <see cref="int"/> that represents a unique has of this instance.</para>
		/// </returns>
		public override int GetHashCode()
		{
			return -308220419 + tintStrength.GetHashCode();
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="ColorTint"/> is equal to another <see cref="ColorTint"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="ColorTint"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="ColorTint"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator ==(ColorTint left, ColorTint right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="ColorTint"/> is not equal to another <see cref="ColorTint"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="ColorTint"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="ColorTint"/>s are not equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator !=(ColorTint left, ColorTint right)
		{
			return !(left == right);
		}
	}
}

#pragma warning restore
#endif
