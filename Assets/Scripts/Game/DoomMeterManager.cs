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
		[SerializeField]
		public float timeLeft = 10;

		private bool isActive = false;

		public TextMeshProUGUI timeText;

		// Start is called before the first frame update
		void Start()
		{
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

		public void AddTime(float timeToAdd)
		{
			if (timeLeft > 0)
			{
				timeLeft = timeLeft + timeToAdd;
			}
			else
			{
				Debug.Log("Time has already run out.");
			}
		}

		public void RemoveTime(float timeToRemove)
		{
			if (timeLeft > 0)
			{
				timeLeft = timeLeft - timeToRemove;
			}
			else
			{
				Debug.Log("Time has already run out.");
			}
		}

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

		void DoSomething()
		{
			Debug.Log("Times run out.");
		}
	}
}
