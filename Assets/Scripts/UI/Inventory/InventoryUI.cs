using Celeritas.Extensions;
using Celeritas.Game.Controllers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI.Inventory
{
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
				if (item == null)
					continue;

				var ui = Instantiate(inventoryItem, parent).GetComponent<InventoryItemUI>();
				ui.Module = item;
				items.Add(ui);
			}
		}
	}
}
