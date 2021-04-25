//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>A camera setup used in <see cref="AssetIconsRenderer"/> to render a Prefab.</para>
	/// </summary>
	/// <seealso cref="AssetIconsRenderer"/>
	/// <seealso cref="AssetIconsRenderCache"/>
	[Serializable]
	public struct AssetIconsCameraSetup : IEquatable<AssetIconsCameraSetup>
	{
		/// <summary>
		/// <para>A set of default settings to render a Prefab with.</para>
		/// </summary>
		public static AssetIconsCameraSetup Default
		{
			get
			{
				return new AssetIconsCameraSetup()
				{
					Padding = 0.0f,

					TransparentBackground = false,
					BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f),

					Orthographic = false,
					PreviewDirection = new Vector3(-1.0f, -1.0f, -1.0f)
				};
			}
		}

		[Header("Positioning")]
		[SerializeField] private float padding;
		[SerializeField] private Vector3 previewDirection;

		[Header("Camera Settings")]
		[SerializeField] private bool transparentBackground;
		[SerializeField] private Color backgroundColor;
		[SerializeField] private bool orthographic;

		/// <summary>
		/// <para>Indicates whether the background should be rendered as transparent.</para>
		/// </summary>
		public bool TransparentBackground
		{
			get
			{
				return transparentBackground;
			}
			set
			{
				transparentBackground = value;
			}
		}

		/// <summary>
		/// <para>The vector direction to render a Prefab from.</para>
		/// </summary>
		public Vector3 PreviewDirection
		{
			get
			{
				return previewDirection;
			}
			set
			{
				previewDirection = value;
			}
		}

		/// <summary>
		/// <para>Padding around the edges of a rendered Prefab.</para>
		/// </summary>
		public float Padding
		{
			get
			{
				return padding;
			}
			set
			{
				padding = value;
			}
		}

		/// <summary>
		/// <para>The background color of the rendered graphic.</para>
		/// </summary>
		public Color BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				backgroundColor = value;
			}
		}

		/// <summary>
		/// <para>Indicates whether projection should be orthographic.</para>
		/// </summary>
		public bool Orthographic
		{
			get
			{
				return orthographic;
			}
			set
			{
				orthographic = value;
			}
		}

		/// <summary>
		/// <para>Applies this <see cref="AssetIconsCameraSetup"/> to a Unity <see cref="Camera"/>.</para>
		/// </summary>
		/// <param name="camera">The Unity <see cref="Camera"/> to be modified.</param>
		public void ApplyToCamera(Camera camera)
		{
			camera.orthographic = Orthographic;
			camera.backgroundColor = BackgroundColor;

			camera.clearFlags = TransparentBackground
				? CameraClearFlags.Depth
				: CameraClearFlags.Color;
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="AssetIconsCameraSetup"/> is equal to an <see cref="object"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="object"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="object"/> is a <see cref="AssetIconsCameraSetup"/>s and is equal to this; otherwise <c>false</c>.</para>
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is AssetIconsCameraSetup && Equals((AssetIconsCameraSetup)obj);
		}

		/// <summary>
		/// <para>Evaluates whether this <see cref="AssetIconsCameraSetup"/> is equal to another <see cref="AssetIconsCameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="AssetIconsCameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="AssetIconsCameraSetup"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public bool Equals(AssetIconsCameraSetup other)
		{
			return Padding == other.Padding &&
				   PreviewDirection == other.PreviewDirection &&
				   TransparentBackground == other.TransparentBackground &&
				   BackgroundColor == other.BackgroundColor &&
				   Orthographic == other.Orthographic;
		}

		/// <summary>
		/// <para>Returns a unique hash for this of <see cref="AssetIconsCameraSetup"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>An <see cref="int"/> that represents a unique has of this instance.</para>
		/// </returns>
		public override int GetHashCode()
		{
			int hashCode = -1977805934;
			hashCode = hashCode * -1521134295 + Padding.GetHashCode();
			hashCode = hashCode * -1521134295 + PreviewDirection.GetHashCode();
			hashCode = hashCode * -1521134295 + TransparentBackground.GetHashCode();
			hashCode = hashCode * -1521134295 + BackgroundColor.GetHashCode();
			hashCode = hashCode * -1521134295 + Orthographic.GetHashCode();
			return hashCode;
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="AssetIconsCameraSetup"/> is equal to another <see cref="AssetIconsCameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="AssetIconsCameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="AssetIconsCameraSetup"/>s are equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator ==(AssetIconsCameraSetup left, AssetIconsCameraSetup right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// <para>Evaluates whether a <see cref="AssetIconsCameraSetup"/> is not equal to another <see cref="AssetIconsCameraSetup"/>.</para>
		/// </summary>
		/// <param name="other">The other <see cref="AssetIconsCameraSetup"/> to compare against.</param>
		/// <returns>
		/// <para><c>true</c> if the <see cref="AssetIconsCameraSetup"/>s are not equal; otherwise <c>false</c>.</para>
		/// </returns>
		public static bool operator !=(AssetIconsCameraSetup left, AssetIconsCameraSetup right)
		{
			return !(left == right);
		}
	}
}

#pragma warning restore
#endif
