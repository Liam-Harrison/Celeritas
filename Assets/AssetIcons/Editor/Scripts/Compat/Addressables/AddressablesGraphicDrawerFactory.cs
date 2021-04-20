//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR && ASSETICONS_ADDRESSABLES
#pragma warning disable

using AssetIcons.Editors;
using AssetIcons.Editors.Pipeline;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Unity​Engine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetIcons.Editors.Compat.Addressables
{
	/// <summary>
	/// <para>Extends AssetIcons with support for Unity's Addressables system.</para>
	/// </summary>
	internal class AddressablesGraphicDrawerFactory : IGraphicDrawerFactory
	{
		private class AddressablesGraphicDrawer : IGraphicDrawer
		{
			private readonly AddressablesGraphicDrawerFactory factory;
			private AssetReference assetReference;
			private IGraphicDrawer wrappedDrawer;
			private Type wrappedDrawerType;

			public AddressablesGraphicDrawer(AddressablesGraphicDrawerFactory factory)
			{
				this.factory = factory;
			}

			public bool CanDraw()
			{
				return wrappedDrawer != null
					&& wrappedDrawer.CanDraw();
			}

			public void Draw(Rect rect, bool selected, AssetIconsCompiledStyle style)
			{
				if (wrappedDrawer != null)
				{
					wrappedDrawer.Draw(rect, selected, style);
				}
			}

			public void SetValue(object value)
			{
				assetReference = value as AssetReference;

				if (assetReference != null && assetReference.editorAsset != null)
				{
					var resultType = assetReference.editorAsset.GetType();

					if (wrappedDrawer == null || wrappedDrawerType != resultType)
					{
						wrappedDrawer = factory.pipeline.CreateGraphicDrawer(resultType);
						wrappedDrawerType = resultType;
					}
					if (wrappedDrawer != null)
					{
						wrappedDrawer.SetValue(assetReference.editorAsset);
					}
				}
				else
				{
					wrappedDrawer = null;
				}
			}
		}

		private readonly IAssetIconPipeline pipeline;

		public int Priority
		{
			get
			{
				return 150;
			}
		}

		public AddressablesGraphicDrawerFactory(IAssetIconPipeline pipeline)
		{
			this.pipeline = pipeline;
		}

		public IGraphicDrawer CreateDrawer()
		{
			return new AddressablesGraphicDrawer(this);
		}

		public bool IsValidFor(Type type)
		{
			return typeof(AssetReference).IsAssignableFrom(type);
		}
	}
}

#pragma warning restore
#endif
