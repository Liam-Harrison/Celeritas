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
	/// <para>A concrete implementation of the <see cref="EventField"/> for <see cref="int"/>.</para>
	/// </summary>
	/// <remarks>
	/// <para>This is nessesary for Unity to be able to serialize generic types.</para>
	/// </remarks>
	/// <seealso cref="BoolEventField"/>
	/// <seealso cref="ColorTintEventField"/>
	[Serializable]
	public sealed class IntEventField : EventField<int>
	{
		/// <summary>
		/// <para>Constructs a new instance of this <see cref="IntEventField"/>.</para>
		/// </summary>
		public IntEventField()
		{
		}

		/// <summary>
		/// <para>Constructs a new instance of this with a default value.</para>
		/// </summary>
		/// <param name="defaultValue">The default value for this <see cref="IntEventField"/>.</param>
		public IntEventField(int defaultValue) : base(defaultValue)
		{
		}
	}
}

#pragma warning restore
#endif
