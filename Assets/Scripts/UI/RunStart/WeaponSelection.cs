using Celeritas.Game.Entities;
using Celeritas.UI.WeaponSelection;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI.Runstart
{
	public class WeaponSelection : MonoBehaviour
	{
		[SerializeField, TitleGroup("Ship")]
		private Vector3 rotation;

		[SerializeField, TitleGroup("Panel Assignments")]
		private GameObject panelPrefab;

		[SerializeField, TitleGroup("Panel Assignments")]
		private Transform leftGrid;

		[SerializeField, TitleGroup("Panel Assignments")]
		private Transform rightGrid;

		[SerializeField, TitleGroup("Item Assignments")]
		private GameObject itemPrefab;

		[SerializeField, TitleGroup("Item Assignments")]
		private Transform itemContent;

		private readonly List<WeaponPanel> panels = new List<WeaponPanel>();

		public ShipSelection ShipSelection { get; private set; }

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

			foreach (var panel in panels)
			{
				Destroy(panel.gameObject);
			}
			panels.Clear();

			foreach (var weapon in ShipSelection.CurrentShip.WeaponEntities)
			{
				var view = Camera.main.WorldToViewportPoint(Quaternion.Inverse(weapon.transform.rotation) * weapon.transform.position);
				var parent = view.x > 0.5f ? rightGrid : leftGrid;

				var panel = Instantiate(panelPrefab, parent).GetComponent<WeaponPanel>();
				panel.SetModule(weapon.AttatchedModule);
				panel.SetWeapon(weapon.WeaponData);

				panels.Add(panel);
			}
		}

		private void SetupUI()
		{
			EntityDataManager.OnLoadedAssets -= SetupUI;

			foreach (var weapon in EntityDataManager.Instance.Weapons)
			{
				var panel = Instantiate(itemPrefab, itemContent).GetComponent<WeaponItem>();
				panel.SetWeapon(weapon);
			}
		}
	}
}