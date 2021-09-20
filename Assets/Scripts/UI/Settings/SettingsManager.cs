using Celeritas.Game;
using System;
using UnityEngine;

namespace Celeritas.UI
{
	public enum SettingKey
	{
		Width,
		Height,
		RefreshRate,
		FullscreenMode,
	}

	public class SettingsManager : Singleton<SettingsManager>
	{
		protected override void Awake()
		{
			base.Awake();
			FetchStoredSettings();
		}

		private void FetchStoredSettings()
		{
			SetupScreen();
			PlayerPrefs.Save();
		}

		private void SetupScreen()
		{
			var width = GetInt(SettingKey.Width, Screen.currentResolution.width);
			var height = GetInt(SettingKey.Height, Screen.currentResolution.height);
			var hz = GetInt(SettingKey.RefreshRate, Screen.currentResolution.refreshRate);
			var smode = GetString(SettingKey.FullscreenMode, Screen.fullScreenMode.ToString());

			FullScreenMode mode = Screen.fullScreenMode;
			if (Enum.TryParse<FullScreenMode>(smode, out var result))
			{
				mode = result;
			}

			Screen.SetResolution(width, height, mode, hz);
		}

		public static int GetInt(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString());
		}

		public static int GetInt(SettingKey key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue);
		}

		public static void SetInt(SettingKey key, int value)
		{
			PlayerPrefs.SetInt(key.ToString(), value);
			PlayerPrefs.Save();
		}

		public static bool GetBool(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString()) == 1;
		}

		public static bool GetBool(SettingKey key, bool defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue ? 1 : 0) == 1;
		}

		public static void SetBool(SettingKey key, bool value)
		{
			PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
			PlayerPrefs.Save();
		}

		public static string GetString(SettingKey key)
		{
			return PlayerPrefs.GetString(key.ToString());
		}

		public static string GetString(SettingKey key, string defaultValue)
		{
			return PlayerPrefs.GetString(key.ToString(), defaultValue);
		}

		public static void SetString(SettingKey key, string value)
		{
			PlayerPrefs.SetString(key.ToString(), value);
			PlayerPrefs.Save();
		}
	}
}