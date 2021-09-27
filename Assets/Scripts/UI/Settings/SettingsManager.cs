using Celeritas.Game;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas
{
	public enum SettingKey
	{
		Width,
		Height,
		RefreshRate,
		FullscreenMode,
		StackingNumbers,
	}

	public class SettingsManager : Singleton<SettingsManager>
	{
		public static InputActions InputActions { get; private set; }

		public static bool StackingDamageNumbers { get; set; }

		protected override void Awake()
		{
			base.Awake();
			InputActions = new InputActions();
			FetchStoredSettings();
		}

		private void FetchStoredSettings()
		{
			SetupScreen();
			LoadKeybinds();
			LoadGameplaySettings();
			PlayerPrefs.Save();
		}

		private void LoadGameplaySettings()
		{
			StackingDamageNumbers = GetBool(SettingKey.StackingNumbers, true);
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

		private void LoadKeybinds()
		{
			foreach (var action in InputActions)
			{
				for (int i = 0; i < action.bindings.Count; i++)
				{
					var binding = action.bindings[i];
					var key = $"{action.name},{binding.name}";

					if (PlayerPrefs.HasKey(key))
					{
						if (PlayerPrefs.GetString(key) == "")
							PlayerPrefs.DeleteKey(key);
						else
						{
							action.ApplyBindingOverride(i, PlayerPrefs.GetString(key));
						}
					}
				}
			}
		}

		public static void SaveActionKeybind(InputAction action)
		{
			for (int i = 0; i < action.bindings.Count; i++)
			{
				var binding = action.bindings[i];
				var key = $"{action.name},{binding.name}";

				if (binding.overridePath == "")
				{
					if (PlayerPrefs.HasKey(key))
					{
						PlayerPrefs.DeleteKey(key);
					}
				}
				else
				{
					PlayerPrefs.SetString(key, binding.overridePath);
				}
			}
		}

		public static void SaveAllKeybinds()
		{
			foreach (var action in InputActions)
			{
				SaveActionKeybind(action);
			}
			PlayerPrefs.Save();
		}

		public static int GetInt(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString());
		}

		public static int GetInt(SettingKey key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue);
		}

		public static bool GetBool(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString()) == 1;
		}

		public static bool GetBool(SettingKey key, bool defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue ? 1 : 0) == 1;
		}

		public static string GetString(SettingKey key)
		{
			return PlayerPrefs.GetString(key.ToString());
		}

		public static string GetString(SettingKey key, string defaultValue)
		{
			return PlayerPrefs.GetString(key.ToString(), defaultValue);
		}

		public static void SetInt(SettingKey key, int value)
		{
			PlayerPrefs.SetInt(key.ToString(), value);
			PlayerPrefs.Save();
		}

		public static void SetBool(SettingKey key, bool value)
		{
			PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
			PlayerPrefs.Save();
		}

		public static void SetString(SettingKey key, string value)
		{
			PlayerPrefs.SetString(key.ToString(), value);
			PlayerPrefs.Save();
		}
	}
}