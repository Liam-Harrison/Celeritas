using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Celeritas.UI.General;
using System.Collections.Generic;

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

		//[SerializeField, TitleGroup("Ship Stats Line prefab")]
		//GameObject shipStatsLinePrefab;

		[SerializeField, TitleGroup("Ship Stats")]
		TextMeshProUGUI shipStatsText;

		[SerializeField, TitleGroup("Ship Stats")]
		Slider shipHealthSlider;

		private Dictionary<string, int> maxStats;

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

			maxStats = new Dictionary<string, int>();
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

				// if ship has any max stats, record them for sliders later
				if (!maxStats.ContainsKey("health") || maxStats["health"] < ship.StartingHealth)
					maxStats.Add("health", (int)ship.StartingHealth);
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

		private void SetupShipStatsText(ShipData ship)
		{
			string toWrite = "";
			toWrite += $"Health: {ship.StartingHealth}";
			shipStatsText.text = toWrite;
			shipHealthSlider.maxValue = maxStats["health"];
			shipHealthSlider.value = ship.StartingHealth;
		}
	}
}