//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser.Processing
{
	/// <summary>
	/// <para>A code representing the type of operation that an <see cref="IntermediateOperation"/> represents.</para>
	/// </summary>
	internal enum IntermediateOperationCode
	{
		None,

		// Maths
		Add,
		Subtract,
		Multiply,
		Divide,
		Power,

		// Logic
		And,
		Or,
		Not,
		Equal,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		Percentage,

		Invoke,
		Copy
	}
}

#pragma warning restore
#endif
