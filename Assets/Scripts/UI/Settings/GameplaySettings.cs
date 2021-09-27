using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class GameplaySettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private Toggle stackingNumbers;

		private void Awake()
		{
			SetupUI();
			stackingNumbers.onValueChanged.AddListener(OnStackingNumbersChanged);
		}

		private void SetupUI()
		{
			stackingNumbers.isOn = SettingsManager.StackingDamageNumbers;
		}

		private void OnStackingNumbersChanged(bool value)
		{
			SettingsManager.StackingDamageNumbers = value;
			SettingsManager.SetBool(SettingKey.StackingNumbers, value);
		}
	}
}