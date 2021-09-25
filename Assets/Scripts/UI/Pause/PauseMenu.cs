using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	public class PauseMenu : MonoBehaviour, InputActions.INavigationActions
	{
		private InputActions.NavigationActions actions = default;

		[SerializeField, TitleGroup("Assignments")]
		private GameObject blocker;

		private float timescale;

		private void Start()
		{
			actions = new InputActions.NavigationActions(SettingsManager.InputActions);
			actions.SetCallbacks(this);
			actions.Enable();
		}

		public void Show()
		{
			blocker.SetActive(true);
			CombatHUD.Instance.SetGameCursor(false);

			timescale = Time.timeScale;
			Time.timeScale = 0;
		}

		public void Hide()
		{
			blocker.SetActive(false);
			CombatHUD.Instance.SetGameCursor(true);
			Time.timeScale = timescale;
		}

		public void OnNavigateUI(InputAction.CallbackContext context)
		{
			// Unused.
		}

		public void OnPauseMenu(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				if (blocker.activeInHierarchy)
					Hide();
				else
					Show();
			}
		}
	}
}