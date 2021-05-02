using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Celeritas.UI.Inventory
{
	/// <summary>
	/// A container for managing a module within the inventory UI.
	/// </summary>
	public class InventoryItemUI : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private Image image;

		[SerializeField]
		private TextMeshProUGUI label;

		private ModuleData module;

		/// <summary>
		/// Get the module attatched to this inventory item.
		/// </summary>
		public ModuleData Module
		{
			get => module;
			set
			{
				module = value;
				image.sprite = module.icon;
				label.text = module.Title;
			}
		}
	}
}
