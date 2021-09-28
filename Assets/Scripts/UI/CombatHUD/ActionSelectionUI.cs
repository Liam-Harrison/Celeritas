using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	public class ActionSelectionUI : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private GameObject optionPrefab;

		public new RectTransform transform {  get; set; }

		private void Awake()
		{
			transform = GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			SettingsManager.InputActions.Basic.Fire.performed += FirePerformed;
		}

		private void OnDisable()
		{
			SettingsManager.InputActions.Basic.Fire.performed -= FirePerformed;
		}

		public void Show(UIAbilitySlot slot, List<GameAction> actions)
		{
			// draw a button for each action

			gameObject.SetActive(true);
			CombatHUD.Instance.SetGameCursor(false);

			var corners = new Vector3[4];
			slot.transform.GetWorldCorners(corners);

			transform.position = (corners[1] + corners[2]) / 2;
		}

		private void Hide()
		{
			gameObject.SetActive(false);
			CombatHUD.Instance.SetGameCursor(true);
			transform.DestroyAllChildren();
		}

		private void FirePerformed(InputAction.CallbackContext obj)
		{
			var pos = Mouse.current.position.ReadValue();
			var l = transform.InverseTransformPoint(pos);

			if (transform.rect.Contains(l) == false)
			{
				Hide();
			}
		}

		private void ActionSelected(GameAction action)
		{
			Hide();
		}
	}
}
