using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Celeritas.UI
{
	public class KeybindItemUI : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private TextMeshProUGUI value;

		public void BindToAction(InputAction action)
		{
			label.text = action.name;
			value.text = action.GetBindingDisplayString();
		}

		public void RebindAction()
		{

		}
	}
}