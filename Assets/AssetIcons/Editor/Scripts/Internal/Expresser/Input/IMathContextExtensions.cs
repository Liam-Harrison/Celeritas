//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	internal static class IMathContextExtensions
	{
		public static IValueProvider[] ResolveTerms(this IMathContext context, string[] terms)
		{
			if (context == null)
			{
				if (terms != null && terms.Length != 0)
				{
					throw new InvalidOperationException("Could not resolve terms with no math context provided");
				}

				return null;
			}

			int termsCount = terms.Length;
			var providers = new IValueProvider[termsCount];

			for (int i = 0; i < termsCount; i++)
			{
				string term = terms[i];
				IValueProvider provider;
				if (!context.TryGetTerm(term, out provider))
				{
					throw new InvalidOperationException(string.Format("Unable to find value for term \"{0}\"", term));
				}
				providers[i] = provider;
			}
			return providers;
		}
	}
}

#pragma warning restore
#endif
