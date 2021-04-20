//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>A collection of resources for multiple Unity editor themes.</para>
	/// </summary>
	/// <typeparam name="T">The type of the object inheriting from this.</typeparam>
	internal abstract class ThemedResourceCollection<T> : ScriptableObject
		where T : ThemedResourceCollection<T>
	{
		private static T lightTheme;
		private static T darkTheme;

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Light theme.</para>
		/// </summary>
		public static T LightTheme
		{
			get
			{
				if (lightTheme == null)
				{
					string path = CreateInstance<T>().LightThemePath;
					lightTheme = LoadTheme(path);
				}

				return lightTheme;
			}
		}

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for usage with the Dark theme.</para>
		/// </summary>
		public static T DarkTheme
		{
			get
			{
				if (darkTheme == null)
				{
					string path = CreateInstance<T>().DarkThemePath;
					darkTheme = LoadTheme(path);
				}

				return darkTheme;
			}
		}

		/// <summary>
		/// <para>An instance of this <see cref="ScriptableObject"/> for the current theme.</para>
		/// </summary>
		public static T CurrentTheme
		{
			get
			{
				if (EditorGUIUtility.isProSkin)
				{
					return DarkTheme;
				}
				else
				{
					return LightTheme;
				}
			}
		}

		/// <summary>
		/// <para>A path in Unity's resources to the singleton instance of this <see cref="ScriptableObject"/> for the light theme.</para>
		/// </summary>
		public abstract string LightThemePath { get; }

		/// <summary>
		/// <para>A path in Unity's resources to the singleton instance of this <see cref="ScriptableObject"/> for the dark theme.</para>
		/// </summary>
		public abstract string DarkThemePath { get; }

		private static T LoadTheme(string path)
		{
			var loadedResouce = Resources.Load<T>(path);

			if (loadedResouce == null)
			{
				Debug.LogError("No \"" + typeof(T).Name + "\" at the path \"" + path + "\" found in the Resources directories");
			}

			return loadedResouce;
		}
	}
}

#pragma warning restore
#endif
