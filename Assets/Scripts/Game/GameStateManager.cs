using System;

namespace Celeritas.Game
{
	/// <summary>
	/// States that match the names of actions inside the animator.
	/// </summary>
	public enum GameState
	{
		BUILD,
		PLAY,
		WAVE,
		BOSS,
		SPACE_STATION
	}

	public class GameStateManager : Singleton<GameStateManager>
	{
		public GameState GameState { get; private set; } = GameState.PLAY;

		public static event Action<GameState> onStateChanged;

		public void SetGameState(GameState state)
		{
			GameState = state;
			onStateChanged?.Invoke(state);
		}
	}
}
