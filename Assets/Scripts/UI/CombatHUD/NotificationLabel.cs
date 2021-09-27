using Celeritas.Game;
using Celeritas.Game.Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Celeritas.UI
{

	/// <summary>
	/// Logic for moving a text component slightly, while making it fade.
	/// Used for displaying temporary notifications to the player, via PrintNotification in HUDManager.
	/// </summary>
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class NotificationLabel : MonoBehaviour, IPooledObject<NotificationLabel>
	{
		private const float SHOW_TIME = 2f;

		private const float ANIMATE_OUT_TIME = 0.5f;

		[SerializeField, Title("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField]
		private float moveRate;

		private float spawned;

		public ObjectPool<NotificationLabel> OwningPool { get; set; }

		private static NotificationLabel latest;

		private new RectTransform transform;

		private bool crossfading = false;

		void Awake()
		{
			transform = GetComponent<RectTransform>();
		}

		public void OnSpawned()
		{
			crossfading = false;
			text.CrossFadeAlpha(1, 0, true);
			spawned = Time.unscaledTime;

			if (latest != null)
			{
				transform.position = latest.transform.position - new Vector3(0, transform.rect.height + 5, 0);
			}
			else
			{
				transform.anchoredPosition = new Vector2(0, 0);
			}

			latest = this;
		}

		public void OnDespawned()
		{
			if (latest == this)
				latest = null;
		}

		void Update()
		{
			if (Time.unscaledTime > spawned + SHOW_TIME)
			{
				OwningPool.ReleasePooledObject(this);
			}
			else
			{
				if (Time.unscaledTime > spawned + SHOW_TIME - ANIMATE_OUT_TIME && crossfading == false)
				{
					crossfading = true;
					text.CrossFadeAlpha(0f, ANIMATE_OUT_TIME, true);
				}

				transform.position = new Vector3(transform.position.x, transform.position.y + moveRate * Time.unscaledDeltaTime, 0);
			}
		}

		public void SetText(string text)
		{
			this.text.text = text;
		}
	}
}