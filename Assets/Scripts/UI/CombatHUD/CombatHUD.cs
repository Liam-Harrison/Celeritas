using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Celeritas.Game.Entities.LootController;

/// <summary>
/// Manages the majority of in-combat HUD interface.
/// Responsibilities include:
/// - displaying notifications & messages to the player via PrintNotification
/// - setting up stationary health/shield bars for player
/// - setting up & managing floating health/shield bars for all ships
/// - putting abilities into the AbilityBar (logic prone to change prior to integration w ability logic)
///	- setting up the mouse cursor
/// </summary>
public class CombatHUD : Singleton<CombatHUD>
{
	[SerializeField, Title("Assignments")]
	private Transform statbarParent;

	[SerializeField]
	private StatBar playerMainHealthBar; // main health bar at the bottom of the screen

	[SerializeField]
	private StatBar playerMainShieldBar; // main shield bar for player

	/// <summary>
	/// 'floating' bars are the bars that follow ships around.
	/// </summary>
	[SerializeField]
	private GameObject floatingHealthBarPrefab;

	[SerializeField]
	private GameObject floatingShieldBarPrefab;

	// stat bars that follow ship entities
	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingHealthStatBars; 

	[SerializeField]
	private ObjectPool<MovingStatBar> pooledFloatingShieldStatBars;

	[SerializeField]
	private AbilityBar abilityBar;

	[SerializeField]
	public GameObject TractorAimingLine;

	// to display how many rare metals the player has
	[SerializeField]
	private TextMeshProUGUI rareMetalsCountText;

	// to display how many modules the player has
	[SerializeField]
	private TextMeshProUGUI moduleCountText;

	// just used for dummy ability display right now
	[SerializeField]
	private Sprite defaultAbilityIcon;

	[SerializeField]
	private Texture2D mouseTexture;

	[SerializeField]
	private GameObject floatingNotificationPrefab;

	private ShipEntity playerShip;

	public AbilityBar AbilityBar { get => abilityBar; }

	protected override void Awake()
	{
		pooledFloatingHealthStatBars = new ObjectPool<MovingStatBar>(floatingHealthBarPrefab, transform);
		pooledFloatingShieldStatBars = new ObjectPool<MovingStatBar>(floatingShieldBarPrefab, transform);

		// trigger this class's 'OnCreatedEntity' when that event occurs in EntityDataManager
		EntityDataManager.OnCreatedEntity += OnCreatedEntity;

		Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);

		TractorAimingLine.SetActive(false);

		base.Awake();
	}

	/// <summary>
	/// Display a notification to the player, via a temporary message tha displays above their ship.
	/// </summary>
	/// <param name="message"></param>
	public void PrintNotification(string message)
	{
		GameObject toPrint = Instantiate(floatingNotificationPrefab, statbarParent.transform);
		toPrint.transform.SetParent(statbarParent.transform);
		toPrint.GetComponent<TextMeshProUGUI>().text = message;
	}

	public void UpdateLootCount(LootType type, int amount)
	{
		if (type == LootType.Module)
			moduleCountText.text = amount.ToString();
		if (type == LootType.RareMetal)
			rareMetalsCountText.text = amount.ToString();
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

	/// <summary>
	/// Add a health bar to the provided ship, that will follow it around closely
	/// </summary>
	/// <param name="ship">The ship the bar will follow & whose healthbar the bar will show</param>
	private void AddFloatingHealthBarToShip(ShipEntity ship)
	{
		var healthBar = pooledFloatingHealthStatBars.GetPooledObject();
		healthBar.Initalize(ship, ship.Health);
		healthBar.transform.SetParent(statbarParent);
	}

	private void AddFloatingShieldBarToShip(ShipEntity ship)
	{
		var shieldBar = pooledFloatingShieldStatBars.GetPooledObject();
		shieldBar.Initalize(ship, ship.Shield);
		shieldBar.transform.SetParent(statbarParent);
	}

	private void UpdateStatBarPool(ObjectPool<MovingStatBar> toUpdate) {
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
