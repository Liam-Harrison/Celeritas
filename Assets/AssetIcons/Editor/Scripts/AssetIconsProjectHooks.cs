//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>AssetIcons uses this to manage the rendering callbacks.</para>
	/// </summary>
	public static class AssetIconsProjectHooks
	{
		/// <summary>
		/// <para>Delgate used by AssetIcons to draw an icon.</para>
		/// </summary>
		/// <param name="guid">The GUID of the asset to be drawn.</param>
		/// <param name="area">The target area to draw the assets icon.</param>
		public delegate void DrawAssetIconCallback(string guid, Rect area);

		private delegate void ObjectListDelegate(Rect area, string guid, bool isSmall);
		private delegate void AssetTreeViewDelegate(Rect area, string GUID);

		/// <summary>
		/// <para>The callback that AssetIcons uses to draw draw icons.</para>
		/// </summary>
		internal static event DrawAssetIconCallback OnInternalDrawIcon;

		/// <summary>
		/// <para>The callback that AssetIcons uses to draw draw icons.</para>
		/// </summary>
		public static event DrawAssetIconCallback OnDrawIcon;

		static AssetIconsProjectHooks()
		{
			var editorAssembly = Assembly.GetAssembly(typeof(SerializedProperty));

#if UNITY_2017_1_OR_NEWER
			try
			{
				var objectListAreaType = editorAssembly.GetType("UnityEditor.ObjectListArea");

				var hookEvent = objectListAreaType.GetEvent("postAssetIconDrawCallback", BindingFlags.Static | BindingFlags.NonPublic);
				var hookField = objectListAreaType.GetField("postAssetIconDrawCallback", BindingFlags.Static | BindingFlags.NonPublic);
				var hookDelegateType = objectListAreaType.GetNestedType("OnAssetIconDrawDelegate", BindingFlags.NonPublic);

				SubscribeAsFirst(hookEvent, hookField, (ObjectListDelegate)ObjectListAreaCallback, hookDelegateType);
			}
			catch (Exception exception)
			{
				Debug.LogWarning("AssetIcons is using fallback method of rendering due to incompatiblity with this version of Unity. Internal Exception: " + exception);
				EditorApplication.projectWindowItemOnGUI += ItemOnGUI;
			}

			try
			{
				var assetsTreeViewGUIType = editorAssembly.GetType("UnityEditor.AssetsTreeViewGUI");

				var hookEvent = assetsTreeViewGUIType.GetEvent("postAssetIconDrawCallback", BindingFlags.Static | BindingFlags.NonPublic);
				var hookField = assetsTreeViewGUIType.GetField("postAssetIconDrawCallback", BindingFlags.Static | BindingFlags.NonPublic);
				var hookDelegateType = assetsTreeViewGUIType.GetNestedType("OnAssetIconDrawDelegate", BindingFlags.NonPublic);

				SubscribeAsFirst(hookEvent, hookField, (AssetTreeViewDelegate)AssetTreeViewCallback, hookDelegateType);
			}
			catch
			{
			}

#else
			EditorApplication.projectWindowItemOnGUI += ItemOnGUI;
#endif
		}

		private static void SubscribeAsFirst(EventInfo hookEvent, FieldInfo hookField, Delegate callback, Type delegateType)
		{
			var hookAddMethod = hookEvent.GetAddMethod(true);

			// Collect all previous subscribers to the delegate
			Delegate[] resubTargsList = null;
			var del = (Delegate)hookField.GetValue(null);
			if (del != null)
			{
				resubTargsList = del.GetInvocationList();
			}

			// Remove all previous subscribers to the delegate
			hookField.SetValue(null, null);

			// Add AssetIcons handler to the delegate now that it's empty. This should make
			// AssetIcons render below all other handlers.
			hookAddMethod.Invoke(null, new object[1] { Cast(callback, delegateType) });

			// Resubscribe all of the delegates that I had previously removed.
			if (resubTargsList != null)
			{
				for (int i = 0; i < resubTargsList.Length; i++)
				{
					var resubTarg = resubTargsList[i];
					hookAddMethod.Invoke(null, new object[1] { resubTarg });
				}
			}
		}

		private static void AssetTreeViewCallback(Rect rect, string guid)
		{
			ItemOnGUI(guid, rect);
		}

		private static void ItemOnGUI(string guid, Rect rect)
		{
			if (OnInternalDrawIcon != null)
			{
				OnInternalDrawIcon(guid, rect);
			}

			if (OnDrawIcon != null)
			{
				OnDrawIcon(guid, rect);
			}
		}

#if UNITY_2017_1_OR_NEWER

		private static void ObjectListAreaCallback(Rect rect, string guid, bool isSmall)
		{
			ItemOnGUI(guid, rect);
		}

#endif

		private static Delegate Cast(Delegate source, Type type)
		{
			if (source == null)
			{
				return null;
			}

			var delegates = source.GetInvocationList();

			if (delegates.Length == 1)
			{
				return Delegate.CreateDelegate(type, delegates[0].Target, delegates[0].Method);
			}

			var delegatesDest = new Delegate[delegates.Length];

			for (int i = 0; i < delegates.Length; i++)
			{
				delegatesDest[i] = Delegate.CreateDelegate(type, delegates[i].Target,
					delegates[i].Method);
			}

			return Delegate.Combine(delegatesDest);
		}
	}
}

#pragma warning restore
#endif
