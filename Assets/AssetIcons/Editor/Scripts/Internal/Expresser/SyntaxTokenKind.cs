//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

namespace AssetIcons.Editors.Internal.Expresser
{
	internal enum SyntaxTokenKind : byte
	{
		None,

		// Maths
		Plus,
		Minus,
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

		// Suffix
		Percentage,

		// Data
		Value,
		Source,

		// Structure
		OpenParentheses,
		CloseParentheses,
		Comma,
	}
}

#pragma warning restore
#endif
