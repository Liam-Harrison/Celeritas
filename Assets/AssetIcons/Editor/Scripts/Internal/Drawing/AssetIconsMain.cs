//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using AssetIcons.Editors.Preferences;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>The central hub of AssetIcons that ties all of the smaller components together to draw them.</para>
	/// </summary>
	[InitializeOnLoad]
	internal static class AssetIconsMain
	{
		public static HashSet<string> UnsupportedExtensions = new HashSet<string>()
		{
			".asset",
			".bmp",
			".exr",
			".gif",
			".hdr",
			".iff",
			".jpeg",
			".jpg",
			".pict",
			".png",
			".psd",
			".tga",
			".tiff"
		};

		private static AssetIconPipeline pipeline;

		public static AssetIconPipeline Pipeline
		{
			get
			{
				return pipeline;
			}
		}

		static AssetIconsMain()
		{
			AssetIconsProjectHooks.OnInternalDrawIcon += ItemOnGUI;

			var extensions = new List<IAssetIconsExtension>();

			var checkAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in checkAssemblies)
			{
				Type[] assemblyTypes;
				try
				{
					assemblyTypes = assembly.GetTypes();
				}
				catch
				{
					continue;
				}

#if !UNITY_2017_1_OR_NEWER
				string assemblyName = assembly.FullName.Substring (0, assembly.FullName.IndexOf (','));

				if (assemblyName != "Assembly-UnityScript" &&
					assemblyName != "Assembly-CSharp" &&
					assemblyName != "Assembly-CSharp-Editor")
					continue;
#endif

				foreach (var type in assemblyTypes)
				{
					if (!typeof(IAssetIconsExtension).IsAssignableFrom(type))
					{
						continue;
					}
					if (type.IsAbstract || type.IsInterface)
					{
						continue;
					}

					var instance = (IAssetIconsExtension)Activator.CreateInstance(type);

					extensions.Add(instance);
				}
			}

			pipeline = new AssetIconPipeline(extensions);
		}

		/// <summary>
		/// <para>Paints the item in the project window.</para>
		/// </summary>
		/// <param name="guid">The GUID of the asset to check.</param>
		/// <param name="rect">The Rect in which the item is drawn.</param>
		private static void ItemOnGUI(string guid, Rect rect)
		{
			if (Event.current.type != EventType.Repaint || string.IsNullOrEmpty(guid))
			{
				return;
			}

			if (!AssetIconsPreferences.Enabled.Value)
			{
				return;
			}

			var assetTarget = pipeline.AssetTargets.GetOrCreateFromGuid(guid);

			if (string.IsNullOrEmpty(assetTarget.Extension))
			{
				return;
			}

			if (assetTarget.Extension == ".asset")
			{
				if (assetTarget.Drawer != null)
				{
					if (assetTarget.Drawer.CanDraw())
					{
						var backgroundRect = AssetIconsGUIUtility.AreaToIconRect(rect, 128);
						AssetIconsGUI.DrawBackground(backgroundRect);

						assetTarget.Drawer.Draw(rect, assetTarget.IsSelected);
					}
				}
			}
			else if (AssetIconsPreferences.DrawGUIStyles.Value && assetTarget.Extension == ".guiskin")
			{
				var obj = assetTarget.ObjectReference;
				if (obj is GUISkin)
				{
					rect = AssetIconsGUIUtility.AreaToIconRect(rect);
					AssetIconsGUI.DrawBackground(rect);

					var skin = (GUISkin)obj;
					skin.box.Draw(rect, new GUIContent("Style"), 0, assetTarget.IsSelected);
				}
			}
			else
			{
				var icon = AssetIconsPreferences.TypeIcons[assetTarget.Extension];
				if (icon != null && icon.ObjectReference != null && icon.GraphicDrawer != null)
				{
					if (icon.GraphicDrawer.CanDraw())
					{
						AssetIconsGUI.DrawBackground(rect);

						icon.GraphicDrawer.Draw(rect, assetTarget.IsSelected, AssetIconsCompiledStyle.Default);
					}
				}
			}
		}
	}
}

#pragma warning restore
#endif
