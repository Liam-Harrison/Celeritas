//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>A cache for <see cref="AssetTarget"/>s.</para>
	/// </summary>
	internal class AssetTargetCache
	{
		private readonly Dictionary<string, AssetTarget> cacheCollection;

		internal AssetTargetCache()
		{
			cacheCollection = new Dictionary<string, AssetTarget>();
		}

		public AssetTarget GetOrCreateFromGuid(string guid)
		{
			AssetTarget assetDrawer;
			if (!cacheCollection.TryGetValue(guid, out assetDrawer))
			{
				assetDrawer = AssetTarget.CreateFromGUID(guid);
				cacheCollection[guid] = assetDrawer;
			}
			return assetDrawer;
		}
	}
}

#pragma warning restore
#endif
