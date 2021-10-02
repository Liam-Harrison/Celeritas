using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
{
	public Image background;

	public Color gold = new Color(255f, 211f, 0f);
	public Color red => Color.red;
	public Color white => Color.white;

	private Color transparent;

	private float lerpTime;

	// Start is called before the first frame update
	void Start()
    {
		gold.a = 0.01f;
		transparent = white;
		transparent.a = 0.0f;
		background.color = transparent;
		background.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {

	}

	public void RunFlash(float duration)
	{
		StartCoroutine(Flash(duration));
	}

	/// <summary>
	/// Coroutine that will run the flash effect.
	/// </summary>
	public IEnumerator Flash(float duration)
	{
		background.color = gold;
		background.enabled = true;
		yield return new WaitForSeconds(duration);
		background.color = transparent;
		background.enabled = false;
	}
}
