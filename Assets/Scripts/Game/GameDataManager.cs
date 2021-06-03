using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Simplifies the data access of persistent, non-loaded items. Such as sprites.
	/// </summary>
	public class GameDataManager : Singleton<GameDataManager>
	{
		[SerializeField, FoldoutGroup("Rarity Portraits")]
		private Sprite common;

		[SerializeField, FoldoutGroup("Rarity Portraits")]
		private Sprite uncommon;

		[SerializeField, FoldoutGroup("Rarity Portraits")]
		private Sprite rare;

		[SerializeField, FoldoutGroup("Rarity Portraits")]
		private Sprite epic;

		[SerializeField, FoldoutGroup("Rarity Portraits")]
		private Sprite legendary;

		/// <summary>
		/// The sprite border for common items.
		/// </summary>
		public Sprite CommonBorder { get => common; }

		/// <summary>
		/// The sprite border for uncommon items.
		/// </summary>
		public Sprite UncommonBorder { get => uncommon; }

		/// <summary>
		/// The sprite border for rare items.
		/// </summary>
		public Sprite RareBorder { get => rare; }

		/// <summary>
		/// The sprite border for epic items.
		/// </summary>
		public Sprite EpicBorder { get => epic; }

		/// <summary>
		/// The sprite border for legendary items.
		/// </summary>
		public Sprite LegendaryBorder { get => legendary; }

		/// <summary>
		/// Get the border sprite for a given rarity.
		/// </summary>
		/// <param name="rarity">The rarity.</param>
		/// <returns>The sprite which belongs to the given rarity.</returns>
		public Sprite GetBorderSprite(Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common:
					return CommonBorder;
				case Rarity.Uncommon:
					return UncommonBorder;
				case Rarity.Rare:
					return RareBorder;
				case Rarity.Epic:
					return EpicBorder;
				case Rarity.Legendary:
					return LegendaryBorder;
				default:
					return null;
			}
		}
	}
}