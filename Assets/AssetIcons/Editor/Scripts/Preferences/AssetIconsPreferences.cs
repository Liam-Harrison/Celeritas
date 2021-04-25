//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>Stores AssetIcons preferences information in <see cref="EditorPrefs"/>.</para>
	/// </summary>
	/// <example>
	/// <para>You can interact with AssetIcons preferences via an editor scripting API.</para>
	/// <code>
	/// using AssetIcons.Editors.Preferences;
	/// using UnityEditor;
	/// 
	/// public class DemoWindow : EditorWindow
	/// {
	/// 	[MenuItem("AssetIcons/Demo Window")]
	/// 	private static void Init()
	/// 	{
	/// 		var window = GetWindow(typeof(DemoWindow), false, "Demo Window");
	/// 		window.Show();
	/// 	}
	/// 
	/// 	private void OnGUI()
	/// 	{
	/// 		AssetIconsPreferences.Enabled.Value = EditorGUILayout.Toggle("AssetIcons Enabled", AssetIconsPreferences.Enabled.Value);
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <remarks>
	/// <para>These preferences are effective on a single machine and in a single Unity project. Changing to another project with AssetIcons 
	/// installed will not preserve these saved preferences.</para>
	/// </remarks>
	/// <seealso cref="AssetIconsPreferencesPreset"/>
	public static class AssetIconsPreferences
	{
		/// <summary>
		/// <para>The <see cref="EditorPrefs"/> key used to store the current users preferences.</para>
		/// </summary>
		public const string EditorPrefsKey = "AssetIcons_Preferences";

		private static AssetIconsPreferencesPreset currentPreferences;

		/// <summary>
		/// <para>The current set of preferences used by AssetIcons.</para>
		/// </summary>
		/// <example>
		/// <para>You can interact with AssetIcons preferences via an editor scripting API.</para>
		/// <code>
		/// using AssetIcons.Editors.Preferences;
		/// using UnityEditor;
		/// 
		/// public class DemoWindow : EditorWindow
		/// {
		/// 	[MenuItem("AssetIcons/Demo Window")]
		/// 	private static void Init()
		/// 	{
		/// 		var window = GetWindow(typeof(DemoWindow), false, "Demo Window");
		/// 		window.Show();
		/// 	}
		/// 
		/// 	private void OnGUI()
		/// 	{
		/// 		var preferences = AssetIconsPreferences.CurrentPreferences;
		/// 
		/// 		preferences.Enabled.Value = EditorGUILayout.Toggle("AssetIcons Enabled", preferences.Enabled.Value);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static AssetIconsPreferencesPreset CurrentPreferences
		{
			get
			{
				if (currentPreferences == null)
				{
					string jsonData = EditorPrefs.GetString(EditorPrefsKey, "");

					if (string.IsNullOrEmpty(jsonData))
					{
						currentPreferences = ScriptableObject.CreateInstance<AssetIconsPreferencesPreset>();
					}
					else
					{
						currentPreferences = ScriptableObject.CreateInstance<AssetIconsPreferencesPreset>();
						JsonUtility.FromJsonOverwrite(jsonData, currentPreferences);
					}

					currentPreferences.Enabled.OnChanged += SaveChangesCallback;
					currentPreferences.DrawGUIStyles.OnChanged += SaveChangesCallback;
					currentPreferences.PrefabResolution.OnChanged += ClearCacheCallback;
					currentPreferences.SelectionTint.OnChanged += SaveChangesCallback;
					currentPreferences.TypeIcons.OnChanged += SaveChangesCallback;
				}
				return currentPreferences;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.DrawGUIStyles</c>.</para>
		/// <para>Enables previewing of Unity <see cref="GUIStyle"/> assets.</para>
		/// </summary>
		public static BoolEventField DrawGUIStyles
		{
			get
			{
				return CurrentPreferences.DrawGUIStyles;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.Enabled</c>.</para>
		/// <para>Controls whether AssetIcons should be enabled or disabled.</para>
		/// </summary>
		public static BoolEventField Enabled
		{
			get
			{
				return CurrentPreferences.Enabled;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.PrefabResolution</c>.</para>
		/// <para>Allows for adjusting the resolution AssetIcons will render Prefabs with.</para>
		/// </summary>
		/// <remarks>
		/// <para>If you are struggling with performance with a large amount of rendered assets, you could try
		/// adjusting this to boost performance.</para>
		/// </remarks>
		public static IntEventField PrefabResolution
		{
			get
			{
				return CurrentPreferences.PrefabResolution;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.SelectionTint</c>.</para>
		/// <para>Controls how strong of a tint is applied to AssetIcons rendered graphics when selected.</para>
		/// </summary>
		public static ColorTintEventField SelectionTint
		{
			get
			{
				return CurrentPreferences.SelectionTint;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.TypeIcons</c>.</para>
		/// <para>A collection of Graphics associated with file extensions that AssetIcons uses to render custom
		/// file icons.</para>
		/// </summary>
		public static IconMapping TypeIcons
		{
			get
			{
				return CurrentPreferences.TypeIcons;
			}
		}

		/// <summary>
		/// <para>A shorthand for <c>CurrentPreferences.HideReviewDialog</c>.</para>
		/// <para>Controls whether the review dialog should be shown.</para>
		/// </summary>
		public static BoolEventField HideReviewDialog
		{
			get
			{
				return CurrentPreferences.HideReviewDialog;
			}
		}

		private static void ClearCacheCallback()
		{
			AssetIconsRenderCache.ClearCache();
			SaveChangesCallback();
		}

		private static void SaveChangesCallback()
		{
			EditorApplication.delayCall -= SaveAndRepaint;
			EditorApplication.delayCall += SaveAndRepaint;
		}

		private static void SaveAndRepaint()
		{
			EditorApplication.RepaintProjectWindow();
			EditorPrefs.SetString(EditorPrefsKey, JsonUtility.ToJson(CurrentPreferences));
		}
	}
}

#pragma warning restore
#endif
