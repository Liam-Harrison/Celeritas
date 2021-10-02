using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
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

	public void RunFlash(float duration, Color color)
	{
		flashColor = color;
		flashColor.a = 0.01f;
		StartCoroutine(Flash(duration));
	}

	/// <summary>
	/// Coroutine that will run the flash effect.
	/// </summary>
	public IEnumerator Flash(float duration)
	{
		background.color = flashColor;
		background.enabled = true;
		yield return new WaitForSeconds(duration);
		background.color = transparent;
		background.enabled = false;
	}
}
