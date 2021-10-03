using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace Celeritas.UI.General
{
	public class ToggleText : MonoBehaviour
	{
		[SerializeField, TitleGroup("Settings")]
		private string onText;

		[SerializeField, TitleGroup("Settings")]
		private string offText;

		[SerializeField, TitleGroup("Settings")]
		private TextMeshProUGUI text;

		public void SetText(bool state)
		{
			text.text = state ? onText : offText;
		}
	}
}