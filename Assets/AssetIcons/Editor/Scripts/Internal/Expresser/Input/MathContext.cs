//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System.Collections.Generic;

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	/// <summary>
	/// <para></para>
	/// </summary>
	internal class MathContext : IMathContext
	{
		private readonly Dictionary<string, IValueProvider> terms;
		private readonly Dictionary<string, IValueProvider> units;
		private readonly IValueProvider implicitReference;

		public IValueProvider ImplicitReference
		{
			get
			{
				return implicitReference;
			}
		}

		public MathContext(Dictionary<string, IValueProvider> terms, Dictionary<string, IValueProvider> units, IValueProvider implicitReference)
		{
			this.terms = terms;
			this.units = units;
			this.implicitReference = implicitReference;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool TryGetTerm(string key, out IValueProvider provider)
		{
			return terms.TryGetValue(key.ToLower(), out provider);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool TryGetUnit(string key, out IValueProvider provider)
		{
			return units.TryGetValue(key.ToLower(), out provider);
		}
	}
}

#pragma warning restore
#endif
