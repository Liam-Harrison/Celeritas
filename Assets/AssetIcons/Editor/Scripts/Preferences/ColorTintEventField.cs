//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>A concrete implementation of the <see cref="EventField"/> for <see cref="ColorTint"/>.</para>
	/// </summary>
	/// <remarks>
	/// <para>This is nessesary for Unity to be able to serialize generic types.</para>
	/// </remarks>
	/// <seealso cref="BoolEventField"/>
	/// <seealso cref="IntEventField"/>
	/// <seealso cref="ColorTint"/>
	[Serializable]
	public sealed class ColorTintEventField : EventField<ColorTint>
	{
		/// <summary>
		/// <para>Constructs a new instance of this <see cref="ColorTintEventField"/>.</para>
		/// </summary>
		public ColorTintEventField()
		{
		}

		/// <summary>
		/// <para>Constructs a new instance of this with a default value.</para>
		/// </summary>
		/// <param name="defaultValue">The default value for this <see cref="ColorTintEventField"/>.</param>
		public ColorTintEventField(ColorTint defaultValue) : base(defaultValue)
		{
		}
	}
}

#pragma warning restore
#endif
