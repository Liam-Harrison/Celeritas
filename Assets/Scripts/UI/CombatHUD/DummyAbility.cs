using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pretend ability for the HUD design
/// </summary>
public class DummyAbility
{

	public DummyAbility(string inputButtonString)
	{
		inputButton = inputButtonString;
	}

	public string inputButton; // used to activate ability

	public Sprite icon;

	// cooldown

}
