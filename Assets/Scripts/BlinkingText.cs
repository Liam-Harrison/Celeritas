using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
	private float timer;
	[SerializeField]
	private TextMeshProUGUI textMesh;

	void Update()
	{
		timer = timer + Time.deltaTime * 5;
		textMesh.color = new Color(1, 1, 1, Mathf.Sin(timer));
	}
}
