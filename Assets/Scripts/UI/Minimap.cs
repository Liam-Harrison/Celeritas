using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using UnityEngine;

namespace Celeritas.UI
{
	/// <summary>
	/// Types of markers avalaible to the UI.
	/// </summary>
	public enum MarkerType
	{
		Player,
		Enemy
	}

	public class Minimap : MonoBehaviour
	{
		[SerializeField]
		private PlayerController ship;

		[SerializeField]
		private float radius = 10;

		[SerializeField]
		private float pxRadius = 128;

		[SerializeField]
		private GameObject markerPrefab;

		[SerializeField]
		private ObjectPool<MinimapMarker> markers;

		private void Awake()
		{
			markers = new ObjectPool<MinimapMarker>(markerPrefab, transform);
		}

		private void Update()
		{
			foreach (var marker in markers.ActiveObjects)
			{
				marker.
			}
			marker.anchoredPosition = GetPosition(ship.transform.position);
		}

		public void TrackEntity(Entity ship, MarkerType type)
		{
			markers.GetPooledObject().Initalize(ship, type);
		}

		private Vector3 GetPosition(Vector3 target)
		{
			var dir = target.normalized;
			var p = Mathf.Clamp01(target.magnitude / radius);

			return dir * p * pxRadius;
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
