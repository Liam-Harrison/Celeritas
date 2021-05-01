using Celeritas.Game;
using UnityEngine;

namespace Celeritas.UI
{
	public class HUDManager : Singleton<HUDManager>
	{
		[SerializeField]
		private CombatHUD combatHud;

		[SerializeField]
		private BuildHUD buildHud;

		private void Start()
		{
			OnGameStateChanged(GameStateManager.Instance.GameState);
		}

		private void OnEnable()
		{
			GameStateManager.onStateChanged += OnGameStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState state)
		{
			combatHud.gameObject.SetActive(state == GameState.PLAY);
			buildHud.gameObject.SetActive(state == GameState.BUILD);
		}
	}
}
