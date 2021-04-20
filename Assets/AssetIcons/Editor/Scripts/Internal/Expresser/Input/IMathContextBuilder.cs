//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal interface IMathContextBuilder
	{
		IMathContext Build();

		IMathContextBuilder ImplicitlyReferences(IValueProvider value);

		IMathContextBuilder WithTerm(string term, IValueProvider value);

		IMathContextBuilder WithUnit(string unit, IValueProvider value);
	}
}

#pragma warning restore
#endif
