//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Reflection;
using UnityEditor;

namespace AssetIcons.Editors.Pipeline.Default
{
	internal class DefaultCompat : IAssetIconsExtension
	{
		public void Initialise(IAssetIconPipeline pipeline)
		{
			pipeline.RegisterDrawer(new DefaultColor32GraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultColorGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultDecimalGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultGameObjectGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultGUISkinGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultIntegerGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultSpriteGraphicDrawerFactory());
			pipeline.RegisterDrawer(new DefaultTextureGraphicDrawerFactory());
		}
	}
}

#pragma warning restore
#endif
