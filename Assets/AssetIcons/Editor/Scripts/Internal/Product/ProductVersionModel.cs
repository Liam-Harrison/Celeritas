//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Internal.Product
{
	[Serializable]
	internal class ProductVersionModel
	{
#pragma warning disable
		public string Version;
		public long Timestamp;
#pragma warning restore
	}
}

#pragma warning restore
#endif
