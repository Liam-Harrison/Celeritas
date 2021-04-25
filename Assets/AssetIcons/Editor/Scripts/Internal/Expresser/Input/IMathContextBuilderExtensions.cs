//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal static class IMathContextBuilderExtensions
	{
		public static IMathContextBuilder WithTerm(this IMathContextBuilder builder, string term, MathValue mathValue)
		{
			return builder.WithTerm(term, new StaticValueProvider(mathValue));
		}
	}
}

#pragma warning restore
#endif
