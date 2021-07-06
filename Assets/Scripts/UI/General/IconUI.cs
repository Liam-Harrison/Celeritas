using Celeritas.Game.Interfaces;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Celeritas.Game;

namespace Celeritas.UI.General
{
	public class IconUI : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private Image background;

		[SerializeField]
		private Image icon;

		private IGameUI item;

		public IGameUI Item
		{
			get
			{
				return item;
			}
			set
			{
				item = value;
				SetItem(item);
			}
		}

		private void SetItem(IGameUI item)
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