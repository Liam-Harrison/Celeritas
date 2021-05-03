using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Celeritas.UI.Inventory
{
	/// <summary>
	/// A container for managing a module within the inventory UI.
	/// </summary>
	public class InventoryItemUI : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField, Title("Assignments")]
		private Image image;

		[SerializeField]
		private TextMeshProUGUI label;

		private ModuleData module;

		private BuildHUD hud;

		private void Awake()
		{
			hud = GetComponentInParent<BuildHUD>();
		}

		/// <summary>
		/// Get the module attatched to this inventory item.
		/// </summary>
		public ModuleData Module
		{
			get => module;
			set
			{
				module = value;
				image.sprite = module.Icon;
				label.text = module.Title;
			}
		}

		public void OnPointerDown(PointerEventData _)
		{
			hud.OnItemDragBegin(this);
		}
	}
}
