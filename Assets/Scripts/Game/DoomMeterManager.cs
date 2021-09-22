using Celeritas.AI;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages doom meter / timer.
	/// </summary>
	public class DoomMeterManager : Singleton<DoomMeterManager>
	{
		// Determines how much time is given at the beginning of the game.
		[SerializeField]
		public float timer = 10;

		// How much time remains.
		private float timeLeft;

		// Whether the count down is running.
		private bool isActive = false;

		public TextMeshProUGUI timeText;

		// Start is called before the first frame update
		void Start()
		{
			timeLeft = Mathf.FloorToInt(timer * 60);
			isActive = true;
		}

		// Update is called once per frame
		void Update()
		{
			if (isActive)
			{
				if (timeLeft > 0)
				{
					timeLeft -= Time.deltaTime;
					ShowTime(timeLeft);
				}
				else
				{
					timeLeft = 0;
					ShowTime(timeLeft);
					isActive = false;
					DoSomething();
				}
			}
		}

		// Adds X amount of time in seconds.
		public void AddTime(float timeToAdd)
		{
			if (timeLeft > 0)
			{
				timeLeft = Mathf.FloorToInt(timeLeft + timeToAdd);
			}
			else
			{
				Debug.Log("Time has already run out.");
			}
		}

		// Removes X amount of time in seconds.
		public void RemoveTime(float timeToRemove)
		{
			if (timeLeft > 0)
			{
				timeLeft = Mathf.FloorToInt(timeLeft - timeToRemove);
			}
			else
			{
				Debug.Log("Time has already run out.");
			}
		}

		// displays timer. Will be swapped out for a bar of some sort in the near future.
		void ShowTime(float timeToShow)
		{
			float minutes = Mathf.FloorToInt(timeToShow / 60);
			float seconds = Mathf.FloorToInt(timeToShow % 60);

			if (timeText)
			{
				
				timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
			}
			else
			{
				Debug.Log("No time text field located.");
			}
		}

		// What happens when the timer reaches zero.
		void DoSomething()
		{
			Debug.Log("Times run out.");
		}
	}
}
