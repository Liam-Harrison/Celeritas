using Celeritas.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.Game
{
	public class DoomManager : Singleton<DoomManager>
	{
		public const int MAX_DOOM = 10;

		private const float ANIMATE_TIME = 1;

		[SerializeField]
		private Image fill;

		public int DoomMeter { get; private set; }

		[ConsoleCommand]
		public static void SetDoom(int amount)
		{
			Instance.SetDoomMeterLevel(amount);
		}

		public void SetDoomMeterLevel(int level)
		{
			DoomMeter = Mathf.Clamp(level, 0, MAX_DOOM);
			UpdateDoomGraphic();
		}

		public void ChangeDoomMeter(int amount)
		{
			DoomMeter = Mathf.Clamp(DoomMeter + amount, 0, MAX_DOOM);
			UpdateDoomGraphic();
		}

		private void UpdateDoomGraphic()
		{
			float p = Mathf.Clamp01(DoomMeter / (float)MAX_DOOM);

			StopAllCoroutines();
			StartCoroutine(AnimateDoomMeter(p));
		}

		private IEnumerator AnimateDoomMeter(float goal)
		{
			float start = fill.fillAmount;
			float time = Time.unscaledTime;

			float p;
			do
			{
				p = Mathf.Clamp01(Time.unscaledTime - time / ANIMATE_TIME);
				fill.fillAmount = Mathf.Lerp(start, goal, p);
				yield return null;
			} while (p < 1);

			fill.fillAmount = goal;
			yield break;
		}
	}
}