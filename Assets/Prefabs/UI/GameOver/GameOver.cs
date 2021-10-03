using Celeritas.Game.Controllers;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Celeritas.UI.General
{
	public class GameOver : MonoBehaviour
	{
		[SerializeField, TitleGroup("Settings")]
		private float waitTime = 3.0f;

		[SerializeField, TitleGroup("Settings")]
		private float fadeTime = 5.0f;

		[SerializeField, TitleGroup("Settings")]
		private GameObject combatHud;

		[SerializeField, TitleGroup("Settings")]
		private GameObject background;

		private float desiredAlpha = 1.0f;

		private CanvasGroup canvas;

		private void OnEnable()
		{
			PlayerController.OnPlayerShipKilled += ShowGameOver;
		}

		private void OnDisable()
		{
			PlayerController.OnPlayerShipKilled -= ShowGameOver;
		}

		public void ShowGameOver()
		{
			combatHud.SetActive(false);
			canvas = GetComponent<CanvasGroup>();

			StartCoroutine(WaitTimer(waitTime));
		}

		/// <summary>
		/// Coroutine that will wait slow time down then reveal the menu
		/// </summary>
		public IEnumerator WaitTimer(float duration)
		{
			yield return new WaitForSeconds(duration);

			background.SetActive(true);
			var starta = canvas.alpha;
			var start = Time.unscaledTime;
			float p;
			do
			{
				p = Mathf.Clamp01((Time.unscaledTime - start) / fadeTime);
				Time.timeScale = 1 - p;
				canvas.alpha = Mathf.Lerp(starta, desiredAlpha, p);
				yield return null;
			} while (p < 1);

			canvas.alpha = desiredAlpha;
		}
	}
}