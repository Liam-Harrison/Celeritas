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

	[SerializeField]
	private AbilityBar abilityBar;

	[SerializeField]
	private Sprite defaultAbilityIcon;

	[SerializeField]
	private Texture2D mouseTexture;

	[SerializeField]
	private GameObject floatingNotificationPrefab;

	/*
	[SerializeField]
	private Color mouseCrosshairColour;

	[SerializeField]
	private Material material; // for mouse crosshair*/

	//private AimingRecticle mouseCrosshair;

	private ShipEntity playerShip;

	protected override void Awake()
	{
		base.Awake();

		pooledFloatingHealthStatBars = new ObjectPool<MovingStatBar>(floatingHealthBarPrefab, transform);
		pooledFloatingShieldStatBars = new ObjectPool<MovingStatBar>(floatingShieldBarPrefab, transform);

		//mouseCrosshair = gameObject.AddComponent<AimingRecticle>();
		//mouseCrosshair.Material = material;
		//mouseCrosshair.Colour = mouseCrosshairColour;

		// trigger this class's 'OnCreatedEntity' when that event occurs in EntityDataManager
		EntityDataManager.OnCreatedEntity += OnCreatedEntity;

		// https://docs.unity3d.com/ScriptReference/Cursor.SetCursor.html
		Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);
	}

	/// <summary>
	/// Display a notification to the player, via a temporary message
	/// </summary>
	/// <param name="message"></param>
	public void PrintNotification(string message)
	{
		GameObject toPrint = Object.Instantiate<GameObject>(floatingNotificationPrefab, canvas.transform);
		toPrint.transform.SetParent(canvas.transform);
		toPrint.GetComponent<Text>().text = message;
	}

	private void Start()
	{
		List<DummyAbility> abilities = GenerateDummyAbilities();
		for (int i = 0; i < abilities.Count; i++)
		{
			abilityBar.AddAbility(abilities[i], i);
		}

		PrintNotification("Hello hello!! c:");
	}

	protected override void OnDestroy()
	{
		EntityDataManager.OnCreatedEntity -= OnCreatedEntity;
		base.OnDestroy();
	}

	private void OnPostRender()
	{
		//Debug.Log("boop");
		//mouseCrosshair.Draw(playerShip);
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

		//mouseCrosshair.Draw(playerShip);
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

	/// <summary>
	/// For testing HUD only
	/// </summary>
	/// <returns></returns>
	private List<DummyAbility> GenerateDummyAbilities()
	{
		List<DummyAbility> toReturn = new List<DummyAbility>();
		for (int i = 0; i < 4; i++)
		{
			DummyAbility toAdd = new DummyAbility((i+1)+"");
			toAdd.icon = defaultAbilityIcon;
			toReturn.Add(toAdd);
		}
		toReturn[1].inputButton = "shift";
		return toReturn;
	}

}
