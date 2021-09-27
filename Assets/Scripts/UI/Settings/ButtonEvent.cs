using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	public class ButtonEvent : MonoBehaviour
	{
		[SerializeField, TitleGroup("Asset")]
		InputActionAsset asset;

		[SerializeField, TitleGroup("Action"), ValueDropdown(nameof(GetOptions))]
		private string selectedMap;

		[SerializeField, TitleGroup("Action"), ValueDropdown(nameof(GetActionOptions)), ShowIf("@this.selectedMap != null && this.selectedMap != \"\"")]
		private string selectedAction;

		[SerializeField, TitleGroup("Events"), ShowIf("@this.selectedAction != null && this.selectedAction != \"\"")]
		private UnityEvent performed;

		[SerializeField, TitleGroup("Events"), ShowIf("@this.selectedAction != null && this.selectedAction != \"\"")]
		private UnityEvent cancelled;

		private void OnEnable()
		{
			var action = GetLiveAction();

			if (action == null)
				return;

			action.canceled += ActionCanceled;
			action.performed += ActionPerformed;
		}

		private void OnDisable()
		{
			var action = GetLiveAction();

			if (action == null)
				return;

			action.canceled -= ActionCanceled;
			action.performed -= ActionPerformed;
		}

		private void OnValidate()
		{
			if (GetCurrentActionMap() == null)
			{
				selectedMap = "";
				selectedAction = "";
			}
			else if (GetCurrentAction() == null)
			{
				selectedAction = "";
			}
		}

		private IEnumerable<string> GetOptions()
		{
			List<string> options = new List<string>();

			if (asset == null)
				return options;

			foreach (var map in asset.actionMaps)
			{
				options.Add(map.name);
			}
			return options;
		}

		private IEnumerable<string> GetActionOptions()
		{
			List<string> options = new List<string>();
			var map = GetCurrentActionMap();

			if (string.IsNullOrEmpty(selectedMap) || map == null)
				return options;

			foreach (var action in map)
			{
				options.Add(action.name);
			}
			return options;
		}

		private InputActionMap GetCurrentActionMap()
		{
			foreach (var map in asset.actionMaps)
			{
				if (map.name == selectedMap)
					return map;
			}
			return null;
		}

		private InputAction GetCurrentAction()
		{
			var map = GetCurrentActionMap();

			if (map == null)
				return null;

			foreach (var action in map)
			{
				if (action.name == selectedAction)
					return action;
			}
			return null;
		}

		private void ActionPerformed(InputAction.CallbackContext obj)
		{
			performed.Invoke();
		}

		private void ActionCanceled(InputAction.CallbackContext obj)
		{
			cancelled.Invoke();
		}

		private InputAction GetLiveAction()
		{
			var desiredAction = GetCurrentAction();
			var actions = SettingsManager.InputActions;

			if (desiredAction == null)
				return null;

			foreach (var action in actions)
			{
				if (action.name == desiredAction.name)
				{
					return action;
				}
			}

			return null;
		}
	}
}
