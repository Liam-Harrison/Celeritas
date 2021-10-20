using Celeritas.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CanvasScaler = Celeritas.UI.CanvasScaler;

namespace Celeritas.Game.Events
{
	public class GameEvent
	{
		public EventData EventData { get; private set; }

		public readonly List<Vector2Int> ChunkIds = new List<Vector2Int>();

		public readonly List<Scrollable> scrollables = new List<Scrollable>();

		public Vector2Int OriginChunk { get; private set; }

		private DirectionMarker marker;

		public bool AreaEntered { get; private set; } = false;

		private GameObject backgroundObject;

		public virtual void Initalize(EventData data, Vector2Int chunk)
		{
			EventData = data;
			OriginChunk = chunk;

			ChunkIds.AddRange(GetChunks(OriginChunk, data));

			foreach(var c in ChunkIds)
			{
				EventManager.Instance.ClearImageInChunk(c);
			}

			if (data.BackgroundSprite != null)
			{
				backgroundObject = EventManager.Instance.CreateBackgroundPrefab();

				var pos = Chunks.ChunkManager.GetPositionFromIndex(chunk);

				pos.z = 40;

				backgroundObject.transform.position = pos;
				backgroundObject.GetComponentInChildren<Image>().sprite = data.BackgroundSprite;

				backgroundObject.GetComponent<CanvasScaler>().SetSize(data.BackgroundSize, data.BackgroundSize);
			}

			if (data.ShowArrow)
			{
				Vector3 middle = Vector3.zero;

				foreach (var id in ChunkIds)
				{
					middle += Chunks.ChunkManager.GetPositionFromIndex(id);
				}

				middle /= ChunkIds.Count;

				marker = Minimap.Instance.CreateMarker(middle, data.ArrowColor, data.ArrowText, data.ArrowIcon);
				marker.Marker.color = data.ArrowColor;
			}

			if (data.ShowOnMap)
			{
				foreach (var c in ChunkIds)
				{
					var s = Minimap.Instance.CreateScrollable(c);
					s.Image.color = data.MapColor;
					scrollables.Add(s);
				}
			}
		}

		public static List<Vector2Int> GetChunks(Vector2Int origin, EventData data)
		{
			var chunks = new List<Vector2Int>();
			for (int x = 0; x < data.Grid.GetLength(0); x++)
			{
				for (int y = 0; y < data.Grid.GetLength(1); y++)
				{
					if (data.Grid[x, y])
					{
						var delta = data.GetRelativeToMiddle(x, y);
						chunks.Add(origin + delta);
					}
				}
			}
			return chunks;
		}

		public void EnterEventArea()
		{
			AreaEntered = true;
			EventData.EventOutcome.DoEventOutcome(this);
		}

		public void EndEvent()
		{
			UnloadEvent();
		}

		public void UnloadEvent()
		{
			if (marker != null)
				Minimap.Instance.RemoveDirectionMarker(marker);

			foreach (var scrollable in scrollables)
			{
				Minimap.Instance.RemoveScrollable(scrollable);
			}

			EventManager.Instance.EventUnloaded(this);
		}
	}
}