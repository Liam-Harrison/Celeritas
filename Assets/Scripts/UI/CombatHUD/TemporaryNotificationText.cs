using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Logic for moving a text component slightly, while making it fade.
/// Used for displaying temporary notifications to the player, via PrintNotification in HUDManager.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class TemporaryNotificationText : MonoBehaviour
{
	[SerializeField, Title("Assignments")]
	private TextMeshProUGUI text;

	[SerializeField, Title("Settings")]
	private float timeToLive;

	[SerializeField]
	private float moveRate;

    void Start()
    {
		text.CrossFadeAlpha(0.0f, timeToLive, true);
	}
	 
    void Update()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y + (moveRate * Time.unscaledDeltaTime), 0);

		if (text.color.a == 0)
			Destroy(gameObject);
	}
}
