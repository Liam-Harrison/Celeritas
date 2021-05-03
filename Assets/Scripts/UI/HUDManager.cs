using Celeritas.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.UI
{
	public class HUDManager : Singleton<HUDManager>
	{
		[SerializeField, Title("Assignments")]
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

#if UNITY_EDITOR
		[ButtonGroup]
		private void ShowPlayHud()
		{
			OnGameStateChanged(GameState.PLAY);
		}

		[ButtonGroup]
		private void ShowBuildHud()
		{
			OnGameStateChanged(GameState.BUILD);
		}
#endif
	}
}
