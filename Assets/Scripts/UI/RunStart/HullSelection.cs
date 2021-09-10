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

		[SerializeField, TitleGroup("Ship Stats")]
		private int verticalSpacingBetweenElements;

		[SerializeField, TitleGroup("Ship Stats")]
		private GameObject weaponCountIcon;

		[SerializeField, TitleGroup("Ship Stats")]
		private int maxNumberOfWeaponSlots;

		[SerializeField, TitleGroup("Ship Stats")]
		private Gradient weaponIconGradient;

		[SerializeField, TitleGroup("Hull Preview")]
		private GridLayoutGroup hullPreviewGridLayout;

		[SerializeField, TitleGroup("Hull Preview")]
		private Image hullSectionImage; // for use in hull preview layout

		[SerializeField, TitleGroup("Hull Preview")]
		private int maxHullDimension = 10;

		private Dictionary<string, float> maxStats;

		private List<ShipSelectionStats> statLines;

		public ShipSelection ShipSelection { get; private set; }

		public ShipClass ShipClass { get; private set; }

		private int numberOfModuleSlots; // in currently selected ship.

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
			setupHullUI();
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
				if (ship.IsPlaceholder)
					continue;

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

			setupHullLayoutPreview();
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

			// unsure how to count for weapon slots >>
			// add slider for weapon slots ? wont' have room with icons.
		}

		private Image[] weaponIcons;

		/// <summary>
		/// Setup 'stats' section of the UI for the currently selected ship
		/// (ie, fill them with the currently selected ship's values)
		/// </summary>
		/// <param name="ship">currently selected ship</param>
		private void SetupShipStatsText(ShipData ship)
		{
			// module slots
			statLines[0].title.text = $"Module Slots: {numberOfModuleSlots}";
			statLines[0].hideSlider();

			// weapons count
			//statLines[1].title.text = $"Weapon Slots: {ShipSelection.CurrentShip.WeaponEntities.Count}";
			statLines[1].title.text = $"Weapon Slots: ";
			statLines[1].hideSlider();

			// health
			statLines[2].setTitle($"Health: ({ship.StartingHealth/1000}k)");
			statLines[2].slider.maxValue = maxStats["health"];
			statLines[2].setSliderValue(ship.StartingHealth);

			// shield
			statLines[3].setTitle($"Shield: ({ship.StartingShield/1000}k)");
			statLines[3].slider.maxValue = maxStats["shield"];
			statLines[3].setSliderValue(ship.StartingShield);

			// weight
			statLines[4].setTitle($"Weight: ({ship.MovementSettings.mass})");
			statLines[4].slider.maxValue = maxStats["weight"];
			statLines[4].setSliderValue(ship.MovementSettings.mass);

			// speed
			statLines[5].setTitle($"Speed: ");
			statLines[5].slider.maxValue = maxStats["speed"];
			statLines[5].setSliderValue(ship.MovementSettings.forcePerSec / ship.MovementSettings.mass);

			// speed (turning)
			statLines[6].setTitle($"Turning Speed: ");
			statLines[6].slider.maxValue = maxStats["torque"];
			statLines[6].setSliderValue(ship.MovementSettings.torquePerSec.magnitude / ship.MovementSettings.mass);

			for (int i = 0; i < maxNumberOfWeaponSlots; i++)
			{
				if (i < ShipSelection.CurrentShip.WeaponEntities.Count)
				{
					//weaponIcons[i].color = weaponIconGradient.Evaluate((float)i / maxNumberOfWeaponSlots);
					weaponIcons[i].color = weaponIconGradient.Evaluate((float)ShipSelection.CurrentShip.WeaponEntities.Count / maxNumberOfWeaponSlots);
				}
				else
					weaponIcons[i].color = Color.clear;
			}
		}

		/// <summary>
		/// Setup the lines in the UI that show the ship stat value + a bar
		/// </summary>
		private void setupStatUI()
		{
			statLines = new List<ShipSelectionStats>();
			var lastLine = shipStatsLinePrefab;
			int numberOfLines = 7;
			statLines.Add(lastLine.GetComponent<ShipSelectionStats>());

			for(int i = 0; i < numberOfLines - 1; i++) { // -1 as one line already exists 

				//var currentLine = Instantiate(lastLine, lastLine.transform);
				var currentLine = Instantiate(lastLine, lastLine.transform.parent);
				currentLine.transform.position += new Vector3(0, - verticalSpacingBetweenElements * (i+1), 0); // 16, 0
				ShipSelectionStats stats = currentLine.GetComponent<ShipSelectionStats>();
				currentLine.SetActive(true);
				statLines.Add(stats);

				//if (i == 0)
				//	lastLine.SetActive(false);
				lastLine = currentLine;

			}

			if (weaponIcons == null)
			{
				weaponIcons = new Image[maxNumberOfWeaponSlots];
				for (int i = 0; i < maxNumberOfWeaponSlots; i++)
				{
					var icon = Instantiate(weaponCountIcon, statLines[1].transform); // don't worry about position yet
					icon.transform.position += new Vector3(30 * i - 50, 5, 0);
					weaponIcons[i] = icon.GetComponentInChildren<Image>();
					weaponIcons[i].color = Color.clear;
					// make them all clear
				}
			}
		}

		private Image[,] hullPreviewImages;

		/// <summary>
		/// Use on initial setup or if the maxHullDimension of ship is less than that of the current ship's.
		/// </summary>
		private void setupHullUI()
		{
			if (hullPreviewImages == null)
				hullPreviewImages = new Image[10,10];

			for (int i = 0; i < maxHullDimension; i++)
			{
				for (int j = 0; j < maxHullDimension; j++)
				{
					hullPreviewImages[i, j] = Instantiate(hullSectionImage, hullPreviewGridLayout.gameObject.transform);
					hullPreviewImages[i, j].color = Color.clear;
				}
			}
		}

		/// <summary>
		/// Display the currently selected ship's hull layout in the UI
		/// </summary>
		private void setupHullLayoutPreview()
		{
			// try to print out hull layout in debug
			// retrieves the 'selected ship's data from ShipSelection
			bool[,] hullLayout = ShipSelection.CurrentShip.HullManager.HullData.HullLayout;
			int xMax = hullLayout.GetUpperBound(0);
			int yMax = hullLayout.GetUpperBound(1);
			numberOfModuleSlots = 0;

			// resize grid depending on how large it appears to be.
			if (yMax >= maxHullDimension - 1)
			{
				hullPreviewGridLayout.cellSize = new Vector2(15, 15);
			}
			else if (yMax >= maxHullDimension - 2)
			{
				hullPreviewGridLayout.cellSize = new Vector2(20, 20);
			}
			else
			{ 
				hullPreviewGridLayout.cellSize = new Vector2(25, 25);
			}

			// colour 'hull' cells.
			for (int i = 0; i < maxHullDimension; i++)
			{
				for (int j = 0; j < maxHullDimension; j++)
				{
					if (i < xMax && j < yMax)
					{
						hullPreviewImages[i, j].gameObject.SetActive(true);
						if (hullLayout[i, j])
						{
							hullPreviewImages[i, j].color = Color.white;
							numberOfModuleSlots++;
						}
						else
						{
							hullPreviewImages[i, j].color = Color.clear;
						}
					}
					else
					{ // trim any usued (late) columns / rows
						hullPreviewImages[i, j].gameObject.SetActive(false);
						hullPreviewImages[i, j].color = Color.clear;
					}
				}
			}

		}

	}
}