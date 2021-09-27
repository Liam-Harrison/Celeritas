using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class KeybindSettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private GameObject rebindUI;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI selectionText;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject keybindItemPrefab;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject settingsGroup;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject settingsButton;

		private readonly HashSet<KeybindItemUI> items = new HashSet<KeybindItemUI>();

		private void Awake()
		{
			List<string> names = new List<string>();

			foreach (var action in SettingsManager.InputActions)
			{
				var group = action.actionMap.name;
				if (names.Contains(group) == false)
				{
					names.Add(group);
					var text = Instantiate(settingsGroup, transform).GetComponentInChildren<TextMeshProUGUI>();
					text.text = group;
				}

				if (action.bindings.Count > 1)
				{
					for (int i = 1; i < action.bindings.Count; i++)
					{
						var keybind = Instantiate(keybindItemPrefab, transform).GetComponent<KeybindItemUI>();
						keybind.BindToAction(action, RebindItem, i);
						items.Add(keybind);
					}
				}
				else
				{
					var keybind = Instantiate(keybindItemPrefab, transform).GetComponent<KeybindItemUI>();
					keybind.BindToAction(action, RebindItem);
					items.Add(keybind);
				}
			}

			var button = Instantiate(settingsButton, transform).GetComponentInChildren<Button>();
			button.GetComponentInChildren<TextMeshProUGUI>().text = $"Reset All Keybinds";
			button.onClick.AddListener(ResetKeybinds);
		}

		public void ResetKeybinds()
		{
			foreach (var action in SettingsManager.InputActions)
			{
				action.RemoveAllBindingOverrides();
			}

			foreach (var keybind in items)
			{
				keybind.UpdateText();
			}

			SettingsManager.SaveAllKeybinds();
		}

		InputActionRebindingExtensions.RebindingOperation operation;

		private void RebindItem(KeybindItemUI keybind)
		{
			var action = keybind.InputAction;

			rebindUI.SetActive(true);
			selectionText.text = keybind.GetBindingName();

			action.Disable();
			operation = action.PerformInteractiveRebinding(keybind.Binding)
							.OnMatchWaitForAnother(0.1f)
							.WithCancelingThrough("<Keyboard>/escape")
							.OnCancel(_ => RebindCancelled())
							.OnComplete(_ => RebindComplete(keybind))
							.Start();
		}

		private void RebindCancelled()
		{
			HideKeybindUI();
		}

		private void RebindComplete(KeybindItemUI keybind)
		{
			HideKeybindUI();
			keybind.InputAction.Enable();
			keybind.UpdateText();
			SettingsManager.SaveActionKeybind(keybind.InputAction);
			PlayerPrefs.Save();
		}

		private void HideKeybindUI()
		{
			operation.Dispose();
			rebindUI.SetActive(false);
		}
	}
}
