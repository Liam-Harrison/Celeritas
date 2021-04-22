using Celeritas.Game.Actions;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField]
	private GameObject abilityPrefab;

	public List<UIAbilitySlot> AbilitySlots { get; private set; } = new List<UIAbilitySlot>();

	public void LinkAction(GameAction action)
	{
		var slot = Instantiate(abilityPrefab, transform).GetComponent<UIAbilitySlot>();

		slot.LinkToAction(action);

		AbilitySlots.Add(slot);
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
}
