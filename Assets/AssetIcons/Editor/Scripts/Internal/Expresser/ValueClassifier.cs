//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser
{
	internal enum ValueClassifier : byte
	{
		None,
		Boolean,
		Float,
		FloatFractional,
		Int,
		IntFractional
	}
}

#pragma warning restore
#endif
