using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDManager : Singleton<CombatHUDManager>
{
	public StatBar playerMainHealthBar; // main health bar at the bottom of the screen
	public StatBar playerMainShieldBar; // main shield bar for player

	private ShipEntity playerShip;
	private List<GameObject> followingHealthBars; // maybe put into ShipEntity instead?

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

			CreateHealthBarThatFollowsShip(playerShip);
		}
	}

	/// <summary>
	/// creates a health bar that will follow ship entities around
	/// </summary>
	public GameObject CreateHealthBarThatFollowsShip(ShipEntity ship)
	{
		var healthBar = new GameObject();
		healthBar.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
		healthBar.transform.localPosition = ship.transform.position;

		Image bigBar = healthBar.AddComponent<Image>();
		bigBar.rectTransform.sizeDelta = new Vector2(150, 15);

		Image smallBar = healthBar.AddComponent<Image>();
		//smallBar.rectTransform.sizeDelta = new Vector2(150, 15);
		//smallBar.color = Color.red;

		return healthBar;

	}
}
