using UnityEngine;

namespace Celeritas.Game.Interfaces
{
	/// <summary>
	/// Exposes common information for game icons.
	/// </summary>
	public interface IGameUI
	{
		/// <summary>
		/// The title of this icon.
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// The icon of this item.
		/// </summary>
		Sprite Icon { get; }

		/// <summary>
		/// The rarity of this item.
		/// </summary>
		Rarity Rarity { get; }

		/// <summary>
		/// The description attatched to this item.
		/// </summary>
		string Description { get; }
	}
}
