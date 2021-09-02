using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Celeritas.UI.General;
using System.Collections.Generic;
using Assets.Scripts.UI.RunStart;

namespace Celeritas.UI.Runstart
{
	/// <summary>
	/// Simplifies the management of the runstart UI responsible for class and hull selection.
	/// </summary>
	public class HullSelection : MonoBehaviour
	{
		[SerializeField, TitleGroup("Ship")]
		private Vector3 rotation;

		[SerializeField, TitleGroup("Class Selection")]
		private Toggle corvetteToggle;

		[SerializeField, TitleGroup("Class Selection")]
		private Toggle destroyerToggle;

		[SerializeField, TitleGroup("Class Selection")]
		private Toggle battleshipToggle;

		[SerializeField, TitleGroup("Hull Selection")]
		private GameObject togglePrefab;

		[SerializeField, TitleGroup("Hull Selection")]
		private ToggleGroup hullGroup;

		[SerializeField, TitleGroup("Hull Selection")]
		private RectTransform hullParent;

		[SerializeField, TitleGroup("Info Panel")]
		private TextMeshProUGUI shipTitle;

		[SerializeField, TitleGroup("Info Panel")]
		private TextMeshProUGUI shipDescription;

		[SerializeField, TitleGroup("Info Panel")]
		private LineUI lineUI;

		[SerializeField, TitleGroup("Ship Stats")]
		GameObject shipStatsLinePrefab;

		private Dictionary<string, float> maxStats;

		private List<ShipSelectionStats> statLines;

		public ShipSelection ShipSelection { get; private set; }

		public ShipClass ShipClass { get; private set; }

		private void Awake()
		{
			ShipSelection = FindObjectOfType<ShipSelection>();

			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
			{
				SetupUI();
			}
			else
				EntityDataManager.OnLoadedAssets += SetupUI;
		}

		private void OnEnable()
		{
			ShipSelection.RotateOrigin(rotation);
		}

		private void SetupUI()
		{
			EntityDataManager.OnLoadedAssets -= SetupUI;

			corvetteToggle.interactable = EntityDataManager.Instance.PlayerShips.Where((a) => a.ShipClass == ShipClass.Corvette).Count() > 0;
			destroyerToggle.interactable = EntityDataManager.Instance.PlayerShips.Where((a) => a.ShipClass == ShipClass.Destroyer).Count() > 0;
			battleshipToggle.interactable = EntityDataManager.Instance.PlayerShips.Where((a) => a.ShipClass == ShipClass.Dreadnought).Count() > 0;

			corvetteToggle.onValueChanged.AddListener((b) => { if (b) LoadClassHulls(ShipClass.Corvette); });
			destroyerToggle.onValueChanged.AddListener((b) => { if (b) LoadClassHulls(ShipClass.Destroyer); });
			battleshipToggle.onValueChanged.AddListener((b) => { if (b) LoadClassHulls(ShipClass.Dreadnought); });

			setupStatUI();
			maxStats = new Dictionary<string, float>();
			LoadClassHulls(ShipClass.Destroyer);
			LoadClassHulls(ShipClass.Dreadnought);
			LoadClassHulls(ShipClass.Corvette);
		}

		private void LoadClassHulls(ShipClass shipClass)
		{
			var ships = EntityDataManager.Instance.PlayerShips.Where((a) => a.ShipClass == shipClass);

			hullParent.DestroyAllChildren();

			bool added = false;
			foreach (var ship in ships)
			{
				var toggle = Instantiate(togglePrefab, hullParent).GetComponent<Toggle>();
				toggle.GetComponentInChildren<TextMeshProUGUI>().text = ship.Title;
				toggle.group = hullGroup;

				if (!added)
				{
					added = true;
					toggle.isOn = true;
				}

				toggle.onValueChanged.AddListener((b) => { if (b) SelectHull(ship); });

				checkShipForMaxStats(ship);
			}

			SelectHull(ships.First());
		}

		private void SelectHull(ShipData ship)
		{
			ShipSelection.SelectShip(ship);

			shipTitle.text = ship.Title;
			shipDescription.text = ship.Description;

			lineUI.WorldTarget = ShipSelection.CurrentShip.transform;

			SetupShipStatsText(ship);
		}

		/// <summary>
		/// Use when loading ships. Checks the passed ship for any max stats, if any exist, will record them in 'maxStats'
		/// </summary>
		/// <param name="ship">ship to check</param>
		private void checkShipForMaxStats(ShipData ship)
		{
			// if ship has any max stats, record them for sliders later
			if (!maxStats.ContainsKey("health") || maxStats["health"] < ship.StartingHealth)
				maxStats["health"] = ship.StartingHealth;

			if (!maxStats.ContainsKey("shield") || maxStats["shield"] < ship.StartingShield)
				maxStats["shield"] = ship.StartingShield;

			if (!maxStats.ContainsKey("weight") || maxStats["weight"] < ship.MovementSettings.mass)
				maxStats["weight"] = ship.MovementSettings.mass;

			if (!maxStats.ContainsKey("speed") || maxStats["speed"] < ship.MovementSettings.forcePerSec / ship.MovementSettings.mass)
				maxStats["speed"] = ship.MovementSettings.forcePerSec / ship.MovementSettings.mass;

			if (!maxStats.ContainsKey("torque") || maxStats["torque"] < ship.MovementSettings.torquePerSec.magnitude / ship.MovementSettings.mass)
				maxStats["torque"] = ship.MovementSettings.torquePerSec.magnitude / ship.MovementSettings.mass;

			// no slider for weapon count so don't need max
		}

		/// <summary>
		/// Setup 'stats' section of the UI for the currently selected ship
		/// (ie, fill them with the currently selected ship's values)
		/// </summary>
		/// <param name="ship">currently selected ship</param>
		private void SetupShipStatsText(ShipData ship)
		{
			// weapons count
			statLines[0].title.text = $"Number Of Weapons: \t{ShipSelection.CurrentShip.WeaponEntities.Count}";
			statLines[0].hideSlider();

			// health
			statLines[1].title.text = $"Health: ({ship.StartingHealth})";
			statLines[1].setSliderMaxValue(maxStats["health"]);
			statLines[1].setSliderValue(ship.StartingHealth);

			// shield
			statLines[2].title.text = $"Shield: ({ship.StartingShield})";
			statLines[2].slider.maxValue = maxStats["shield"];
			statLines[2].setSliderValue(ship.StartingShield);

			// weight
			statLines[3].title.text = $"Weight: ({ship.MovementSettings.mass})";
			statLines[3].slider.maxValue = maxStats["weight"];
			statLines[3].setSliderValue(ship.MovementSettings.mass);

			// speed
			statLines[4].title.text = $"Speed: ";
			statLines[4].slider.maxValue = maxStats["speed"];
			statLines[4].setSliderValue(ship.MovementSettings.forcePerSec / ship.MovementSettings.mass);

			// speed (turning)
			statLines[5].title.text = $"Turning Speed: ";
			statLines[5].slider.maxValue = maxStats["torque"];
			statLines[5].setSliderValue(ship.MovementSettings.torquePerSec.magnitude / ship.MovementSettings.mass);

		}

		/// <summary>
		/// Setup the lines in the UI that show the ship stat value + a bar
		/// </summary>
		private void setupStatUI()
		{
			statLines = new List<ShipSelectionStats>();
			var lastLine = shipStatsLinePrefab;
			int numberOfLines = 6;
			statLines.Add(lastLine.GetComponent<ShipSelectionStats>());

			for(int i = 0; i < numberOfLines - 1; i++) { // -1 as one line already exists 

				var currentLine = Instantiate(lastLine, lastLine.transform);
				currentLine.transform.position += new Vector3(16, 0, 0);
				ShipSelectionStats stats = currentLine.GetComponent<ShipSelectionStats>();
				currentLine.SetActive(true);
				statLines.Add(stats);

				//if (i == 0)
				//	lastLine.SetActive(false);
				lastLine = currentLine;

			}
		}
	}
}