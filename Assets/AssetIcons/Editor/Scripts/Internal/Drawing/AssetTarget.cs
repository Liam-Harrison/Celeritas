//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Contains the information of the target used in the modules.</para>
	/// </summary>
	internal class AssetTarget
	{
		private readonly string guid;

		private AssetDrawer drawer;
		private Object objectReference;
		private string extension;
		private string fileName;
		private string filePath;

		/// <summary>
		/// <para>An <see cref="AssetDrawer"/> constructed to draw the targeted asset.</para>
		/// </summary>
		public AssetDrawer Drawer
		{
			get
			{
				var drawerTarget = ObjectReference;
				if (drawerTarget == null)
				{
					return null;
				}

				if (Extension != ".asset")
				{
					return null;
				}

				if (drawer == null)
				{
					AssetDrawerFactory assetDrawerFactory;
					if (AssetIconsMain.Pipeline.Library.TryGetAssetDrawerFactory(drawerTarget.GetType(), out assetDrawerFactory))
					{
						drawer = assetDrawerFactory.CreateAssetDrawer(drawerTarget);
					}
				}

				return drawer;
			}
		}

		/// <summary>
		/// <para>The file extension of the asset.</para>
		/// </summary>
		public string Extension
		{
			get
			{
				return extension;
			}
		}

		/// <summary>
		/// <para>The file name of the asset.</para>
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
		}

		/// <summary>
		/// <para>The full file path of the asset.</para>
		/// </summary>
		public string FilePath
		{
			get
			{
				return filePath;
			}
		}

		/// <summary>
		/// <para>The GUID of the asset.</para>
		/// </summary>
		public string GUID
		{
			get
			{
				return guid;
			}
		}

		/// <summary>
		/// <para>A cached load of the Asset from the Unity <see cref="AssetDatabase"/>.</para>
		/// </summary>
		public Object ObjectReference
		{
			get
			{
				if (objectReference == null)
				{
					objectReference = AssetDatabase.LoadAssetAtPath<Object>(FilePath);
				}

				return objectReference;
			}
		}

		public bool IsSelected
		{
			get
			{
				return Selection.Contains(ObjectReference);
			}
		}

		private AssetTarget(string filePath, string guid)
		{
			this.guid = guid;
			this.filePath = filePath;

			extension = Path.GetExtension(filePath);
			fileName = Path.GetFileName(filePath);
			objectReference = null;
			drawer = null;
		}

		internal void RefreshFileInformation()
		{
			filePath = AssetDatabase.GUIDToAssetPath(guid);
			extension = Path.GetExtension(filePath);
			fileName = Path.GetFileName(filePath);
			objectReference = null;
			drawer = null;
		}

		/// <summary>
		/// <para>Creates an <see cref="AssetTarget"/> from a guid.</para>
		/// </summary>
		/// <param name="guid">The guid of the asset.</param>
		/// <returns>
		/// <para>An <see cref="AssetTarget"/> representing an asset with the specified guid.</para>
		/// </returns>
		internal static AssetTarget CreateFromGUID(string guid)
		{
			string filePath = AssetDatabase.GUIDToAssetPath(guid);

			return new AssetTarget(filePath, guid);
		}

		/// <summary>
		/// <para>Creates an <see cref="AssetTarget"/> from a local file path.</para>
		/// </summary>
		/// <param name="filePath">The local file path of the asset.</param>
		/// <returns>
		/// <para>An <see cref="AssetTarget"/> representing an asset at the specified path.</para>
		/// </returns>
		internal static AssetTarget CreateFromPath(string filePath)
		{
			string guid = AssetDatabase.AssetPathToGUID(filePath);

			return new AssetTarget(filePath, guid);
		}
	}
}

#pragma warning restore
#endif

