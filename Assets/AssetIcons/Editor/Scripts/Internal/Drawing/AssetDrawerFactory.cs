//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Describes a collection of graphics that make up a GUI draw.</para>
	/// </summary>
	internal class AssetDrawerFactory
	{
		private readonly AssetIconPipeline pipeline;

		/// <summary>
		/// <para>The collection of graphics that make up a GUI draw.</para>
		/// </summary>
		private readonly GraphicSource[] elements;

		/// <summary>
		/// <para>Constructs a new AssetDrawer with a collection of graphics.</para>
		/// </summary>
		/// <param name="elements">A collection of graphics that make up this drawer.</param>
		public AssetDrawerFactory(AssetIconPipeline pipeline, GraphicSource[] elements)
		{
			this.pipeline = pipeline;
			this.elements = elements;
		}

		public AssetDrawer CreateAssetDrawer(object target)
		{
			int length = this.elements.Length;
			var graphics = new IAssetGraphic[length];
			var drawers = new IGraphicDrawer[length];
			for (int i = 0; i < length; i++)
			{
				var graphic = elements[i].CreateAssetGraphic(target);
				graphics[i] = graphic;
				drawers[i] = pipeline.CreateGraphicDrawer(graphic.GraphicType);
			}

			return new AssetDrawer(graphics, drawers);
		}
	}
}

#pragma warning restore
#endif
