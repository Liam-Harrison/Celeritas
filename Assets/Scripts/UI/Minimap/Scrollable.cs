using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class Scrollable : MonoBehaviour
	{
		[SerializeField]
		private Image image;

		public new RectTransform transform;

		public Vector3 Position { get; private set; }

		public Image Image { get => image; }

		private void Awake()
		{
			transform = GetComponent<RectTransform>();
		}

		public void Initalize(Vector3 position)
		{
			Position = position;
		}
	}
}
