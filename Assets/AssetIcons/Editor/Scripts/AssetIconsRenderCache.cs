//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>Cache renders of the models that we would like to render.</para>
	/// </summary>
	/// <seealso cref="AssetIconsRenderer"/>
	/// <seealso cref="AssetIconsCameraSetup"/>
	public sealed class AssetIconsRenderCache : UnityEditor.AssetModificationProcessor
	{
		private static Dictionary<AssetIconsCameraSetup, Dictionary<string, Texture2D>> Renders { get; set; }

		static AssetIconsRenderCache()
		{
			Renders = new Dictionary<AssetIconsCameraSetup, Dictionary<string, Texture2D>>();
		}

		/// <summary>
		/// <para>Retrieves a <see cref="Texture2D"/> from the cache for a <see cref="GameObject"/> using a <see cref="AssetIconsCameraSetup"/>.</para>
		/// </summary>
		/// <param name="cameraSetup">A camera setup to use when rendering the graphic.</param>
		/// <param name="target">A Prefab to render a graphic for.</param>
		/// <returns>
		/// <para>A <see cref="Texture2D"/> of the renedred <see cref="GameObject"/> with the <see cref="AssetIconsCameraSetup"/>.</para>
		/// </returns>
		public static Texture2D GetTexture(AssetIconsCameraSetup cameraSetup, GameObject target)
		{
			string path = AssetDatabase.GetAssetPath(target);

			Dictionary<string, Texture2D> styleCache;

			bool result = Renders.TryGetValue(cameraSetup, out styleCache);

			if (!result)
			{
				styleCache = new Dictionary<string, Texture2D>();
				Renders.Add(cameraSetup, styleCache);
			}

			Texture2D texture;
			result = styleCache.TryGetValue(path, out texture);

			if (!result || texture == null || texture.Equals(null))
			{
				int size = AssetIconsPreferences.PrefabResolution.Value;

				texture = AssetIconsRenderer.RenderModel(target, cameraSetup, size, size);

				styleCache[path] = texture;
			}

			return texture;
		}

		/// <summary>
		/// <para>Clears all rendered textures from this cache.</para>
		/// </summary>
		public static void ClearCache()
		{
			Renders.Clear();
		}

		/// <summary>
		/// <para>Listens for an asset being saved, in this case we will mostly be listening for a Prefab
		/// to be saved, however this can also listen for anything that imports a Prefab to be monitored.</para>
		/// </summary>
		/// <param name="paths">The paths of the assets that will be saved.</param>
		private static string[] OnWillSaveAssets(string[] paths)
		{
			foreach (var cache in Renders)
			{
				foreach (string path in paths)
				{
					cache.Value.Remove(path);
				}
			}

			return paths;
		}
	}
}

#pragma warning restore
#endif
