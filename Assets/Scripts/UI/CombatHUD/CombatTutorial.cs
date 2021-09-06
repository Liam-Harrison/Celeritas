using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : MonoBehaviour
{
	private bool tutorialOn;
	private void Awake()
	{
		tutorialOn = true;
	}

	private float timer = 0.0f;
	public float secondsToDisplay;
	private void Update()
	{
		timer += Time.deltaTime;
		if ((timer > secondsToDisplay) && tutorialOn)
		{
			toggleTutorial();
		}
	}

	public void toggleTutorial()
	{
		tutorialOn = !tutorialOn;
		gameObject.SetActive(tutorialOn);
	}
}
