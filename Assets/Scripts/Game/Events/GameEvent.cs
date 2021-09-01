using Celeritas.UI;
using System.Collections.Generic;
using UnityEngine;

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

		public virtual void Initalize(EventData data, Vector2Int chunk)
		{
			EventData = data;
			OriginChunk = chunk;

			for (int x = 0; x < data.Grid.GetLength(0); x++)
			{
				for (int y = 0; y < data.Grid.GetLength(1); y++)
				{
					if (data.Grid[x, y])
					{
						var delta = data.GetRelativeToMiddle(x, y);
						ChunkIds.Add(OriginChunk + delta);
					}
				}
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
			}

			if (data.ShowOnMap)
			{
				foreach (var c in ChunkIds)
				{
					Minimap.Instance.CreateScrollable(c);
				}
			}

			EventData.OnCreated();
		}

		public void EnterEventArea()
		{
			AreaEntered = true;
			EventData.OnEntered();

			if (EventData.ShowDialogue)
			{
				DialogueManager.Instance.ShowDialogue(EventData.DialogueInfo, null);
			}
		}

		public void EndEvent()
		{
			EventData.OnEnded();
			UnloadEvent();
		}

		public void UnloadEvent()
		{
			EventData.OnUnloaded();

			if (marker != null)
				Minimap.Instance.RemoveDirectionMarker(marker);

			foreach (var scrollable in scrollables)
			{
				Minimap.Instance.RemoveScrollable(scrollable);
			}
		}
	}
}