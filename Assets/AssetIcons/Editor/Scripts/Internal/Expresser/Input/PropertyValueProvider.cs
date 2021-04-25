//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System.Reflection;

namespace AssetIcons.Editors.Internal.Expresser.Input
{
	/// <summary>
	/// <para></para>
	/// </summary>
	internal class PropertyValueProvider : IValueProvider, IObjectContext
	{
		private readonly PropertyInfo targetProperty;

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public MathValue Value
		{
			get
			{
				return new MathValue(targetProperty.GetValue(Target, null));
			}
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		public object Target { get; set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetProperty"></param>
		public PropertyValueProvider(PropertyInfo targetProperty)
		{
			this.targetProperty = targetProperty;
			Target = null;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="targetProperty"></param>
		/// <param name="targetObject"></param>
		public PropertyValueProvider(PropertyInfo targetProperty, object targetObject)
		{
			this.targetProperty = targetProperty;
			Target = targetObject;
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return targetProperty.Name;
		}
	}
}

#pragma warning restore
#endif
