using UnityEngine;

namespace Celeritas.UI
{
	public class Scrollable : MonoBehaviour
	{
		public new RectTransform transform;

		public Vector3 Position { get; private set; }

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