//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Preferences;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

#if UNITY_2018_3_OR_NEWER
using System.Collections.Generic;
#endif

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>Integrates support for Unity's preferences menu.</para>
	/// </summary>
	internal static class AssetIconsPreferencesMenu
	{
		private struct GitHubSectionContent
		{
			public string Header { get; set; }
			public string Body { get; set; }
			public Action ClickAction { get; set; }
		}

		private const float starIndent = 4.0f;

		private static readonly string versionString = "Version " + ProductInformation.Version;
		private static readonly string[] propertyStrings = new string[]
		{
			"enabled",
			"drawGUIStyles",
			"prefabResolution",
			"selectionTint",
			"typeIcons"
		};

		private static GUIStyle pageBodyStyle;
		private static GUIStyle pageHeaderStyle;
		private static GUIStyle pageTextStyle;
		private static GUIStyle reviewBoxStyle;
		private static GUIStyle githubSectionBlock;
		private static GUIStyle linkLable;
		private static int hoveredReviewStar = -1;
		private static int hoveredGithubSection = -1;
		private static EditorWindow preferencesEditorWindow;
		private static SerializedObject serializedPreferences;

		private static SerializedObject SerializedPreferences
		{
			get
			{
				if (serializedPreferences == null
					|| serializedPreferences.targetObject == null
					|| serializedPreferences.targetObject.Equals(null)
					|| AssetIconsPreferences.CurrentPreferences != serializedPreferences.targetObject)
				{
					serializedPreferences = new SerializedObject(AssetIconsPreferences.CurrentPreferences);
				}
				return serializedPreferences;
			}
		}

		private static readonly GitHubSectionContent[] githubSections = new GitHubSectionContent[]
		{
			new GitHubSectionContent()
			{
				Header = "Need a hand?",
				Body = "Ask your questions here, I typically respond quite quickly.",
				ClickAction = () => EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Question)
			},
			new GitHubSectionContent()
			{
				Header = "Request a Feature",
				Body = "Feel free to request any features!",
				ClickAction = () => EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Feature)
			},
			new GitHubSectionContent()
			{
				Header = "Found a bug?",
				Body = "Report from here on the GitHub issue tracker!",
				ClickAction = () => EditorProductInformation.SubmitIssue(EditorProductInformation.IssueType.Bug)
			},
			new GitHubSectionContent()
			{
				Header = "Documentation and User Guides",
				Body = "Check out the public APIs and read the user guides.",
				ClickAction = () => Application.OpenURL("https://fydar.github.io/AssetIcons/Documentation")
			}
		};

		private static void DrawPreferences(SerializedObject preferences)
		{
			if (linkLable == null)
			{
				linkLable = new GUIStyle(EditorStyles.label);
				linkLable.normal.textColor = new Color(0.0f, 0.0f, 1.0f);
				linkLable.hover.textColor = new Color(0.0f, 0.0f, 1.0f);
				linkLable.active.textColor = new Color(0.5f, 0.0f, 0.5f);

				pageBodyStyle = new GUIStyle
				{
#if UNITY_2019_1_OR_NEWER
					padding = new RectOffset(12, 12, 0, 0)
#else
					padding = new RectOffset(0, 4, 0, 0)
#endif
				};

				pageTextStyle = new GUIStyle(EditorStyles.label)
				{
					wordWrap = true
				};

				reviewBoxStyle = new GUIStyle(EditorStyles.helpBox)
				{
					padding = new RectOffset(8, 8, 8, 8)
				};
				int fontSize = EditorStyles.label.fontSize;
				if (fontSize <= 2)
				{
					fontSize = 10;
				}
				pageHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
				{
					fontSize = fontSize + 2,
					normal = new GUIStyleState()
					{
						textColor = EditorStyles.boldLabel.normal.textColor * new Color(1.0f, 1.0f, 1.0f, 0.8f)
					}
				};

				githubSectionBlock = new GUIStyle()
				{
					padding = new RectOffset(6, 6, 5, 5)
				};
			}

			if (preferencesEditorWindow == null)
			{
				string preferencesMenu;
#if UNITY_2019_1_OR_NEWER
				preferencesMenu = "PreferenceSettingsWindow";
#else
				preferencesMenu = "PreferencesWindow";
#endif
				if (EditorWindow.focusedWindow != null
					&& EditorWindow.focusedWindow.GetType().Name == preferencesMenu)
				{
					preferencesEditorWindow = EditorWindow.focusedWindow;
				}
			}
			if (preferencesEditorWindow != null)
			{
				preferencesEditorWindow.wantsMouseMove = true;
			}

			EditorGUILayout.BeginVertical(pageBodyStyle);

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(versionString);

				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Issue Tracker", linkLable))
				{
					Application.OpenURL(ProductInformation.IssueTracker);
				}
				var rect = GUILayoutUtility.GetLastRect();
				EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
			}

			GUILayout.Space(10.0f);

			if (GUILayout.Button("Check for Updates"))
			{
				AssetIconsUpdateCheckerWindow.Open();
			}

			float labelWidth = EditorGUIUtility.labelWidth;
#if !UNITY_2019_1_OR_NEWER
			EditorGUIUtility.labelWidth -= 60.0f;
#endif

			GUILayout.Space(10.0f);
			DrawConfigurationSection(preferences);
			GUILayout.Space(10.0f);
			DrawUnityAssetStoreSection();
			GUILayout.Space(10.0f);
			DrawReviewArea();
			GUILayout.Space(14.0f);
			DrawGithubSection();
			GUILayout.Space(10.0f);

			EditorGUIUtility.labelWidth = labelWidth;

			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField(ProductInformation.Copyright, EditorStyles.centeredGreyMiniLabel);

			EditorGUILayout.EndVertical();
		}

		private static void DrawConfigurationSection(SerializedObject preferences)
		{
			GUILayout.Label("Configuration", pageHeaderStyle);
			GUILayout.Space(6.0f);

			preferences.Update();

			EditorGUI.BeginChangeCheck();

			foreach (string propertyString in propertyStrings)
			{
				var property = preferences.FindProperty(propertyString);
				EditorGUILayout.PropertyField(property, new GUIContent(property.displayName, property.tooltip));
			}

			if (EditorGUI.EndChangeCheck())
			{
				preferences.ApplyModifiedProperties();

				EditorPrefs.SetString(AssetIconsPreferences.EditorPrefsKey,
					JsonUtility.ToJson(AssetIconsPreferences.CurrentPreferences));
			}
		}

		private static void DrawUnityAssetStoreSection()
		{
			var logoToDraw = AssetIconsResources.CurrentTheme.UnityLogo;
			var logoRect = GUILayoutUtility.GetRect(logoToDraw.width * 0.5f, logoToDraw.height * 0.5f,
				GUILayout.ExpandWidth(false), GUILayout.MinWidth(logoToDraw.width * 0.5f));
			var originalColor = GUI.color;
			GUI.color = AssetIconsResources.CurrentTheme.LogoColor;
			GUI.DrawTexture(logoRect, logoToDraw);
			GUI.color = originalColor;
			EditorGUIUtility.AddCursorRect(logoRect, MouseCursor.Link);
			if (Event.current.type == EventType.MouseDown)
			{
				if (logoRect.Contains(Event.current.mousePosition))
				{
					Application.OpenURL(ProductInformation.StorePageUrl);
					Event.current.Use();
				}
			}
		}

		private static void DrawReviewArea()
		{
			if (AssetIconsPreferences.HideReviewDialog.Value)
			{
				return;
			}

			using (new EditorGUILayout.VerticalScope(reviewBoxStyle, GUILayout.MaxWidth(420.0f)))
			{
				EditorGUILayout.LabelField("Write a Review", pageHeaderStyle);

				var headerRect = GUILayoutUtility.GetLastRect();
				headerRect.xMin = headerRect.xMax - headerRect.height;
				headerRect = new Rect(headerRect.x + 8.0f, headerRect.y + 2.0f,
					headerRect.width - 8.0f, headerRect.height - 8.0f);

				var originalColor = GUI.color;
				GUI.color *= AssetIconsResources.CurrentTheme.CrossNormalColour;
				GUI.DrawTexture(headerRect, AssetIconsResources.CurrentTheme.CrossIcon);
				GUI.color = originalColor;

				if (Event.current.type == EventType.MouseDown
					&& headerRect.Contains(Event.current.mousePosition))
				{
					AssetIconsPreferences.HideReviewDialog.Value = true;
				}
				EditorGUIUtility.AddCursorRect(headerRect, MouseCursor.Link);

				GUILayout.Space(6.0f);

				EditorGUILayout.BeginHorizontal();
				var starsBackground = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.MinWidth(200.0f));

				originalColor = GUI.color;
				GUI.color *= new Color(1.0f, 1.0f, 1.0f, 0.35f);
				GUI.DrawTexture(starsBackground, AssetIconsResources.CurrentTheme.StarBackground);
				GUI.color = originalColor;

				EditorGUIUtility.AddCursorRect(starsBackground, MouseCursor.Link);

				GUILayout.Space(2.0f);

				for (int i = 0; i < 5; i++)
				{
					var rect = GUILayoutUtility.GetRect(40.0f, 40.0f, GUILayout.ExpandWidth(false));
					var indentedRect = new Rect(rect.x + starIndent, rect.y + starIndent,
						rect.width - (starIndent * 2), rect.height - (starIndent * 2));

					if (hoveredReviewStar >= i)
					{
						GUI.color = AssetIconsResources.CurrentTheme.StarHighlightedColor;
					}
					else
					{
						GUI.color = AssetIconsResources.CurrentTheme.StarNormalColor;
					}
					GUI.DrawTexture(indentedRect, AssetIconsResources.CurrentTheme.StarIcon);

					if (rect.Contains(Event.current.mousePosition))
					{
						if (hoveredReviewStar != i)
						{
							hoveredReviewStar = i;
							if (preferencesEditorWindow != null)
							{
								preferencesEditorWindow.Repaint();
							}
						}

						if (Event.current.type == EventType.MouseDown)
						{
							EditorProductInformation.WriteReview(hoveredReviewStar);
							AssetIconsPreferences.HideReviewDialog.Value = true;
						}
					}
				}

				GUI.color = originalColor;

				GUILayout.Space(2.0f);
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(10.0f);
				var reactionRect = GUILayoutUtility.GetRect(40.0f, 40.0f, GUILayout.ExpandWidth(false));

				if (Event.current.type != EventType.Layout)
				{
					if (starsBackground.Contains(Event.current.mousePosition))
					{
						if (hoveredReviewStar != -1)
						{
							originalColor = GUI.color;
							GUI.color *= new Color(1.0f, 1.0f, 1.0f, 0.55f);
							GUI.DrawTexture(reactionRect, AssetIconsResources.CurrentTheme.EmojisBackground);
							GUI.color = originalColor;

							var indentedRectionRect = new Rect(reactionRect.x + starIndent, reactionRect.y + starIndent,
								reactionRect.width - (starIndent * 2), reactionRect.height - (starIndent * 2));

							GUI.DrawTexture(indentedRectionRect, AssetIconsResources.CurrentTheme.EmojisForRatings[hoveredReviewStar]);
						}
					}
					else
					{
						if (hoveredReviewStar != -1)
						{
							hoveredReviewStar = -1;
							if (preferencesEditorWindow != null)
							{
								preferencesEditorWindow.Repaint();
							}
						}
					}
				}

				EditorGUILayout.EndHorizontal();
				GUILayout.Space(6.0f);

				EditorGUILayout.LabelField("It’s great to hear all of your wonderful feedback. If you have a moment, feel free to write a review.", pageTextStyle);
				GUILayout.Space(2.0f);
			}
		}

		private static void DrawGithubSection()
		{
			var logoToDraw = AssetIconsResources.CurrentTheme.GithubLogo;
			var logoRect = GUILayoutUtility.GetRect(logoToDraw.width * 0.5f, logoToDraw.height * 0.5f,
				GUILayout.ExpandWidth(false), GUILayout.MinWidth(logoToDraw.width * 0.5f));
			var originalColor = GUI.color;
			GUI.color = AssetIconsResources.CurrentTheme.LogoColor;
			GUI.DrawTexture(logoRect, logoToDraw);
			GUI.color = originalColor;
			EditorGUIUtility.AddCursorRect(logoRect, MouseCursor.Link);
			if (Event.current.type == EventType.MouseDown)
			{
				if (logoRect.Contains(Event.current.mousePosition))
				{
					Application.OpenURL(ProductInformation.GitHubUrl);
					Event.current.Use();
				}
			}

			GUILayout.Space(10.0f);

			bool hoveredAny = false;
			for (int i = 0; i < githubSections.Length; i++)
			{
				var section = githubSections[i];
				var backgroundRect = EditorGUILayout.BeginVertical(githubSectionBlock, GUILayout.MinWidth(340.0f));
				if (backgroundRect.Contains(Event.current.mousePosition))
				{
					hoveredAny = true;
					if (hoveredGithubSection != i)
					{
						hoveredGithubSection = i;
						if (preferencesEditorWindow != null)
						{
							preferencesEditorWindow.Repaint();
						}
					}
					EditorGUI.DrawRect(backgroundRect, new Color(1.0f, 1.0f, 1.0f, 0.2f));
					if (Event.current.type == EventType.MouseDown)
					{
						if (section.ClickAction != null)
						{
							section.ClickAction();
						}
					}
				}
				EditorGUIUtility.AddCursorRect(backgroundRect, MouseCursor.Link);
				EditorGUILayout.LabelField(section.Header, pageHeaderStyle);
				EditorGUILayout.LabelField(section.Body, pageTextStyle);
				EditorGUILayout.EndVertical();
			}
			if (!hoveredAny && hoveredGithubSection != -1)
			{
				hoveredGithubSection = -1;
				if (preferencesEditorWindow != null)
				{
					preferencesEditorWindow.Repaint();
				}
			}
		}

#if UNITY_2018_3_OR_NEWER

		/// <summary>
		/// <para>Implements <see cref="SettingsProvider"/>.</para>
		/// </summary>
		/// <returns>
		/// <para>A settings provider configured to draw AssetIcons preferences.</para>
		/// </returns>
		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new SettingsProvider("Preferences/AssetIcons", SettingsScope.User)
			{
				label = "AssetIcons",
				guiHandler = (searchContext) =>
				{
					DrawPreferences(SerializedPreferences);
				},
				keywords = new HashSet<string>(new[] { "AssetIcons", "Icons" })
			};

			return provider;
		}

#else

		/// <summary>
		/// <para>Implements <see cref="PreferenceItem"/>.</para>
		/// </summary>
		[PreferenceItem("AssetIcons")]
		public static void DrawPreferencesItem()
		{
			DrawPreferences(SerializedPreferences);
		}

#endif
	}
}

#pragma warning restore
#endif
