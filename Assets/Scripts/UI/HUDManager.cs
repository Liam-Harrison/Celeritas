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
			OnStateChanged(GameState.MAINMENU, GameStateManager.Instance.GameState);
		}

		private void OnEnable()
		{
			GameStateManager.onStateChanged += OnStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnStateChanged;
		}

		private void OnStateChanged(GameState old, GameState state)
		{
			combatHud.gameObject.SetActive(state != GameState.BUILD);
			buildHud.gameObject.SetActive(state == GameState.BUILD);
		}
	}
}
