//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>A collection of resources required for AssetIcons in editor.</para>
	/// </summary>
	/// <remarks>
	/// <para>This class inherits from a utility for providing simple singleton instances for Unity's <see cref="ScriptableObject"/>.</para>
	/// </remarks>
	internal class AssetIconsResources : ThemedResourceCollection<AssetIconsResources>
	{
#pragma warning disable

		[Header("Preferences Window")]
		[Tooltip("A sprite drawn in the Preferences window to preview effects.")]
		[SerializeField] private Sprite sampleImage;

		[AssetIcon]
		[Tooltip("The icon to use on the updater window.")]
		[SerializeField] private Texture2D updaterIcon;
		[Tooltip("The icon to use to show a version on the update checker.")]
		[SerializeField] private Texture2D circleTexture;

		[Space]
		[Tooltip("The color of the version node passively.")]
		[SerializeField] private Color normalVersionColor;
		[SerializeField] private Color futureVersionColor;

		[Space]
		[Tooltip("The color of the version node when it's not currently installed.")]
		[SerializeField] private Color normalVersionLineColor;
		[Tooltip("The color of the version node connections when one of them are currently installed.")]
		[SerializeField] private Color futureVersionLineColor;

		[Space]
		[Tooltip("The color of the version row passively.")]
		[SerializeField] private Color normalVersionBackgroundColor;
		[Tooltip("The color of the version row it's the current version.")]
		[SerializeField] private Color currentVersionBackgroundColor;
		[Tooltip("The color of the version row when it's not currently installed.")]
		[SerializeField] private Color futureVersionBackgroundColor;

		[Space]
		[Tooltip("The color of the version row passively.")]
		[SerializeField] private Color normalVersionBackgroundHoverColor;
		[Tooltip("The color of the version row when it's the current version.")]
		[SerializeField] private Color currentVersionBackgroundHoverColor;
		[Tooltip("The color of the version row when it's not currently installed.")]
		[SerializeField] private Color futureVersionBackgroundHoverColor;

		[Space]
		[SerializeField] private Texture2D unityLogo;
		[SerializeField] private Texture2D githubLogo;

		[Header("Rating")]
		[SerializeField] private Texture2D crossIcon;
		[SerializeField] private Color crossNormalColour;

		[Space]
		[SerializeField] private Texture2D emojisBackground;
		[SerializeField] private Texture2D[] emojisForRatings;

		[Space]
		[SerializeField] private Texture2D starBackground;
		[SerializeField] private Texture2D starIcon;
		[SerializeField] private Color logoColor;
		[SerializeField] private Color starNormalColor;
		[SerializeField] private Color starHighlightedColor;

#pragma warning restore

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Light theme.</para>
		/// </summary>
		public override string LightThemePath
		{
			get
			{
				return "AssetIcon Resources (Light)";
			}
		}

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Dark theme.</para>
		/// </summary>
		public override string DarkThemePath
		{
			get
			{
				return "AssetIcon Resources (Dark)";
			}
		}

		/// <summary>
		/// <para>A sprite drawn in the Preferences window to preview effects.</para>
		/// </summary>
		public Sprite SampleImage
		{
			get
			{
				return sampleImage;
			}
		}

		/// <summary>
		/// <para>The icon to use on the updater window.</para>
		/// </summary>
		public Texture2D UpdaterIcon
		{
			get
			{
				return updaterIcon;
			}
		}

		/// <summary>
		/// <para>The icon to use to show a version on the update checker.</para>
		/// </summary>
		public Texture2D CircleTexture
		{
			get
			{
				return circleTexture;
			}
		}

		/// <summary>
		/// <para>The color of the version node passively.</para>
		/// </summary>
		public Color NormalVersionColor
		{
			get
			{
				return normalVersionColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionColor
		{
			get
			{
				return futureVersionColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node connections when both of them are currently installed.</para>
		/// </summary>
		public Color NormalVersionLineColor
		{
			get
			{
				return normalVersionLineColor;
			}
		}

		/// <summary>
		/// <para>The color of the version node connections when one of them are currently installed.</para>
		/// </summary>
		public Color FutureVersionLineColor
		{
			get
			{
				return futureVersionLineColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row passively.</para>
		/// </summary>
		public Color NormalVersionBackgroundColor
		{
			get
			{
				return normalVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row it's the current version.</para>
		/// </summary>
		public Color CurrentVersionBackgroundColor
		{
			get
			{
				return currentVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionBackgroundColor
		{
			get
			{
				return futureVersionBackgroundColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row passively.</para>
		/// </summary>
		public Color NormalVersionBackgroundHoverColor
		{
			get
			{
				return normalVersionBackgroundHoverColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's the current version.</para>
		/// </summary>
		public Color CurrentVersionBackgroundHoverColor
		{
			get
			{
				return currentVersionBackgroundHoverColor;
			}
		}

		/// <summary>
		/// <para>The color of the version row when it's not currently installed.</para>
		/// </summary>
		public Color FutureVersionBackgroundHoverColor
		{
			get
			{
				return futureVersionBackgroundHoverColor;
			}
		}

		/// <summary>
		/// <para>A plain background which contains emojis.</para>
		/// </summary>
		public Texture2D EmojisBackground
		{
			get
			{
				return emojisBackground;
			}
			set
			{
				emojisBackground = value;
			}
		}

		/// <summary>
		/// <para>Emojis for every one of the 5-star ratings.</para>
		/// </summary>
		public Texture2D[] EmojisForRatings
		{
			get
			{
				return emojisForRatings;
			}
			set
			{
				emojisForRatings = value;
			}
		}

		/// <summary>
		/// <para>A white star icon that is tinted by <see cref="StarNormalColor"/> and <see cref="StarHighlightedColor"/>.</para>
		/// </summary>
		public Texture2D StarIcon
		{
			get
			{
				return starIcon;
			}
			set
			{
				starIcon = value;
			}
		}

		/// <summary>
		/// <para>The colour of the <see cref="StarIcon"/> normally.</para>
		/// </summary>
		public Color StarNormalColor
		{
			get
			{
				return starNormalColor;
			}
			set
			{
				starNormalColor = value;
			}
		}

		/// <summary>
		/// <para>The colour of the <see cref="StarIcon"/> when it is highlighted.</para>
		/// </summary>
		public Color StarHighlightedColor
		{
			get
			{
				return starHighlightedColor;
			}
			set
			{
				starHighlightedColor = value;
			}
		}

		/// <summary>
		/// <para>An long background used to render around the 5 star ratings.</para>
		/// </summary>
		public Texture2D StarBackground
		{
			get
			{
				return starBackground;
			}
			set
			{
				starBackground = value;
			}
		}

		/// <summary>
		/// <para>A plain white Unity logo.</para>
		/// </summary>
		public Texture2D UnityLogo
		{
			get
			{
				return unityLogo;
			}
			set
			{
				unityLogo = value;
			}
		}

		/// <summary>
		/// <para>A plain white GitHub logo.</para>
		/// </summary>
		public Texture2D GithubLogo
		{
			get
			{
				return githubLogo;
			}
			set
			{
				githubLogo = value;
			}
		}

		/// <summary>
		/// <para>The colour for rendering white logos.</para>
		/// </summary>
		public Color LogoColor
		{
			get
			{
				return logoColor;
			}
			set
			{
				logoColor = value;
			}
		}

		/// <summary>
		/// <para>A white cross icon for closing the review dialogue.</para>
		/// </summary>
		public Texture2D CrossIcon
		{
			get
			{
				return crossIcon;
			}
			set
			{
				crossIcon = value;
			}
		}

		/// <summary>
		/// <para>The passive colour of the cross icon.</para>
		/// </summary>
		public Color CrossNormalColour
		{
			get
			{
				return crossNormalColour;
			}
			set
			{
				crossNormalColour = value;
			}
		}
	}
}

#pragma warning restore
#endif
