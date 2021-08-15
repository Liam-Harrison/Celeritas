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

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite single;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite smallL;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite largeL;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite smallLine;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite largeLine;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite t;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite smallSquare;

		[SerializeField, FoldoutGroup("Tetris")]
		private Sprite largeSquare;

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
					return common;
				case Rarity.Uncommon:
					return uncommon;
				case Rarity.Rare:
					return rare;
				case Rarity.Epic:
					return epic;
				case Rarity.Legendary:
					return legendary;
				default:
					return null;
			}
		}

		/// <summary>
		/// Get the sprite for a tetris shape.
		/// </summary>
		/// <param name="shape">The shape to get.</param>
		/// <returns>Returns the sprite which belongs to the given shape.</returns>
		public Sprite GetTetrisSprite(TetrisShape shape)
		{
			switch (shape)
			{
				case TetrisShape.Single:
					return single;
				case TetrisShape.SmallL:
					return smallL;
				case TetrisShape.LargeL:
					return largeL;
				case TetrisShape.SmallLine:
					return smallLine;
				case TetrisShape.LargeLine:
					return largeL;
				case TetrisShape.T:
					return t;
				case TetrisShape.SmallSquare:
					return smallSquare;
				case TetrisShape.LargeSquare:
					return largeSquare;
				default:
					return null;
			}
		}
	}
}