using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// Manages the UI and game-logic of a minimap.
	/// </summary>
	public class Minimap : Singleton<Minimap>
	{
		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI scale;

		[SerializeField, TitleGroup("Assignments")]
		private RectTransform directionMarkerParent;

		[SerializeField, TitleGroup("Minimap Settings")]
		private float worldRadius = 100;

		[SerializeField, TitleGroup("Minimap Settings")]
		private float radiusPadding = 5;

		[SerializeField, TitleGroup("Minimap Settings")]
		private float markerRadius = 100;

		[SerializeField, PropertySpace(20), TitleGroup("Prefabs")]
		private GameObject markerPrefab;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject directionMarkerPrefab;

		[SerializeField, TitleGroup("Prefabs")]
		private GameObject scrollablePrefab;

		[SerializeField, TitleGroup("Prefabs")]
		private ObjectPool<MinimapMarker> pooledMarkers;

		private readonly HashSet<DirectionMarker> directionMarkers = new HashSet<DirectionMarker>();

		private readonly HashSet<Scrollable> scrollables = new HashSet<Scrollable>();

		private new RectTransform transform;

		private Vector3 center;

		protected override void Awake()
		{
			transform = GetComponent<RectTransform>();
			pooledMarkers = new ObjectPool<MinimapMarker>(markerPrefab, transform);
			EntityDataManager.OnCreatedEntity += OnCreatedEntity;

			base.Awake();
		}

		protected override void OnDestroy()
		{
			EntityDataManager.OnCreatedEntity -= OnCreatedEntity;
			base.OnDestroy();
		}

		protected override void OnGameLoaded()
		{
			base.OnGameLoaded();

			CreateScrollable(new Vector2Int(0, 1));
			CreateScrollable(new Vector2Int(1, 1));
			CreateScrollable(new Vector2Int(1, 2));
			CreateScrollable(new Vector2Int(2, 2));
		}

		private void Update()
		{
			if (PlayerController.Exists)
				center = PlayerController.Instance.PlayerShipEntity.Position;

			UpdateEntityMarkers();
			UpdateDirectionMarkers();
			UpdateScrollables();

			string size;
			if (worldRadius < 1000)
				size = $"{RoundToNearest(worldRadius, 100)}m";
			else
				size = $"{RoundToNearest(worldRadius, 250)}km";

			scale.text = size;
		}

		private void UpdateEntityMarkers()
		{
			var radius = GetUIRadius();

			for (int i = 0; i < pooledMarkers.ActiveObjects.Count; i++)
			{
				var marker = pooledMarkers.ActiveObjects[i];

				if (marker.Entity == null || !marker.Entity.IsInitalized)
				{
					pooledMarkers.ReleasePooledObject(marker);
					continue;
				}

				marker.RectTransform.anchoredPosition = GetPosition(center, marker.Entity.Position, radius);
				marker.SetAlpha(GetAlpha(center, marker.Entity.Position));
			}
		}

		private void UpdateScrollables()
		{
			var radius = GetUIRadius();

			foreach (var scrollable in scrollables)
			{
				scrollable.transform.anchoredPosition = GetPositionUnclamped(center, scrollable.Position, radius);
			}
		}

		public Scrollable CreateScrollable(Vector2Int chunk)
		{
			var scrollable = Instantiate(scrollablePrefab, transform).GetComponent<Scrollable>();
			scrollable.transform.SetAsFirstSibling();
			scrollable.Initalize(Chunks.ChunkManager.GetPositionFromIndex(chunk));
			scrollables.Add(scrollable);
			return scrollable;
		}

		public void RemoveScrollable(Scrollable scrollable)
		{
			if (scrollables.Contains(scrollable))
			{
				scrollables.Remove(scrollable);
			}
		}

		public DirectionMarker CreateMarker(Vector3 point, Color color, string text = "", Sprite icon = null)
		{
			var marker = Instantiate(directionMarkerPrefab, directionMarkerParent).GetComponent<DirectionMarker>();
			marker.SetupMarker(point, color, text, icon);
			directionMarkers.Add(marker);
			return marker;
		}

		private void UpdateDirectionMarkers()
		{
			foreach (var marker in directionMarkers)
			{
				var dir = (marker.Point - center).normalized;
				var rot = Quaternion.LookRotation(Vector3.forward, dir);

				marker.transform.anchoredPosition3D = rot * Vector3.up * markerRadius;
				marker.transform.rotation = rot;
			}
		}

		public void RemoveDirectionMarker(DirectionMarker marker)
		{
			if (directionMarkers.Contains(marker))
				directionMarkers.Remove(marker);
		}

		private float RoundToNearest(float value, float amount)
		{
			return Mathf.Round(value / amount) * amount;
		}

		/// <summary>
		/// Track an entity in the minimap.
		/// </summary>
		/// <param name="entity">The entity to track.</param>
		public void TrackEntity(Entity entity)
		{
			if (entity is ModuleEntity || entity is ProjectileEntity || entity is Asteroid)
				return;

			var marker = pooledMarkers.GetPooledObject();
			marker.Initalize(entity);
			marker.RectTransform.anchoredPosition = GetPosition(center, marker.Entity.Position, GetUIRadius());
		}

		private float GetUIRadius()
		{
			return transform.rect.width / 2 - radiusPadding;
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

		private Vector3 GetPosition(Vector3 center, Vector3 target, float uiRadius)
		{
			var delta = target - center;
			var dir = delta.normalized;
			var p = Mathf.Clamp01(delta.magnitude / worldRadius);

			return dir * p * uiRadius;
		}

		private Vector3 GetPositionUnclamped(Vector3 center, Vector3 target, float uiRadius)
		{
			var delta = target - center;
			var dir = delta.normalized;
			var p = delta.magnitude / worldRadius;

			return dir * p * uiRadius;
		}
	}
}
