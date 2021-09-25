using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeritas.UI
{
	public class GraphicsSettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private TMP_Dropdown resolution;

		[SerializeField, TitleGroup("Assignments")]
		private TMP_Dropdown refreshRate;

		[SerializeField, TitleGroup("Assignments")]
		private TMP_Dropdown fullscreen;

		private readonly List<Resolution> sResolutions = new List<Resolution>();
		private readonly List<FullScreenMode> sFullscreenModes = new List<FullScreenMode>();
		private readonly List<int> sRefreshRates = new List<int>();

		private Resolution selected;

		private void Awake()
		{
			SetupUI();
			resolution.onValueChanged.AddListener(OnResolutionSelected);
			refreshRate.onValueChanged.AddListener(OnRefreshRateSelected);
			fullscreen.onValueChanged.AddListener(OnFullscreenModeChanged);
		}

		private void SetupUI()
		{
			selected = GetRealResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.currentResolution.refreshRate);
			SetupResolutionDropdown();
			SetupRefreshRates(Screen.currentResolution);
			SetupFullscreen(Screen.fullScreenMode);
		}

		private void SetupResolutionDropdown()
		{
			resolution.ClearOptions();
			sResolutions.Clear();
			var resolutions = Screen.resolutions;

			int selected = 0;
			var options = new List<string>();

			for (int i = 0; i < resolutions.Length; i++)
			{
				var resolution = resolutions[i];
				var existing = ContainsResolution(resolution);

				if (existing == -1)
				{
					sResolutions.Add(resolution);
					options.Add($"{resolution.width} x {resolution.height}");

					if (Screen.currentResolution.width == resolution.width &&
						Screen.currentResolution.height == resolution.height)
					{
						selected = i;
					}
				}
				else
				{
					sResolutions[existing] = resolution;
				}
			}

			resolution.AddOptions(options);
			resolution.SetValueWithoutNotify(selected);
		}

		private void SetupRefreshRates(Resolution resolution)
		{
			refreshRate.ClearOptions();
			sRefreshRates.Clear();
			var options = new List<string>();

			int selected = 0;
			foreach (var item in GetRefreshRates(resolution))
			{
				sRefreshRates.Add(item);
				options.Add($"{item}Hz");
				if (item == resolution.refreshRate)
				{
					selected = item;
				}
			}

			refreshRate.AddOptions(options);
			refreshRate.SetValueWithoutNotify(selected);
		}

		private void SetupFullscreen(FullScreenMode mode)
		{
			fullscreen.ClearOptions();
			sFullscreenModes.Clear();
			var options = new List<string>();
			var modes = Enum.GetValues(typeof(FullScreenMode));

			int selected = 0;
			for (int i = 0; i < modes.Length; i++)
			{
				var item = (FullScreenMode) modes.GetValue(i);
				sFullscreenModes.Add(item);

				if (item == FullScreenMode.ExclusiveFullScreen)
					options.Add("Fullscreen");
				else
					options.Add(item.ToString().AsDisplayString());

				if (item == mode)
				{
					selected = i;
				}
			}

			fullscreen.AddOptions(options);
			fullscreen.SetValueWithoutNotify(selected);
		}

		private int ContainsResolution(Resolution resolution)
		{
			for (int i = 0; i < sResolutions.Count; i++)
			{
				var r = sResolutions[i];

				if (resolution.width == r.width &&
					resolution.height == r.height)
				{
					return i;
				}
			}
			return -1;
		}

		private Resolution GetRealResolution(int width, int height, int refreshRate)
		{
			foreach (var mode in Screen.resolutions)
			{
				if (mode.width == width &&
					mode.height == height &&
					mode.refreshRate == refreshRate)
				{
					return mode;
				}
			}
			return Screen.resolutions[Screen.resolutions.Length - 1];
		}

		private List<int> GetRefreshRates(Resolution resolution)
		{
			var resolutions = Screen.resolutions;
			List<int> rates = new List<int>();

			foreach (var r in resolutions)
			{
				if (r.width == resolution.width &&
					r.height == resolution.height)
				{
					rates.Add(r.refreshRate);
				}
			}

			return rates;
		}

		private void OnResolutionSelected(int index)
		{
			Debug.Log($"{sResolutions[index].width}, {sResolutions[index].height}, {Screen.fullScreenMode}, {selected.refreshRate}");
			Screen.SetResolution(sResolutions[index].width, sResolutions[index].height, Screen.fullScreenMode, selected.refreshRate);
			SetupRefreshRates(sResolutions[index]);

			SettingsManager.SetInt(SettingKey.Width, sResolutions[index].width);
			SettingsManager.SetInt(SettingKey.Height, sResolutions[index].height);
			selected = GetRealResolution(sResolutions[index].width, sResolutions[index].height, selected.refreshRate);
		}

		private void OnRefreshRateSelected(int index)
		{
			Debug.Log($"{selected.width}, {selected.height}, {Screen.fullScreenMode}, {sRefreshRates[index]}");
			Screen.SetResolution(selected.width, selected.height, Screen.fullScreenMode, sRefreshRates[index]);
			
			SettingsManager.SetInt(SettingKey.RefreshRate, sRefreshRates[index]);
			selected = GetRealResolution(selected.width, selected.height, sRefreshRates[index]);
		}

		private void OnFullscreenModeChanged(int index)
		{
			Screen.fullScreenMode = sFullscreenModes[index];
			SettingsManager.SetString(SettingKey.FullscreenMode, sFullscreenModes[index].ToString());
		}
	}
}
