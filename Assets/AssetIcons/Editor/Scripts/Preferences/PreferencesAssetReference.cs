//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal;
using AssetIcons.Editors.Internal.Drawing;
using AssetIcons.Editors.Pipeline;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>Used to reference assets inside the Unity project inside the editor.</para>
	/// </summary>
	/// <remarks>
	/// <para>This is used inside the <see cref="AssetIconsPreferencesPreset"/> to select assets to render an icon.</para>
	/// </remarks>
	[Serializable]
	public sealed class PreferencesAssetReference
	{
		[FormerlySerializedAs("AssetPath")]
		[SerializeField] private string assetPath;

		[FormerlySerializedAs("AssetName")]
		[SerializeField] private string assetName;

		[NonSerialized] private string objectAssetPath;
		[NonSerialized] private string objectAssetName;
		[NonSerialized] private UnityEngine.Object objectReference;
		[NonSerialized] private IGraphicDrawer graphicDrawer;

		/// <summary>
		/// <para>A <see cref="IGraphicDrawer"/> for the asset referenced by this <see cref="PreferencesAssetReference"/>.</para>
		/// </summary>
		public IGraphicDrawer GraphicDrawer
		{
			get
			{
				return graphicDrawer;
			}
		}

		/// <summary>
		/// <para>A path, relative to the Unity project root, used to reference the asset.</para>
		/// </summary>
		public string AssetPath
		{
			get
			{
				return assetPath;
			}
			set
			{
				assetPath = value;
			}
		}

		/// <summary>
		/// <para>The <see cref="UnityEngine.Object.name"/> of the asset.</para>
		/// </summary>
		public string AssetName
		{
			get
			{
				return assetName;
			}
			set
			{
				assetName = value;
			}
		}

		/// <summary>
		/// <para>Load or assign assets from the <see cref="AssetDatabase"/>.</para>
		/// </summary>
		public UnityEngine.Object ObjectReference
		{
			get
			{
				if (string.IsNullOrEmpty(AssetPath))
				{
					return null;
				}

				if (objectReference == null
					|| objectReference.Equals(null)
					|| objectAssetPath != AssetPath
					|| objectAssetName != AssetName)
				{
					objectAssetPath = AssetPath;
					objectAssetName = AssetName;
					objectReference = GetFromPath(AssetPath, AssetName);

					if (objectReference != null)
					{
						var objectReferenceType = objectReference.GetType();
						graphicDrawer = AssetIconsMain.Pipeline.CreateGraphicDrawer(objectReferenceType);
						if (graphicDrawer != null)
						{
							graphicDrawer.SetValue(objectReference);
						}
					}
					else
					{
						graphicDrawer = null;
					}
				}

				return objectReference;
			}
			set
			{
				objectAssetPath = null;
				objectAssetName = null;
				objectReference = null;

				if (value != null || value.Equals(null))
				{
					AssetPath = AssetDatabase.GetAssetPath(value);
					AssetName = value.name;
				}
			}
		}

		/// <summary>
		/// <para>Loads an asset from the <see cref="AssetDatabase"/> with the specified name.</para>
		/// </summary>
		/// <param name="assetPath">A path, relative to the Unity project root, used to reference the asset.</param>
		/// <param name="assetName">The <see cref="UnityEngine.Object.name"/> of the asset.</param>
		/// <returns>
		/// <para>This the same logic as an <see cref="PreferencesAssetReference"/> would use to locate an asset, used in editor scripting.</para>
		/// </returns>
		public static UnityEngine.Object GetFromPath(string assetPath, string assetName)
		{
			if (assetPath.EndsWith(".unity"))
			{
				return null;
			}

			var references = AssetDatabase.LoadAllAssetsAtPath(assetPath);

			if (references.Length == 0)
			{
				return null;
			}

			foreach (var reference in references)
			{
				if (reference.name == assetName)
				{
					return reference;
				}
			}

			return references[0];
		}
	}
}

#pragma warning restore
#endif
