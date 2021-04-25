//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Drawing;
using AssetIcons.Editors.Pipeline;
using AssetIcons.Editors.Preferences;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

namespace AssetIcons.Editors.Internal
{
	internal class AssetIconPipeline : IAssetIconPipeline
	{
		private List<IGraphicDrawerFactory> graphicDrawers;
		private Dictionary<Type, IGraphicDrawerFactory> graphicDrawerCache;
		private AssetDrawerFactoryLibrary library;
		private AssetTargetCache assetTargets;

		public List<IGraphicDrawerFactory> GraphicDrawers
		{
			get
			{
				return graphicDrawers;
			}
		}

		internal AssetDrawerFactoryLibrary Library
		{
			get
			{
				return library;
			}
		}

		internal AssetTargetCache AssetTargets
		{
			get
			{
				return assetTargets;
			}
		}

		internal AssetIconPipeline(IEnumerable<IAssetIconsExtension> extensions)
		{
			graphicDrawers = new List<IGraphicDrawerFactory>();
			graphicDrawerCache = new Dictionary<Type, IGraphicDrawerFactory>();

			foreach (var extension in extensions)
			{
				extension.Initialise(this);
			}

			assetTargets = new AssetTargetCache();
			library = new AssetDrawerFactoryLibrary(this);
		}

		public void RegisterDrawer(IGraphicDrawerFactory graphicDrawer)
		{
			graphicDrawers.Add(graphicDrawer);
		}

		public bool TryGetGraphicDrawerFactory(Type key, out IGraphicDrawerFactory value)
		{
			if (!graphicDrawerCache.TryGetValue(key, out value))
			{
				foreach (var graphicDrawer in graphicDrawers)
				{
					if (graphicDrawer.IsValidFor(key))
					{
						value = graphicDrawer;
						break;
					}
				}
				graphicDrawerCache[key] = value;
			}
			return value != null;
		}

		public bool IsSupportedType(Type type)
		{
			IGraphicDrawerFactory factory;
			return TryGetGraphicDrawerFactory(type, out factory);
		}

		public IGraphicDrawer CreateGraphicDrawer(Type type)
		{
			IGraphicDrawerFactory drawerFactory;
			if (TryGetGraphicDrawerFactory(type, out drawerFactory))
			{
				return drawerFactory.CreateDrawer();
			}
			else
			{
				Debug.LogError("Failed to get a graphic drawer for type " + type.Name + ".");
				return null;
			}
		}
	}
}

#pragma warning restore
#endif
