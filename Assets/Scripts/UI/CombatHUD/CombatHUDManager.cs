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
	public StatBar playerMainHealthBar; // main health bar at the bottom of the screen
	public StatBar playerMainShieldBar; // main shield bar for player

	[SerializeField, PropertySpace(20)]
	public GameObject floatingHealthBarPrefab;

	[SerializeField, PropertySpace(20)]
	public GameObject floatingShieldBarPrefab;

	public GameObject canvas;

	private ShipEntity playerShip;
	//private List<MovingStatBar> movingStatBars; 

	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingStatBars; // stat bars that follow ship entities

	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingShieldStatBars;
	protected override void Awake()
	{
		base.Awake();

		pooledFloatingStatBars = new ObjectPool<MovingStatBar>(floatingHealthBarPrefab, transform);
		pooledFloatingShieldStatBars = new ObjectPool<MovingStatBar>(floatingShieldBarPrefab, transform);
		// todo: automatically add health bar to new ships, too.

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
			AddFloatingShieldBarToShip(ship);
		}
	}

	private void Update()
	{
		// setup main health bar for player
		if (playerShip == null && PlayerController.Instance != null) {
			playerShip = PlayerController.Instance.ShipEntity;

			playerMainHealthBar.EntityStats = playerShip.Health;

			playerMainShieldBar.EntityStats = playerShip.Shield;

		}

		updateStatBarPool(pooledFloatingStatBars);
		updateStatBarPool(pooledFloatingShieldStatBars);

	}

	public void AddFloatingHealthBarToShip(ShipEntity ship)
	{
		var healthBar = pooledFloatingStatBars.GetPooledObject();
		healthBar.Initalize(ship, ship.Health);
		healthBar.transform.SetParent(canvas.transform);
	}

	public void AddFloatingShieldBarToShip(ShipEntity ship)
	{
		var shieldBar = pooledFloatingShieldStatBars.GetPooledObject();
		shieldBar.Initalize(ship, ship.Shield);
		shieldBar.transform.SetParent(canvas.transform);
	}

	private void updateStatBarPool(ObjectPool<MovingStatBar> toUpdate) {
		//foreach (var statBar in toUpdate.ActiveObjects)
		for (int i=0; i<toUpdate.ActiveObjects.Count; i++)
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
