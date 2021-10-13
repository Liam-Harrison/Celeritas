using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class GameplaySettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private Toggle stackingNumbers;
		[SerializeField, TitleGroup("Assignments")]
		private Toggle tutorialEvent;

		private void Awake()
		{
			SetupUI();
			stackingNumbers.onValueChanged.AddListener(OnStackingNumbersChanged);
			tutorialEvent.onValueChanged.AddListener(OnTutorialChanged);
		}

		private void SetupUI()
		{
			stackingNumbers.isOn = SettingsManager.StackingDamageNumbers;
			tutorialEvent.isOn = SettingsManager.TutorialEvent;
		}

		private void OnStackingNumbersChanged(bool value)
		{
			SettingsManager.StackingDamageNumbers = value;
			SettingsManager.SetBool(SettingKey.StackingNumbers, value);
		}

		private void OnTutorialChanged(bool value)
		{
			SettingsManager.TutorialEvent = value;
			SettingsManager.SetBool(SettingKey.Tutorial, value);
		}
	}
}