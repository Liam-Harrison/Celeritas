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
	private List<MovingStatBar> movingStatBars; // stat bars that follow ship entities

	protected override void Awake()
	{
		base.Awake();

		movingStatBars = new List<MovingStatBar>();

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

		foreach (MovingStatBar statBar in movingStatBars)
		{
			statBar.UpdateLocation();
		}
	}

	/// <summary>
	/// creates a health bar that will follow ship entities around
	/// </summary>
	public void CreateHealthBarThatFollowsShip(ShipEntity ship)
	{
		var healthBar = new GameObject();
		healthBar.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
		healthBar.transform.localPosition = ship.transform.position;

		var healthBorder = new GameObject();
		healthBorder.transform.SetParent(healthBar.transform, false);
		Image bigBar = healthBorder.AddComponent<Image>();
		bigBar.rectTransform.sizeDelta = new Vector2(9, 9); // wrong

		var healthFill = new GameObject();
		healthFill.transform.SetParent(healthBar.transform, false);
		Image smallBar = healthFill.AddComponent<Image>();
		smallBar.rectTransform.sizeDelta = new Vector2(20, 5);
		smallBar.color = Color.red;

		Slider slider = healthBar.AddComponent<Slider>();
		slider.fillRect = smallBar.rectTransform;

		MovingStatBar toAdd = healthBar.AddComponent<MovingStatBar>();
		toAdd.Ship = ship;
		toAdd.StatBar = healthBar;
		toAdd.EntityStats = ship.Health; // todo: add shield too
		toAdd.slider = slider;

		movingStatBars.Add(toAdd);
	}
}
