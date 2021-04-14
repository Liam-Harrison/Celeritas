using Celeritas.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStatBar : StatBar
{
	private ShipEntity ship; // the ship this bar's position is locked onto.

	private GameObject statBar;

	public ShipEntity Ship { get => ship; set => ship = value; }

	public GameObject StatBar { get => statBar; set => statBar = value; }

	public void UpdateLocation()
    {
		if (ship != null) // to avoid errors when ship is destroyed
		{ 
			statBar.transform.localPosition = ship.transform.localPosition;
		}
	}
}
