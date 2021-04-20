//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>A utility script that attempts to open the AssetIcons "<c>Documentation.html</c>" file in a browser rather than a code editor.</para>
	/// </summary>
	internal static class AssetIconsDocumentationOpener
	{
		[OnOpenAsset(1)]
		private static bool Select(int instanceID, int line)
		{
			string path = AssetDatabase.GetAssetPath(instanceID);
			if (path.EndsWith("/AssetIcons/Documentation.html"))
			{
				string fullPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + path;

				Application.OpenURL(fullPath);
				return true;
			}

			return false;
		}
	}
}

#pragma warning restore
#endif
