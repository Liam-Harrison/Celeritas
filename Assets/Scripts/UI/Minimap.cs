using Celeritas.Controllers;
using Cinemachine;
using UnityEngine;

namespace Celeritas.UI
{
	public class Minimap : MonoBehaviour
	{
		[SerializeField]
		private ShipController ship;
		[SerializeField]
		private float radius = 10;
		[SerializeField]
		private float pxRadius = 128;
		[SerializeField]
		private RectTransform marker;
		[SerializeField]
		private Font font;
		[SerializeField]
		private Transform[] boxes;
		[SerializeField]
		private RectTransform[] markers;

		float playerP;

		private void Update()
		{
			marker.anchoredPosition = GetPosition(ship.transform.position);

			for (int i = 0; i < boxes.Length; i++)
			{
				markers[i].anchoredPosition = GetPosition(boxes[i].position);
			}

			var dir = ship.transform.position.normalized;
			var p = playerP = ship.transform.position.magnitude / radius;

			if (p > 1.1f)
			{
				var cam = Camera.main.transform.GetComponent<CinemachineVirtualCamera>();

				dir = -dir;
				p = 1 - (p - 1);

				var old = ship.transform.position;
				ship.transform.position = dir * p * radius;

				var delta = ship.transform.position - old;
				cam.OnTargetObjectWarped(ship.transform, delta);

				//for (int i = 0; i < boxes.Length; i++)
				//{
				//	boxes[i].position += delta;
				//}
			}

			for (int i = 0; i < boxes.Length; i++)
			{
				var d = boxes[i].position - ship.transform.position;
				var pd = d.magnitude / radius;

				if (pd > 1)
				{
					var n = d.normalized;
					pd = 1 - (pd - 1);

					var pos = new Vector3();

					//if (Vector3.Dot(Vector3.right, n) > 0f)
					//{

					//	boxes[i].position = ship.transform.position + (-n * pd * radius);
					//}

					boxes[i].position = ship.transform.position - (Vector3.Reflect(n, Vector3.up) * d.magnitude);

					//boxes[i].position = ship.transform.position + (-n * pd * radius);
				}
			}
		}

		private Vector3 GetPosition(Vector3 target)
		{
			var dir = target.normalized;
			var p = target.magnitude / radius;

			if (p > 1)
			{
				dir = -dir;
				p = 1 - (p - 1);
			}

			return dir * p * pxRadius;
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(8, 4, 512, 26), $"P: {playerP:0.00}");
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(ship.transform.position, radius);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(Vector3.zero, radius * 1.1f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(Vector3.zero, radius);
		}
	}
}
