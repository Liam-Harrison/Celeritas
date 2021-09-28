using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
using Celeritas.UI.Tooltips;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// Represents a single square that displays ability icon/controls in the HUD.
	/// Includes what control must be pressed to activate it, a representative icon, and (pending) cooldown.
	/// </summary>
	public class UIAbilitySlot : MonoBehaviour, ITooltip, IPointerClickHandler
	{
		/// <summary>
		/// Get the action linked to this UI slot.
		/// </summary>
		public GameAction LinkedAction { get; private set; }

		[SerializeField, Title("Assignments")]
		private Image icon;

		[SerializeField]
		private Image highlight;

		[SerializeField]
		private TextMeshProUGUI key;

		[SerializeField]
		private TextMeshProUGUI time;

		public ModuleEntity TooltipEntity => null;

		public GameAction TooltipAction => LinkedAction;

		public InputAction InputAction { get; private set; }

		public AbilityBar AbilityBar { get; private set; }

		public new RectTransform transform { get; set; }

		private void Awake()
		{
			transform = GetComponent<RectTransform>();
			icon.enabled = false;
			time.enabled = false;
			highlight.enabled = false;
			SettingsManager.OnKeybindChanged += OnSettingsChanged;
		}

		public void Initalize(AbilityBar abilityBar,  InputAction inputAction)
		{
			AbilityBar = abilityBar;
			InputAction = inputAction;
			key.text = inputAction.GetBindingDisplayString();
			inputAction.performed += InputAction_performed;
			inputAction.canceled += InputAction_canceled;
		}

		private void Update()
		{
			if (LinkedAction == null)
				return;

			time.text = $"{LinkedAction.TimeLeftOnCooldown:0.0}";
		}

		/// <summary>
		/// Link this slot to an action.
		/// </summary>
		/// <param name="action">The action to link this to.</param>
		public void LinkToAction(GameAction action)
		{
			LinkedAction = action;

			if (action.Data.Icon != null)
			{
				icon.sprite = action.Data.Icon;
				icon.enabled = true;
			}
			else
				icon.enabled = false;

			time.text = "0.0";
			time.enabled = true;
		}

		public void UnlinkAction()
		{
			LinkedAction = null;
			icon.enabled = false;
			time.enabled = false;
			highlight.enabled = false;
		}

		/// <summary>
		/// Used when the Slot has no active ability to display
		/// Makes the ability slot invisible
		/// </summary>
		public void Hide()
		{
			gameObject.SetActive(false);
		}

		private void InputAction_performed(InputAction.CallbackContext obj)
		{
			if (LinkedAction != null)
			{
				highlight.enabled = LinkedAction.Data.IsPassive == false;
			}
			else
			{
				highlight.enabled = true;
			}
		}

		private void InputAction_canceled(InputAction.CallbackContext obj)
		{
			highlight.enabled = false;
		}

		private void OnSettingsChanged()
		{
			if (InputAction != null)
				key.text = InputAction.GetBindingDisplayString();
		}

		public void OnPointerClick(PointerEventData data)
		{
			if (data.button == PointerEventData.InputButton.Right)
			{
				AbilityBar.ChangeAction(this);
			}
		}
	}
}