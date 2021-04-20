//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal interface IMathContext
	{
		IValueProvider ImplicitReference { get; }

		bool TryGetTerm(string key, out IValueProvider provider);

		bool TryGetUnit(string key, out IValueProvider provider);

	}
}

#pragma warning restore
#endif
