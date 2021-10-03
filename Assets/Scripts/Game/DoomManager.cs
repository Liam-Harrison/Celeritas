using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages doom meter / timer.
	/// </summary>
	public class DoomManager : Singleton<DoomManager>
	{
		private const int MAX_DOOM = 5;
		private const float ANIMATE_TIME = 1;

		[SerializeField, TitleGroup("Assignments")]
		public float duration = 90;

		[SerializeField, TitleGroup("Assignments")]
		protected TextMeshProUGUI timeText;

		[SerializeField, TitleGroup("Assignments")]
		protected TextMeshProUGUI levelText;

		[SerializeField, TitleGroup("Assignments")]
		protected Slider slider;

		private float elasped;

		public bool DoomEnabled { get; set; } = true;

		public int DoomLevel { get; private set; } = 0;

		void Start()
		{
			elasped = 0;

			slider.maxValue = 1;
			slider.value = 0;
		}

		void Update()
		{
			if (DoomEnabled)
			{
				elasped += Time.smoothDeltaTime;

				if (elasped >= duration)
				{
					elasped = 0;
					ChangeDoomMeter(1);
				}
			}

			UpdateTime(duration - elasped);
		}

		void UpdateTime(float timeToShow)
		{
			float minutes = Mathf.FloorToInt(timeToShow / 60);
			float seconds = Mathf.FloorToInt(timeToShow % 60);

			timeText.text = $"{minutes:0}:{seconds:00}";
		}

		public void ChangeDoomMeter(int amount)
		{
			DoomLevel = Mathf.Clamp(DoomLevel + amount, 0, MAX_DOOM);
			UpdateDoomGraphic();
		}

		private void UpdateDoomGraphic()
		{
			float p = Mathf.Clamp01(DoomLevel / (float)MAX_DOOM);
			levelText.text = DoomLevel.ToString();

			StopAllCoroutines();
			StartCoroutine(AnimateDoomMeter(p));
		}

		private IEnumerator AnimateDoomMeter(float goal)
		{
			float start = slider.value;
			float time = Time.unscaledTime;

			float p;
			do
			{
				p = Mathf.Clamp01(Time.unscaledTime - time / ANIMATE_TIME);
				slider.value = Mathf.Lerp(start, goal, p);
				yield return null;
			} while (p < 1);

			slider.value = goal;
			yield break;
		}
	}
}
