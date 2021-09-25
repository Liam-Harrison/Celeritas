using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Celeritas.UI.General
{
	public class GameOverScript : MonoBehaviour
	{
		[SerializeField, TitleGroup("Wait before menu", "The amount of time between when the player dies and the menu appears.")]
		private float waitTime = 3.0f;

		[SerializeField, TitleGroup("Fade Speed", "The speed in which the menu will fade in")]
		private float fadeSpeed = 2.0f;

		[SerializeField, TitleGroup("Restart Button")]
		private GameObject restartButton;

		[SerializeField, TitleGroup("Exit Button")]
		private GameObject exitButton;

		private float desiredAlpha = 1.0f;

		private float currentAlpha = 0.0f;

		/// <summary>
		/// Determines if the Canvas will fade in or not.
		/// </summary>
		private bool fade = false;

		private CanvasGroup canvas;

		public void GameOver()
		{
			GameObject gameUI = GameObject.Find("hud_game_main");

			if (gameUI != null)
			{
				// Disables game hud
				gameUI.SetActive(false);
			}
			canvas = GetComponent<CanvasGroup>();

			StartCoroutine(WaitTimer(waitTime));
		}

		/// <summary>
		/// Coroutine that will wait slow time down then reveal the menu
		/// </summary>
		public IEnumerator WaitTimer(float duration)
		{
			Time.timeScale = 0.5f;
			yield return new WaitForSeconds(duration);
			Time.timeScale = 1.0f;

			fade = true;
		}

		void Update()
		{
			if (fade == true)
			{
				if (currentAlpha >= desiredAlpha)
				{
					fade = false;
					canvas.interactable = true;

					if (restartButton)
					{
						restartButton.SetActive(true);
					}

					if (exitButton)
					{
						exitButton.SetActive(true);
					}

				}
				else
				{
					canvas.alpha = currentAlpha;
					currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, fadeSpeed * Time.deltaTime);
				}
			}
		}
	}
}