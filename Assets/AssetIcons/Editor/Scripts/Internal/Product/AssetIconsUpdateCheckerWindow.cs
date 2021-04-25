//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#endif

namespace AssetIcons.Editors.Internal.Product
{
	internal class AssetIconsUpdateCheckerWindow : EditorWindow
	{
		private static GUIStyle subtitleText;
		private static GUIStyle titleText;
		private static GUIStyle headerStyle;
		private static GUIStyle checkingDialogStyle;

		private float currentVersionLastProgress;
#if UNITY_2017_2_OR_NEWER
		private UnityWebRequestAsyncOperation statusOperation;
#else
		private AsyncOperation statusOperation;
#endif
		private UnityWebRequest statusRequest;

		[NonSerialized]
		private ProductStatusModel statusModel;

		[NonSerialized]
		private bool isComplete;

		[NonSerialized]
		private string errorMessage;

		public static void Open()
		{
			var window = GetWindow<AssetIconsUpdateCheckerWindow>();

			window.ShowUtility();

			window.minSize = new Vector2(350, 300);
			// window.maxSize = new Vector2(350, 500);

			window.position = new Rect(450,
				300, 350, 500);

			window.titleContent = new GUIContent("AssetIcons",
				AssetIconsResources.CurrentTheme.UpdaterIcon);
		}

		private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
		{
			var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dateTime;
		}

		private static void DrawHistory(ProductStatusModel statusModel)
		{
			if (statusModel.VersionHistory == null || statusModel.VersionHistory.Length == 0)
			{
				return;
			}

			bool foundCurrent = false;

			var installedVersion = new System.Version(ProductInformation.Version);

			for (int i = statusModel.VersionHistory.Length - 1; i >= 0; i--)
			{
				var current = statusModel.VersionHistory[i];
				var currentVersion = new System.Version(current.Version);
				bool currentIsFutureVersion = installedVersion >= currentVersion;

				bool lastIsFutureVersion = false;
				if (i != statusModel.VersionHistory.Length - 1)
				{
					var last = statusModel.VersionHistory[i + 1];

					var lastVersion = new System.Version(last.Version);
					lastIsFutureVersion = installedVersion >= lastVersion;
				}

				bool isMajor = false;
				if (i != 0)
				{
					var next = statusModel.VersionHistory[i - 1];

					var nextVersion = new System.Version(next.Version);

					if (nextVersion.Major < currentVersion.Major)
					{
						isMajor = true;
					}
				}
				else
				{
					isMajor = true;
				}

				var rect = GUILayoutUtility.GetRect(0, 1000, 38, 38);

				var indetnedRect = new Rect(rect)
				{
					xMin = rect.xMin + 18,
					xMax = rect.xMax - 6,
					yMin = rect.yMin + 6,
					yMax = rect.yMax - 6
				};

				var iconRect = new Rect(indetnedRect)
				{
					xMax = indetnedRect.xMin + indetnedRect.height
				};
				var iconTopLineRect = new Rect(iconRect)
				{
					xMin = iconRect.xMin + (iconRect.width * 0.5f) - 3.0f,
					yMax = rect.yMax,
					yMin = rect.center.y
				};
				iconTopLineRect.xMax = iconTopLineRect.xMin + 6.0f;

				var iconBottomLineRect = new Rect(iconRect)
				{
					xMin = iconRect.xMin + (iconRect.width * 0.5f) - 3.0f,
					yMin = rect.yMin,
					yMax = rect.center.y
				};
				iconBottomLineRect.xMax = iconBottomLineRect.xMin + 6.0f;


				var informationRect = new Rect(indetnedRect)
				{
					xMin = iconRect.xMax + 6
				};

				var originalColor = GUI.color;

				if (i != statusModel.VersionHistory.Length - 1)
				{
					if (!lastIsFutureVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.FutureVersionLineColor;
					}
					else
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.NormalVersionLineColor;
					}
					EditorGUI.DrawRect(iconBottomLineRect, Color.white);
				}
				if (i != 0)
				{
					if (!currentIsFutureVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.FutureVersionLineColor;
					}
					else
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.NormalVersionLineColor;
					}
					EditorGUI.DrawRect(iconTopLineRect, Color.white);
				}

				bool currentIsCurrentVersion = false;
				if (currentIsFutureVersion)
				{
					if (!foundCurrent)
					{
						currentIsCurrentVersion = true;
						foundCurrent = true;
					}

					GUI.color = originalColor * AssetIconsResources.CurrentTheme.NormalVersionColor;
				}
				else
				{
					GUI.color = originalColor * AssetIconsResources.CurrentTheme.FutureVersionColor;
				}

				if (!isMajor)
				{
					iconRect = new Rect(iconRect)
					{
						xMin = iconRect.xMin + 4,
						xMax = iconRect.xMax - 4,
						yMin = iconRect.yMin + 4,
						yMax = iconRect.yMax - 4
					};
				}
				GUI.DrawTexture(iconRect, AssetIconsResources.CurrentTheme.CircleTexture);

				GUI.color = originalColor;
				if (currentIsCurrentVersion)
				{
					EditorGUI.LabelField(informationRect, current.Version + " - Installed", titleText);
				}
				else
				{
					EditorGUI.LabelField(informationRect, current.Version, titleText);
				}

				var releaseDate = UnixTimeStampToDateTime(current.Timestamp);
				EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

				if (rect.Contains(Event.current.mousePosition))
				{
					if (Event.current.type == EventType.MouseDown)
					{
						if (!string.IsNullOrEmpty(statusModel.WebChangelogSource))
						{
							Application.OpenURL(string.Format(statusModel.WebChangelogSource, current.Version, "en"));
						}
					}

					if (currentIsCurrentVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.CurrentVersionBackgroundHoverColor;
					}
					else if (currentIsFutureVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.NormalVersionBackgroundHoverColor;
					}
					else
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.FutureVersionBackgroundHoverColor;
					}
				}
				else
				{
					if (currentIsCurrentVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.CurrentVersionBackgroundColor;
					}
					else if (currentIsFutureVersion)
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.NormalVersionBackgroundColor;
					}
					else
					{
						GUI.color = originalColor * AssetIconsResources.CurrentTheme.FutureVersionBackgroundColor;
					}
				}

				EditorGUI.DrawRect(rect, Color.white);

				GUI.color = originalColor;

				EditorGUI.LabelField(informationRect, "<b>changelog</b> - released " + releaseDate.ToString("Y"),
					subtitleText);
			}
		}

		private void OnGUI()
		{
			wantsMouseMove = true;
#if UNITY_5_6_OR_NEWER
			wantsMouseEnterLeaveWindow = true;
			if (Event.current.type == EventType.MouseEnterWindow
				|| Event.current.type == EventType.MouseLeaveWindow)
			{
				Repaint();
				return;
			}
#endif

			if (Event.current.type == EventType.MouseMove
				|| Event.current.type == EventType.MouseDrag)
			{
				Repaint();
				return;
			}

			if (subtitleText == null)
			{
				subtitleText = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
				{
#if UNITY_2019_1_OR_NEWER
					alignment = TextAnchor.MiddleLeft,
#else
					alignment = TextAnchor.MiddleLeft,
#endif
					fontSize = 8,
					richText = true,
					contentOffset = new Vector2(0, 9.0f),
					padding = new RectOffset(4, 4, 0, 0)
				};
			}
			if (titleText == null)
			{
				titleText = new GUIStyle(EditorStyles.label)
				{
					alignment = TextAnchor.UpperLeft,
					padding = new RectOffset(4, 4, 0, 0)
				};
			}
			if (headerStyle == null)
			{
				headerStyle = new GUIStyle()
				{
					padding = new RectOffset(28, 28, 16, 8)
				};
			}
			if (checkingDialogStyle == null)
			{
				checkingDialogStyle = new GUIStyle()
				{
					padding = new RectOffset(28, 28, 16, 8)
				};
			}

			using (new EditorGUILayout.VerticalScope(headerStyle))
			{
				EditorGUILayout.LabelField("AssetIcons", EditorStyles.largeLabel);

				EditorGUILayout.LabelField("Current Version " + ProductInformation.Version);
			}

			// If a download hasn't started, start a download.
			if (statusRequest == null)
			{
				statusRequest = UnityWebRequest.Get("https://fydar.github.io/AssetIcons/version/status.json");

#if UNITY_2017_2_OR_NEWER
				statusOperation = statusRequest.SendWebRequest();
#else
				statusOperation = statusRequest.Send();
#endif
			}

			// If the request is down and we haven't tried deserializing it, deserialize it
			if (statusRequest.isDone && statusModel == null && Event.current.type == EventType.Layout)
			{
				isComplete = true;

#if !UNITY_2017_1_OR_NEWER
				if (statusRequest.isError)
				{
					errorMessage = statusRequest.error;
				}
#else
				if (statusRequest.isHttpError)
				{
					errorMessage = "Got a bad response of " + statusRequest.responseCode + ".";
				}
				else if (statusRequest.isNetworkError)
				{
					errorMessage = statusRequest.error;
				}
#endif
				else
				{
					// ErrorMessage
					string text = statusRequest.downloadHandler.text;
					try
					{
						statusModel = JsonUtility.FromJson<ProductStatusModel>(text);
						errorMessage = null;
					}
					catch (Exception exception)
					{
						Debug.LogError(string.Format("Got exception\"{0}\" when deserializing the network response.\n{1}",
							exception.GetType().Name, exception.ToString()));

						errorMessage = "Failed to deserialize response";
					}
				}
			}

			if (statusModel != null)
			{
				DrawHistory(statusModel);
			}
			else
			{
				using (new EditorGUILayout.VerticalScope(headerStyle))
				{
					if (isComplete)
					{
						EditorGUILayout.LabelField("Update Check Failed", EditorStyles.centeredGreyMiniLabel);

						EditorGUILayout.HelpBox(errorMessage, MessageType.Error);

						if (GUILayout.Button("Retry"))
						{
							statusRequest = null;
							errorMessage = null;
							isComplete = false;
							statusModel = null;
						}
					}
					else
					{
						EditorGUILayout.LabelField("Checking for Updates", EditorStyles.centeredGreyMiniLabel);

						var rect = GUILayoutUtility.GetRect(0, float.MaxValue, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

						EditorGUI.ProgressBar(rect, statusOperation.progress, "Downloading...");
					}
				}
			}
		}

		private void Update()
		{
			if (statusOperation != null
				&& statusOperation.progress != currentVersionLastProgress)
			{
				currentVersionLastProgress = statusOperation.progress;
				Repaint();
			}
		}
	}
}

#pragma warning restore
#endif
