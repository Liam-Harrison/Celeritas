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
	internal class StaticValueProvider : IValueProvider
	{
		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public MathValue Value { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(MathValue value)
		{
			Value = value;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(float value)
		{
			Value = new MathValue(value, false);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="value"></param>
		public StaticValueProvider(bool value)
		{
			Value = new MathValue(value);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}

#pragma warning restore
#endif
