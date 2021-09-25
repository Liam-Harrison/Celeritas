using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	public class KeybindItemUI : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private TextMeshProUGUI value;

		private Action<KeybindItemUI> callback = null;

		public InputAction InputAction { get; private set; }

		public int Binding { get; private set;  }

		public void BindToAction(InputAction action, Action<KeybindItemUI> callback, int binding = -1)
		{
			this.callback = callback;
			Binding = binding;
			InputAction = action;
			UpdateText();
		}

		public void UpdateText()
		{
			if (InputAction.bindings.Count == 5)
			{
				string direction;
				if (Binding == 1)
					direction = "Forward";
				else if (Binding == 2)
					direction = "Back";
				else if (Binding == 3)
					direction = "Left";
				else
					direction = "Right";

				label.text = $"{InputAction.name} {direction}";
			}
			else
			{
				label.text = InputAction.name;
			}

			value.text = GetBindingName();
		}

		public string GetBindingName()
		{
			if (Binding != -1)
				return InputAction.GetBindingDisplayString(Binding);
			else
				return InputAction.GetBindingDisplayString();
		}

		public void RebindAction()
		{
			callback?.Invoke(this);
		}
	}
}