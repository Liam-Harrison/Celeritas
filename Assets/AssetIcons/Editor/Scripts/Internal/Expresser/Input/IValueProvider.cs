//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	/// <summary>
	/// <para></para>
	/// </summary>
	internal interface IValueProvider
	{
		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		MathValue Value { get; }
	}
}

#pragma warning restore
#endif
