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

			parent.DestroyAllChildren();
			items.Clear();

			foreach (var item in player.Inventory)
			{
				AddInventoryItem(item);
			}
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
			SortList();
		}

		private void SortList()
		{
			items.Sort((x, y) => x.Module.Title.CompareTo(y.Module.Title)); // sort list alphabetically
			foreach (InventoryItemUI item in items)
			{
				item.transform.SetAsLastSibling();
			}
		}
	}
}
