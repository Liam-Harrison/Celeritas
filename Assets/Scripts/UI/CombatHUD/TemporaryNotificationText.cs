using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryNotificationText : MonoBehaviour
{
	[SerializeField, Title("Time for message to last")]
	private float timeToLive; // how long the message should stay on screen for

	[SerializeField, Title("Move Rate - how quickly the message moves up")]
	private float moveRate;

	private Text text;

    // Start is called before the first frame update
    void Start()
    {
		text = GetComponent<Text>();

		// fade out
		text.CrossFadeAlpha(0.0f, timeToLive, false);
	}

    // Update is called once per frame
    void Update()
    {
		// move up
		transform.position = new Vector3(transform.position.x, transform.position.y + moveRate, 0);

		// if not visible anymore, destroy
		if (text.color.a == 0)
			Destroy(gameObject);

	}
}
