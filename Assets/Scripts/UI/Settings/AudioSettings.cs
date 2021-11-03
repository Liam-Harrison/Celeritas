using FMOD.Studio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class AudioSettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private Slider volume;

		private Bus Master;

		private void Awake()
		{
			SetupUI();
			Master = FMODUnity.RuntimeManager.GetBus("bus:/");
			volume.onValueChanged.AddListener(OnStackingNumbersChanged);
		}

		private void SetupUI()
		{
			volume.value = AudioListener.volume;
		}

		private void OnStackingNumbersChanged(float value)
		{
			Master.setVolume(value);

			AudioListener.volume = value;
			SettingsManager.SetFloat(SettingKey.Volume, value);
		}
	}
}