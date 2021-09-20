using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Sirenix.OdinInspector;

namespace Celeritas.UI
{
	public class KeybindSettings : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private GameObject keybindItemPrefab;

		[SerializeField, TitleGroup("Assignments")]
		private GameObject settingsGroup;

		private InputActions inputActions;

		private void Awake()
		{
			inputActions = new InputActions();

			List<string> names = new List<string>();

			foreach (var action in inputActions)
			{
				var group = action.actionMap.name;
				if (names.Contains(group) == false)
				{
					names.Add(group);
					var text = Instantiate(settingsGroup, transform).GetComponentInChildren<TextMeshProUGUI>();
					text.text = group;
				}

				var keybind = Instantiate(keybindItemPrefab, transform).GetComponent<KeybindItemUI>();
				keybind.BindToAction(action);
			}
		}
	}
}