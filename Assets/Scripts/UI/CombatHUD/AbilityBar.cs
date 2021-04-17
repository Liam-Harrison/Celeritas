using System.Collections;
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

	private List<UIAbilitySlot> abilitySlots; // UIAbilitySlot manage the ability slot GameObjects

	private void Awake()
	{
		abilitySlots = new List<UIAbilitySlot>();

		// UI slots are not pre-fabs, as then we can move the slots around as we see fit.
		UIAbilitySlot[] slots = GetComponentsInChildren<UIAbilitySlot>();
		abilitySlots.AddRange(slots);

		foreach (UIAbilitySlot slot in abilitySlots)
		{
			slot.Empty();
		}

	}

	// Start is called before the first frame update
	void Start()
    {

	}

	// Update is called once per frame
	void Update()
    {
        
    }

	/// <summary>
	/// Add an ability to the StatBar at the given index
	/// Index reflects the AbilitySlot number (in the Unity Hierarchy view)
	/// So an ability put into index 2 will display at the same point as AbilitySlot3.
	/// (ui_console/Canvas/AbilityBar/<AbilitySlots>)
	/// </summary>
	/// <param name="toAdd">Ability to add to the AbilityBar</param>
	/// <param name="index">Index of the HUD AbilitySlot you want the ability to be added to.</param>
	public void AddAbility(DummyAbility toAdd, int index)
	{
		abilitySlots[index].Ability = toAdd;
		abilitySlots[index].Initalize();
	}

	/// <summary>
	/// Empty the ability slot at the given index
	/// </summary>
	/// <param name="index">Index of the HUD AbilitySlot you want the ability to be removed from.</param>
	private void RemoveAbility(int index)
	{
		abilitySlots[index].Empty();
	}
}
