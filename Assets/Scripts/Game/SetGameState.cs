using UnityEngine;

namespace Celeritas.Game
{
	public class SetGameState : MonoBehaviour
	{
		[SerializeField]
		private GameState gameState;

		private void Start()
		{
			GameStateManager.Instance.SetGameState(gameState);
		}
	}
}