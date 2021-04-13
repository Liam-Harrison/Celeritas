using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatHUDManager : Singleton<CombatHUDManager>
{
	public StatBar playerMainHealthBar; // main health bar at the bottom of the screen
	public StatBar playerMainShieldBar; // main shield bar for player

	private ShipEntity playerShip;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		// setup main health bar for player
		if (playerShip == null && PlayerController.Instance != null) {
			playerShip = PlayerController.Instance.ShipEntity;

			playerMainHealthBar.EntityStats = playerShip.Health;

			playerMainShieldBar.EntityStats = playerShip.Shield;
		}
	}
}
