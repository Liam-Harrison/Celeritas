using Celeritas.Game.Interfaces;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.UI.Tooltips;
using Celeritas.Game.Entities;
using Celeritas.Game.Actions;

namespace Celeritas.UI.General
{
	public class IconUI : MonoBehaviour, ITooltip
	{
		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private Image background;

		[SerializeField]
		private Image icon;

		private ModuleData item;

		public ModuleData Item
		{
			get
			{
				return item;
			}
		}

		public ModuleEntity TooltipEntity => (ModuleEntity) Item.EntityInstance;

		public GameAction TooltipAction => null;

		public void SetItem(ModuleData item)
		{
			this.item = item;

			if (title != null)
				title.text = item.Title;

			if (background != null)
				background.sprite = GameDataManager.Instance.GetBorderSprite(item.Rarity);

			if (icon != null)
				icon.sprite = item.Icon;
		}

		private void OnValidate()
		{
			if (item != null)
			{
				SetItem(item);
			}
		}
	}
}