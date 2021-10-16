using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class PressAnyKey : MonoBehaviour
	{
		public Button button;
		public CanvasGroup canvas;

		void Update()
		{
			if (Keyboard.current.anyKey.wasPressedThisFrame)
			{
				button.onClick.Invoke();
			}
			if (canvas.alpha > 0)
			{
				enabled = false;
			}
		}
	}
}