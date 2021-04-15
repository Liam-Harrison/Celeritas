using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{

	private List<UIAbilitySlot> abilitySlots; // UIAbilitySlot manage the ability slot GameObjects

	private void Awake()
	{
		abilitySlots = new List<UIAbilitySlot>();

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
	/// </summary>
	/// <param name="toAdd"></param>
	/// <param name="index"></param>
	public void AddAbility(DummyAbility toAdd, int index)
	{
		abilitySlots[index].Ability = toAdd;
		abilitySlots[index].Initalize();
	}

	/// <summary>
	/// Empty the ability slot at the given index
	/// </summary>
	/// <param name="index"></param>
	private void RemoveAbility(int index)
	{
		abilitySlots[index].Empty();
	}
}
