//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System;

namespace AssetIcons.Editors.Internal
{
	/// <summary>
	/// <para>A basic interface for supplying an object return type in a given context.</para>
	/// </summary>
	internal abstract class GraphicSource : IComparable<GraphicSource>
	{
		private readonly AssetIconsCompiledStyle style;

		public AssetIconsCompiledStyle Style
		{
			get
			{
				return style;
			}
		}

		public GraphicSource(AssetIconsCompiledStyle style)
		{
			this.style = style;
		}

		public abstract IAssetGraphic CreateAssetGraphic(object target);

		public int CompareTo(GraphicSource other)
		{
			if (other.Style == null)
			{
				return 1;
			}
			if (Style == null)
			{
				return -1;
			}

			if (Style.Layer != other.Style.Layer)
			{
				if (Style.Layer <= other.Style.Layer)
				{
					return -1;
				}
				return 1;
			}
			return 0;
		}
	}
}

#pragma warning restore
#endif
