using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
	public Image background;

	public Color white => Color.white;

	private Color transparent;
	private Color flashColor;

	// Start is called before the first frame update
	void Start()
    {
		transparent = white;
		transparent.a = 0.0f;
		background.color = transparent;
		background.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {

	}

	public void RunLerp(float duration, Color color)
	{
		background.enabled = true;
		StartCoroutine(LerpEffect(color, (duration/3)));
	}

	/// <summary>
	/// Coroutine that will run the lerp effect.
	/// </summary>
	IEnumerator LerpEffect(Color endValue, float duration)
	{
		float time = 0;
		Color startValue = background.color;

		while (time < duration)
		{
			background.color = Color.Lerp(startValue, endValue, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		background.color = endValue;

		RunLerpReturn(3);
	}

	public void RunLerpReturn(float duration)
	{
		if (background.color.a != 0.0f)
		{
			StartCoroutine(LerpEffect(transparent, (duration)));
		}
		else
		{
			background.enabled = false;
		}
	}

	public void RunFlash(float duration, Color color)
	{
		flashColor = color;
		flashColor.a = 0.01f;
		StartCoroutine(FlashEffect(duration));
	}

	/// <summary>
	/// Coroutine that will run the flash effect.
	/// </summary>
	public IEnumerator FlashEffect(float duration)
	{
		background.color = flashColor;
		background.enabled = true;
		yield return new WaitForSeconds(duration);
		background.color = transparent;
		background.enabled = false;
	}
}
