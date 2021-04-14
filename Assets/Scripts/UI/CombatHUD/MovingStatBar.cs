using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Game.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingStatBar : StatBar, IPooledObject
{
	private ShipEntity ship; // the ship this bar's position is locked onto.

	public ShipEntity Ship { get => ship; set => ship = value; }

	public Color barFillColor = Color.red;

	[SerializeField]
	public Vector3 displacement; // from centre of ship (eg move bar up or down)

	public void OnDespawned(){ }

	public void OnSpawned(){ }

	public void Initalize(ShipEntity shipToFollow, EntityStatBar toTrack)
	{
		ship = shipToFollow;
		slider = GetComponent<Slider>();
		EntityStats = toTrack;

		Image[] images = GetComponentsInChildren<Image>();
		images[1].color = barFillColor;
	}

	public void UpdateLocation()
    {
		if (ship != null) // to avoid errors when ship is destroyed
		{
			transform.position = Camera.main.WorldToScreenPoint(ship.transform.position + displacement);
		}
	}
}
