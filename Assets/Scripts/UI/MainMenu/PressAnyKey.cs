using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
	public Button button;
	public CanvasGroup canvas;

    // Start is called before the first frame update
    void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		if (Keyboard.current.anyKey.wasPressedThisFrame)
		{
			button.onClick.Invoke();
		}
		if (canvas.alpha > 0)
		{
			this.enabled = false;
		}
	}
}
