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
		[SerializeField, Title("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField, Title("Settings")]
		private float timeToLive;

		[SerializeField]
		private float moveRate;

		private float spawned;

		public ObjectPool<NotificationLabel> OwningPool { get; set; }

		public void OnSpawned()
		{
			text.alpha = 1;
			text.CrossFadeAlpha(0f, timeToLive, true);
			spawned = Time.unscaledTime;
		}

		public void OnDespawned()
		{

		}

		void Update()
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + moveRate * Time.unscaledDeltaTime, 0);

			if (Time.unscaledTime > spawned + timeToLive)
				OwningPool.ReleasePooledObject(this);
		}

		public void SetText(string text)
		{
			this.text.text = text;
		}
	}
}