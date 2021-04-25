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
	/// <para>Returns an object from a <see cref="FieldInfo"/> in a given context.</para>
	/// </summary>
	internal class FieldGraphicSource : GraphicSource
	{
		private class FieldAssetGraphic : IAssetGraphic
		{
			private readonly FieldGraphicSource provider;
			private readonly object target;

			public object Graphic
			{
				get
				{
					return provider.field.GetValue(target);
				}
			}

			public Type GraphicType
			{
				get
				{
					return provider.field.FieldType;
				}
			}

			public AssetIconsCompiledStyle Style
			{
				get
				{
					return provider.Style;
				}
			}

			public FieldAssetGraphic(FieldGraphicSource provider, object target)
			{
				this.provider = provider;
				this.target = target;
			}
		}

		private readonly FieldInfo field;

		public FieldGraphicSource(AssetIconsCompiledStyle style, FieldInfo field)
			: base(style)
		{
			this.field = field;
		}

		public override IAssetGraphic CreateAssetGraphic(object target)
		{
			return new FieldAssetGraphic(this, target);
		}

		public override string ToString()
		{
			return string.Format("Icon from {0}", field.Name);
		}
	}
}

#pragma warning restore
#endif
