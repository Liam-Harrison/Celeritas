//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR && ASSETICONS_ADDRESSABLES
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System;
using UnityEditor;

namespace AssetIcons.Editors.Compat.Addressables
{
	internal class AddressablesCompat : IAssetIconsExtension
	{
		public void Initialise(IAssetIconPipeline pipeline)
		{
			pipeline.RegisterDrawer(new AddressablesGraphicDrawerFactory(pipeline));
		}
	}
}

#pragma warning restore
#endif
