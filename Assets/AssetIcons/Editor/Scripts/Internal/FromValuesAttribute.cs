//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEngine;

namespace AssetIcons.Editors.Internal
{
	/// <summary>
	/// <para>A Unity <see cref="PropertyAttribute"/> that exposes a selection of values.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal sealed class FromValuesAttribute : PropertyAttribute
	{
		/// <summary>
		/// <para>The names of all of the values that this property allows.</para>
		/// </summary>
		public GUIContent[] Names { get; set; }

		/// <summary>
		/// <para>The values that this property allows.</para>
		/// </summary>
		public object[] Values { get; set; }

		/// <summary>
		/// <para>Allows for defining a strict range of values that are allowed on this property.</para>
		/// </summary>
		/// <param name="values">The values that this property allows.</param>
		public FromValuesAttribute(params object[] values)
		{
			if (values.Length == 0)
			{
				return;
			}

			Values = values;

			Type consistantType = null;
			Names = new GUIContent[Values.Length];

			for (int i = 0; i < Values.Length; i++)
			{
				object obj = values[i];

				if (consistantType == null)
				{
					consistantType = obj.GetType();
				}
				else
				{
					if (obj.GetType() != consistantType)
					{
						return;
					}
				}

				Names[i] = new GUIContent(obj.ToString());
			}
		}
	}
}

#pragma warning restore
#endif
