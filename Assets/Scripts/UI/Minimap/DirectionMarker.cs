using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class DirectionMarker : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private Image marker;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField, TitleGroup("Assignments")]
		private Image icon;

		public new RectTransform transform;

		public Image Marker { get => marker; }

		public Vector3 Point { get; private set; }

		private void Awake()
		{
			transform = GetComponent<RectTransform>();
		}

		public void SetupMarker(Vector3 point, Color color, string text = "", Sprite sprite = null)
		{
			Point = point;
			marker.color = color;

			this.text.text = text;
			this.text.gameObject.SetActive(text != "");

			icon.sprite = sprite;
			icon.gameObject.SetActive(sprite != null);
		}
	}
}