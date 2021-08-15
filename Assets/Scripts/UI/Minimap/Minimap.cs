using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.UI
{
	/// <summary>
	/// Manages the UI and game-logic of a minimap.
	/// </summary>
	public class Minimap : MonoBehaviour
	{
		[SerializeField, Title("Minimap Settings")]
		private float worldRadius = 10;

		[SerializeField, InfoBox("UI Radius will be half your minimaps diamater, minus some extra for the border of the image.")]
		private float uiRadius = 128;

		[SerializeField, PropertySpace(20)]
		private GameObject markerPrefab;

		[SerializeField]
		private ObjectPool<MinimapMarker> pooledMarkers;

		private Vector3 center;

		private void Awake()
		{
			pooledMarkers = new ObjectPool<MinimapMarker>(markerPrefab, transform);
			EntityDataManager.OnCreatedEntity += OnCreatedEntity;
		}

		private void OnDestroy()
		{
			EntityDataManager.OnCreatedEntity -= OnCreatedEntity;
		}

		private void Update()
		{
			if (PlayerController.Exists)
				center = PlayerController.Instance.PlayerShipEntity.Position;

			for (int i = 0; i < pooledMarkers.ActiveObjects.Count; i++)
			{
				var marker = pooledMarkers.ActiveObjects[i];

				if (marker.Entity == null || !marker.Entity.IsInitalized)
				{
					pooledMarkers.ReleasePooledObject(marker);
					continue;
				}

				marker.RectTransform.anchoredPosition = GetPosition(center, marker.Entity.Position);
				marker.SetAlpha(GetAlpha(center, marker.Entity.Position));
			}
		}

		/// <summary>
		/// Track an entity in the minimap.
		/// </summary>
		/// <param name="entity">The entity to track.</param>
		public void TrackEntity(Entity entity)
		{
			if (entity is ModuleEntity)
				return;

			var marker = pooledMarkers.GetPooledObject();
			marker.Initalize(entity);
			marker.RectTransform.anchoredPosition = GetPosition(center, marker.Entity.Position);
		}

		private void OnCreatedEntity(Entity entity)
		{
			TrackEntity(entity);
		}

		private float GetAlpha(Vector3 center, Vector3 target)
		{
			var delta = target - center;
			var length = delta.magnitude - worldRadius;
			if (length < 0)
				return 1f;
			else
				return 1f - Mathf.Clamp01(length / 10f);
		}

		private Vector3 GetPosition(Vector3 center, Vector3 target)
		{
			var delta = target - center;
			var dir = delta.normalized;
			var p = Mathf.Clamp01(delta.magnitude / worldRadius);

			return dir * p * uiRadius;
		}
	}
}
