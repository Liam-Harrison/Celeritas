using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI.Inventory
{
	/// <summary>
	/// Manages a list of inventory UI items and simplifies accessing and manipulating them.
	/// </summary>
	public class InventoryUI : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private Transform parent;

		[SerializeField, Title("Prefabs")]
		private GameObject inventoryItem;

		private readonly List<InventoryItemUI> items = new List<InventoryItemUI>();

		/// <summary>
		/// All the inventory items in this UI.
		/// </summary>
		public IReadOnlyList<InventoryItemUI> Items { get => items.AsReadOnly(); }

		private void OnEnable()
		{
			var player = PlayerController.Instance.PlayerShipEntity;
			player.HullManager.OnModuleEquipped += OnModuleEquipped;
			player.HullManager.OnModuleUnequipped += OnModuleUnequipped;

			parent.DestroyAllChildren();
			items.Clear();

			foreach (var item in player.Inventory)
			{
				AddInventoryItem(item);
			}
		}

		private void OnDestroy()
		{
			if (PlayerController.Instance == null || PlayerController.Instance.PlayerShipEntity == null || PlayerController.Instance.PlayerShipEntity.HullManager == null)
				return;
			var player = PlayerController.Instance.PlayerShipEntity;
			player.HullManager.OnModuleEquipped -= OnModuleEquipped;
			player.HullManager.OnModuleUnequipped -= OnModuleUnequipped;
		}

		/// <summary>
		/// Remove the specified inventory UI items from the inventory.
		/// </summary>
		/// <param name="item">The inventory item UI to remove.</param>
		public void RemoveInventoryItem(InventoryItemUI item)
		{
			if (items.Contains(item))
			{
				items.Remove(item);
				Destroy(item.gameObject);
			}
		}

		/// <summary>
		/// Add a module to the inventory UI.
		/// </summary>
		/// <param name="module">The module to add to the UI.</param>
		public void AddInventoryItem(ModuleData module)
		{
			var ui = Instantiate(inventoryItem, parent).GetComponent<InventoryItemUI>();
			ui.Module = module;

			items.Add(ui);
			if (PlayerController.Instance.PlayerShipEntity.HullManager.equippedModules.Contains(module))
				ui.SetUpgradable(true);
			else
				ui.SetUpgradable(false);
			SortList();
		}

		public void OnModuleEquipped(ModuleData module)
		{
			foreach (InventoryItemUI ui in items)
				if (ui.Module.name == module.name)
					ui.SetUpgradable(true);
		}

		public void OnModuleUnequipped(ModuleData module)
		{
			foreach (InventoryItemUI ui in items)
				if (ui.Module.name == module.name)
				{ 
					ui.SetUpgradable(false);
				}
		}

		private void SortList()
		{
			items.Sort((x, y) => x.Module.Title.CompareTo(y.Module.Title)); // sort list alphabetically
			List<ModuleData> equippedModules = PlayerController.Instance.PlayerShipEntity.HullManager.equippedModules;

			foreach (InventoryItemUI item in items)
			{
				item.transform.SetAsLastSibling();
			}
		}

	}
}
