using Celeritas.Game;
using Celeritas.Game.Actions;
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
	public class InventoryItemUI : MonoBehaviour, IPointerDownHandler, ITooltip, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
	{
		[SerializeField, TitleGroup("Assignments")]
		private Image background;

		[SerializeField, TitleGroup("Assignments")]
		private Image image;

		[SerializeField, TitleGroup("Assignments")]
		private Image border;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI title;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI subtitle;

		[SerializeField, TitleGroup("Colors")]
		private Color normal = Color.white;

		[SerializeField, TitleGroup("Colors")]
		private Color highlighted = Color.white;

		[SerializeField, TitleGroup("Colors")]
		private Color pressed = Color.white;

		private ModuleData module;

		private BuildHUD hud;

		public ModuleEntity TooltipEntity => (ModuleEntity)module.EntityInstance;

		public GameAction TooltipAction => null;

		private bool MouseOver { get; set; }

		private void Awake()
		{
			hud = GetComponentInParent<BuildHUD>();
			background.color = normal;
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

		public void OnPointerDown(PointerEventData _)
		{
			hud.OnItemDragBegin(this);
			background.color = pressed;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			MouseOver = false;
			background.color = normal;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			MouseOver = true;
			background.color = highlighted;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			background.color = MouseOver ? highlighted : normal;
		}

		private void OnValidate()
		{
			if (background != null)
				background.color = normal;
		}
	}
}
