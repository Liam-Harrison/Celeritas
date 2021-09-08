using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The combat tutorial!
/// Mainly controls whether or not it's visible.
/// </summary>

public class CombatTutorial : MonoBehaviour
{
	private bool tutorialOn;
	private void Awake()
	{
		tutorialOn = true;
	}

	private float timer = 0.0f;
	public float SecondsToDisplay;
	private void Update()
	{
		timer += Time.deltaTime;
		if ((timer > SecondsToDisplay) && tutorialOn)
		{
			ToggleTutorial();
		}
	}

	public void ToggleTutorial()
	{
		tutorialOn = !tutorialOn;
		gameObject.SetActive(tutorialOn);
	}
}
