//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

/*
#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using UnityEditor;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>Adds controls to the Unity menu for interfacing with AssetIcons to draw them.</para>
	/// </summary>
	[InitializeOnLoad]
	internal static class AssetIconsMenu
	{
		private const string TEXT_ENABLED = "Enabled";
		private const string MENU_LOCATION = "Assets/AssetIcons";
		private const int MENU_PRIORITY = 2100000000;

		/// <summary>
		/// <para>Toggles the editor drawing of the asset icons. Used to disable AssetIcons.</para>
		/// </summary>
		[MenuItem(MENU_LOCATION + "/" + TEXT_ENABLED, false, MENU_PRIORITY)]
		public static void ToggleEnabled()
		{
			AssetIconsPreferences.Enabled.Value = !AssetIconsPreferences.Enabled.Value;
		}

		[MenuItem(MENU_LOCATION + "/" + TEXT_ENABLED, true)]
		private static bool ToggleEnabledValidate()
		{
			Menu.SetChecked(MENU_LOCATION + "/" + TEXT_ENABLED, AssetIconsPreferences.Enabled.Value);

			return true;
		}
	}
}

#pragma warning restore
#endif
*/