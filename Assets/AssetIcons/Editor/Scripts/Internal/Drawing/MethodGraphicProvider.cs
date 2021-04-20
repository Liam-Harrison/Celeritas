//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System;
using System.Reflection;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Used to abstract getting of a value from an object and drawing of graphics.</para>
	/// </summary>
	internal class MethodGraphicProvider : GraphicSource
	{
		private class MethodAssetGraphic : IAssetGraphic
		{
			private readonly MethodGraphicProvider provider;
			private readonly object target;

			public object Graphic
			{
				get
				{
					return provider.method.Invoke(target, null);
				}
			}

			public Type GraphicType
			{
				get
				{
					return provider.method.ReturnType;
				}
			}

			public AssetIconsCompiledStyle Style
			{
				get
				{
					return provider.Style;
				}
			}

			public MethodAssetGraphic(MethodGraphicProvider provider, object target)
			{
				this.provider = provider;
				this.target = target;
			}
		}

		private readonly MethodInfo method;

		/// <summary>
		/// <para>Constructs a new instance of the <see cref="MethodGraphicProvider"/>.</para>
		/// </summary>
		/// <param name="style">A style to use to draw graphics provided by this <see cref="GraphicSource"/>.</param>
		/// <param name="method">A method that's invoked to provide the graphic.</param>
		public MethodGraphicProvider(AssetIconsCompiledStyle style, MethodInfo method)
			: base(style)
		{
			this.method = method;
		}

		public override IAssetGraphic CreateAssetGraphic(object target)
		{
			return new MethodAssetGraphic(this, target);
		}

		public override string ToString()
		{
			return string.Format("Icon from {0}", method.Name);
		}
	}
}

#pragma warning restore
#endif
