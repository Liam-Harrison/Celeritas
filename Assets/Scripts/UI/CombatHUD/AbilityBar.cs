using Celeritas.Game.Actions;
using Celeritas.Game.Controllers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	/// <summary>
	/// Logic for the ability bar
	/// Can add abilities, which will be displayed to the user
	/// Can also remove abilities at specific indexes.
	///
	/// Will manage all AbilitySlots stored under AbilityBar, so can add/remove with no problem.
	/// Index refers to the order in which the AbilitySlots are in, under AbilityBar.
	/// </summary>
	public class AbilityBar : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private Transform primaryBar;

		[SerializeField, TitleGroup("Assignments")]
		private Transform secondaryBar;

		[SerializeField, TitleGroup("Assignments")]
		private ActionSelectionUI actionSelection;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject abilityPrefab;

		private UIAbilitySlot[] primaries = new UIAbilitySlot[4];
		private UIAbilitySlot[] alternates = new UIAbilitySlot[4];

		void Start()
		{
			int j = 0;
			for (int i = 0; i < 8; i++)
			{
				InputAction action;

				if (j == 0)
					action = SettingsManager.InputActions.Basic.Ability1;
				else if (j == 1)
					action = SettingsManager.InputActions.Basic.Ability2;
				else if (j == 2)
					action = SettingsManager.InputActions.Basic.Ability3;
				else
					action = SettingsManager.InputActions.Basic.Ability4;

				bool isSecondary = i > 3;
				CreateAction(j, isSecondary, action, isSecondary ? secondaryBar : primaryBar);

				if (i == 3)
					j = 0;
				else
					j++;
			}

			SettingsManager.InputActions.Basic.AlternateAbilities.performed += AlternateAbilitiesPerformed;
			SettingsManager.InputActions.Basic.AlternateAbilities.canceled += AlternateAbilitiesCanceled;

			PlayerController.OnActionLinked += OnActionLinked;
			PlayerController.OnActionUnlinked += OnActionUnlinked;
		}

		private void OnDestroy()
		{
			PlayerController.OnActionLinked -= OnActionLinked;
			PlayerController.OnActionUnlinked -= OnActionUnlinked;
		}

		private void OnActionLinked(int index, bool isAlternate, GameAction action)
		{
			if (isAlternate)
				alternates[index].LinkToAction(action);
			else
				primaries[index].LinkToAction(action);
		}

		private void OnActionUnlinked(int index, bool isAlternate)
		{
			if (isAlternate)
				alternates[index].UnlinkAction();
			else
				alternates[index].UnlinkAction();
		}

		private void CreateAction(int index, bool isAlternate, InputAction action, Transform parent)
		{
			var slot = Instantiate(abilityPrefab, parent).GetComponent<UIAbilitySlot>();
			slot.Initalize(index, isAlternate, this, action);

			if (isAlternate)
				alternates[index] = slot;
			else
				primaries[index] = slot;
		}

		private void AlternateAbilitiesPerformed(InputAction.CallbackContext obj)
		{
			primaryBar.gameObject.SetActive(false);
			secondaryBar.gameObject.SetActive(true);
		}

		private void AlternateAbilitiesCanceled(InputAction.CallbackContext obj)
		{
			primaryBar.gameObject.SetActive(true);
			secondaryBar.gameObject.SetActive(false);
		}

		public void ChangeAction(UIAbilitySlot slot)
		{
			actionSelection.Show(slot, PlayerController.Instance.PlayerShipEntity.Actions);
		}
	}
}