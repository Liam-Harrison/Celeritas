using Celeritas.Game.Actions;
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

		public List<UIAbilitySlot> AbilitySlots { get; private set; } = new List<UIAbilitySlot>();

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

				CreateAction(i <= 3 ? primaryBar : secondaryBar, action);

				if (i == 3)
					j = 0;
				else
					j++;
			}

			SettingsManager.InputActions.Basic.AlternateAbilities.performed += AlternateAbilitiesPerformed;
			SettingsManager.InputActions.Basic.AlternateAbilities.canceled += AlternateAbilitiesCanceled;
		}

		private void CreateAction(Transform parent, InputAction action)
		{
			var slot = Instantiate(abilityPrefab, parent).GetComponent<UIAbilitySlot>();
			slot.Initalize(this, action);
			AbilitySlots.Add(slot);
		}

		public void LinkAction(GameAction action)
		{
			foreach (var slot in AbilitySlots)
			{
				if (slot.LinkedAction == null)
				{
					slot.LinkToAction(action);
					return;
				}
			}
		}

		public void UnlinkAction(GameAction action)
		{
			for (int i = 0; i < AbilitySlots.Count; i++)
			{
				if (AbilitySlots[i].LinkedAction == action)
				{
					Destroy(AbilitySlots[i].gameObject);
					AbilitySlots.RemoveAt(i);
					return;
				}
			}
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
			actionSelection.Show(slot);
		}
	}
}