//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Expresser.Input;

namespace AssetIcons.Editors.Internal.Expresser.Processing
{
	/// <summary>
	/// <para>A source for values within an <see cref="IntermediateExpression"/>.</para>
	/// </summary>
	internal enum IntermediateSource
	{
		/// <summary>
		/// <para>The value is sourced from a collection of static values.</para>
		/// </summary>
		Static,

		/// <summary>
		/// <para>The value is sourced from an <see cref="IValueProvider"/>.</para>
		/// </summary>
		Import,

		/// <summary>
		/// <para>The value is sourced from an <see cref="IValueProvider"/> with a negated value.</para>
		/// </summary>
		ImportNegated,

		/// <summary>
		/// <para>The value is sourced from a buffer of outputted values.</para>
		/// </summary>
		Output,
	}
}

#pragma warning restore
#endif
