using System;

namespace Celeritas.Game
{
	/// <summary>
	/// States that match the names of actions inside the animator.
	/// </summary>
	public enum GameState
	{
		BACKGROUND,
		BUILD,
		WAVE,
		BOSS,
		MAINMENU
	}

	public class GameStateManager : Singleton<GameStateManager>
	{
		public GameState GameState { get; private set; } = GameState.BACKGROUND;

		public static event Action<GameState, GameState> onStateChanged;

		public void SetGameState(GameState state)
		{
			if (state == GameState)
				return;

			var old = GameState;
			GameState = state;
			onStateChanged?.Invoke(old, state);
		}
	}
}
