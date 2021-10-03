using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.General;
using Celeritas.UI.WeaponSelection;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI.Runstart
{
	public class WeaponSelection : MonoBehaviour
	{
		[SerializeField, TitleGroup("Ship")]
		private Vector3 rotation;

		[SerializeField, TitleGroup("Panels")]
		private GameObject panelPrefab;

		[SerializeField, TitleGroup("Panels")]
		private Transform leftGrid;

		[SerializeField, TitleGroup("Panels")]
		private Transform rightGrid;

		[SerializeField, TitleGroup("Items")]
		private GameObject itemPrefab;

		[SerializeField, TitleGroup("Items")]
		private Transform itemContent;

		[SerializeField, TitleGroup("Dragging")]
		private IconUI dragIcon;

		[SerializeField, TitleGroup("ErrorText")]
		private GameObject errorText; // used to show 'please select a weapon before launching' text

		[SerializeField, Title("LaunchButton")]
		private Button launchButton;

		private readonly List<WeaponPanel> panels = new List<WeaponPanel>();

		public ShipSelection ShipSelection { get; private set; }

		public bool Dragging { get; private set; }

		public WeaponItem DraggingItem { get; private set; }

		private WeaponPanel highlighting;

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

			var left = new List<WeaponPanel>();
			var right = new List<WeaponPanel>();

			foreach (var weapon in ShipSelection.CurrentShip.WeaponEntities)
			{
				var view = Camera.main.WorldToViewportPoint(Quaternion.Inverse(weapon.transform.rotation) * weapon.transform.position);
				var parent = view.x > 0.5f ? rightGrid : leftGrid;

				var panel = Instantiate(panelPrefab, parent).GetComponent<WeaponPanel>();
				panel.SetModule(weapon.AttatchedModule); // maybe make a placeholder weapon?
				panel.SetWeapon(weapon.WeaponData);

				panels.Add(panel);

				if (parent == leftGrid)
					left.Add(panel);
				else
					right.Add(panel);
			}

			SortGridPanel(left);
			SortGridPanel(right);
		}

		private void SortGridPanel(List<WeaponPanel> panels)
		{
			var ordered = panels.OrderBy((a) => a.Module.transform.localPosition.y);

			foreach (var panel in ordered)
			{
				panel.transform.SetAsFirstSibling();
			}
		}

		private void Update()
		{
			if (Dragging)
			{
				dragIcon.transform.position = Mouse.current.position.ReadValue();

				if (TryGetPanel(out var panel))
				{
					panel.Highlighted = true;
					highlighting = panel;
				}
				else if (highlighting != null)
				{
					highlighting.Highlighted = false;
					highlighting = null;
				}

				if (Mouse.current.leftButton.ReadValue() == 0)
					StopDrag();
			}
		}

		private void SetupUI()
		{
			EntityDataManager.OnLoadedAssets -= SetupUI;

			foreach (var weapon in EntityDataManager.Instance.Weapons)
			{
				if (weapon.Placeholder) // don't let players equip placeholder weapons
					continue;

				var panel = Instantiate(itemPrefab, itemContent).GetComponent<WeaponItem>();
				panel.SetWeapon(weapon);
			}
		}

		public void StartDrag(WeaponItem item)
		{
			dragIcon.gameObject.SetActive(true);
			dragIcon.SetItem(item.Weapon);
			Dragging = true;
			DraggingItem = item;
		}

		public void StopDrag()
		{
			if (TryGetPanel(out var panel))
			{
				panel.SetWeapon(DraggingItem.Weapon);
			}

			if (highlighting != null)
			{
				highlighting.Highlighted = false;
				highlighting = null;
			}

			dragIcon.gameObject.SetActive(false);
			Dragging = false;
			DraggingItem = null;

			// if no placeholder weapons are left, ready to launch
			bool readyToLaunch = true;
			foreach (WeaponPanel w in panels)
			{
				if (w.Weapon.Placeholder)
				{
					readyToLaunch = false;
					break;
				}
			}
			if (readyToLaunch)
			{
				// remove error message if all weapons are equipped correctly.
				errorText.SetActive(false);
				launchButton.interactable = true;
			}
		}

		private bool TryGetPanel(out WeaponPanel panel)
		{
			foreach (var item in panels)
			{
				if (item.RectTransform.rect.Contains(item.RectTransform.InverseTransformPoint(Mouse.current.position.ReadValue())))
				{
					panel = item;
					return true;
				}
			}

			panel = null;
			return false;
		}
	}
}