using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.Tooltips;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Celeritas.UI.Inventory
{
	/// <summary>
	/// A container for managing a module within the inventory UI.
	/// </summary>
	public class InventoryItemUI : MonoBehaviour, IPointerDownHandler, ITooltip
	{
		[SerializeField, Title("Assignments")]
		private Image image;

		[SerializeField]
		private Image border;

		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private TextMeshProUGUI subtitle;

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
				border.sprite = GameDataManager.Instance.GetBorderSprite(module.Rarity);
				title.text = module.Title;

				if (module.EntityInstance is ModuleEntity entity)
					subtitle.text = $"{module.ModuleCatagory} - {module.ModuleSize} - Level {entity.Level}";
			}
		}

		public ModuleEntity TooltipEntity => (ModuleEntity) module.EntityInstance;

		public void OnPointerDown(PointerEventData _)
		{
			hud.OnItemDragBegin(this);
		}
	}
}
