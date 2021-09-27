using Celeritas;
using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the majority of in-combat HUD interface.
/// Responsibilities include:
/// - displaying notifications & messages to the player via PrintNotification
/// - setting up stationary health/shield bars for player
/// - putting abilities into the AbilityBar (logic prone to change prior to integration w ability logic)
///	- setting up the mouse cursor
/// </summary>
public class CombatHUD : Singleton<CombatHUD>
{
	[SerializeField, TitleGroup("Assignments")]
	private Transform statbarParent;

	[SerializeField, TitleGroup("Assignments")]
	private StatBar playerMainHealthBar;

	[SerializeField, TitleGroup("Assignments")]
	private StatBar playerMainShieldBar;

	[SerializeField, TitleGroup("Assignments")]
	private AbilityBar abilityBar;

	[SerializeField, TitleGroup("Assignments")]
	public GameObject TractorAimingLine;

	[SerializeField, TitleGroup("Assignments")]
	private TextMeshProUGUI switchLabel;

	[SerializeField, TitleGroup("Assignments")]
	private GameObject combatTutorial;

	[SerializeField, TitleGroup("Mouse")]
	private Texture2D mouseTexture;

	[SerializeField, TitleGroup("Notifications")]
	private GameObject floatingNotificationPrefab;

	[SerializeField, TitleGroup("Floating Text")]
	private Transform floatingParent;

	[SerializeField, TitleGroup("Floating Text")]
	private GameObject floatingTextPrefab;

	private ObjectPool<FloatingText> floatingTextPool;

	private ShipEntity playerShip;

	public AbilityBar AbilityBar { get => abilityBar; }

	/// <summary>
	/// Used to apply floating text to entity
	/// </summary>
	protected override void Awake()
	{
		floatingTextPool = new ObjectPool<FloatingText>(floatingTextPrefab, floatingParent);
		EntityDataManager.OnCreatedEntity += OnCreatedEntity;

		WaveManager.OnWaveStarted += OnWaveStarted;
		WaveManager.OnWaveEnded += OnWaveEnded;

		LootController.OnModulesChanged += OnModulesChanged;
		LootController.OnRareComponentsChanged += OnRareComponentsChanged;

		TractorAimingLine.SetActive(false);

		base.Awake();
	}

	protected override void OnDestroy()
	{
		EntityDataManager.OnCreatedEntity -= OnCreatedEntity;

		WaveManager.OnWaveStarted -= OnWaveStarted;
		WaveManager.OnWaveEnded -= OnWaveEnded;

		LootController.OnModulesChanged -= OnModulesChanged;
		LootController.OnRareComponentsChanged -= OnRareComponentsChanged;

		base.OnDestroy();
	}

	private void OnEnable()
	{
		SetGameCursor(true);
	}

	private void OnDisable()
	{
		SetGameCursor(false);
	}

	public void SetGameCursor(bool value)
	{
		if (value)
		{
			Vector2 center = new Vector2(mouseTexture.width / 2f, mouseTexture.height / 2f);
			Cursor.SetCursor(mouseTexture, center, CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	private void Update()
	{
		// if just starting, link stationary stat bars to PlayerShip
		if (playerShip == null && PlayerController.Instance != null)
		{
			playerShip = PlayerController.Instance.PlayerShipEntity;

			playerMainHealthBar.EntityStats = playerShip.Health;

			playerMainShieldBar.EntityStats = playerShip.Shield;
		}

	}

	private void OnWaveStarted()
	{
		switchLabel.gameObject.SetActive(false);
	}

	private void OnWaveEnded()
	{
		switchLabel.gameObject.SetActive(true);
	}

	private void OnModulesChanged(int modules, int amount)
	{
		if (gameObject.activeInHierarchy)
			PrintNotification("+" + amount + " Modules!");
	}

	private void OnRareComponentsChanged(int components, int amount)
	{
		if (gameObject.activeInHierarchy)
			PrintNotification("+" + amount + " Rare Metals!");
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

	private void OnCreatedEntity(Entity entity)
	{

	}

	public void OnToggleTutorial()
	{
		combatTutorial.GetComponent<CombatTutorial>().ToggleTutorial();
	}

	public void PrintFloatingText(Entity entity, float damage)
	{
		if (entity.AttatchedFloatingText != null && SettingsManager.StackingDamageNumbers)
		{
			entity.AttatchedFloatingText.IncreaseNumber(damage);
		}
		else
		{
			var floating = floatingTextPool.GetPooledObject();
			floating.Initalize(entity, damage);
		}
	}
}
