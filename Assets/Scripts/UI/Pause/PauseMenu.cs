using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.UI
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private GameObject blocker;

		private float timescale;

		bool pressed = false;

		private void LateUpdate()
		{
			if (Keyboard.current.escapeKey.isPressed && !pressed)
			{
				pressed = true;
				if (blocker.activeInHierarchy)
					Hide();
				else
					Show();
			}
			else
			{
				pressed = false;
			}
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
	}
}