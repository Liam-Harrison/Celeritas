using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDManager : Singleton<CombatHUDManager>
{
	[SerializeField]
	private StatBar playerMainHealthBar; // main health bar at the bottom of the screen

	[SerializeField]
	private StatBar playerMainShieldBar; // main shield bar for player

	[SerializeField, PropertySpace(20)]
	private GameObject floatingHealthBarPrefab;

	[SerializeField, PropertySpace(20)]
	private GameObject floatingShieldBarPrefab;

	[SerializeField]
	private GameObject canvas;

	// stat bars that follow ship entities
	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingHealthStatBars; 

	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingShieldStatBars;

	protected override void Awake()
	{
		base.Awake();

		pooledFloatingHealthStatBars = new ObjectPool<MovingStatBar>(floatingHealthBarPrefab, transform);
		pooledFloatingShieldStatBars = new ObjectPool<MovingStatBar>(floatingShieldBarPrefab, transform);

		// trigger this class's 'OnCreatedEntity' when that event occurs in EntityDataManager
		EntityDataManager.OnCreatedEntity += OnCreatedEntity;

	}

	protected override void OnDestroy()
	{
		EntityDataManager.OnCreatedEntity -= OnCreatedEntity;
		base.OnDestroy();
	}

	private void OnCreatedEntity(Entity entity)
	{
		if (entity is ShipEntity ship)
		{
			AddFloatingHealthBarToShip(ship);

			// don't add shield stat bar to ships without any shield.
			if (! ship.Shield.IsEmpty())
				AddFloatingShieldBarToShip(ship);
		}
	}

	private ShipEntity playerShip;

	private void Update()
	{

		// if just starting, link stationary stat bars to PlayerShip
		if (playerShip == null && PlayerController.Instance != null) {
			playerShip = PlayerController.Instance.ShipEntity;

			playerMainHealthBar.EntityStats = playerShip.Health;

			playerMainShieldBar.EntityStats = playerShip.Shield;

		}

		// update floating health bars every loop (so they can follow their ships)
		UpdateStatBarPool(pooledFloatingHealthStatBars);
		UpdateStatBarPool(pooledFloatingShieldStatBars);

	}

	private void AddFloatingHealthBarToShip(ShipEntity ship)
	{
		var healthBar = pooledFloatingHealthStatBars.GetPooledObject();
		healthBar.Initalize(ship, ship.Health);
		healthBar.transform.SetParent(canvas.transform);
	}

	private void AddFloatingShieldBarToShip(ShipEntity ship)
	{
		var shieldBar = pooledFloatingShieldStatBars.GetPooledObject();
		shieldBar.Initalize(ship, ship.Shield);
		shieldBar.transform.SetParent(canvas.transform);
	}

	private void UpdateStatBarPool(ObjectPool<MovingStatBar> toUpdate) {
		//foreach (var statBar in toUpdate.ActiveObjects)
		for (int i = 0; i < toUpdate.ActiveObjects.Count; i++)
		{
			var statBar = toUpdate.ActiveObjects[i];

			if (statBar.Ship.Died || statBar.Ship == null)
			{
				toUpdate.ReleasePooledObject(statBar);
				continue;
			}

			statBar.UpdateLocation();
		}
	}

}
