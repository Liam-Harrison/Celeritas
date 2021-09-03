using Celeritas.Game.Entities;
using Celeritas.Game.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Celeritas.Game
{
	public class EventManager : Singleton<EventManager>
	{
		private const int EVENT_CREATION_DIST = 6;

		private const int EVENT_DESTROY_DIST = 10;

		private readonly HashSet<GameEvent> events = new HashSet<GameEvent>();

		public IReadOnlyCollection<GameEvent> Events { get => events; }

		protected override void OnGameLoaded()
		{
			base.OnGameLoaded();

			if (GameStateManager.Instance.GameState != GameState.MAINMENU)
				CreateRandomEvent();

			GameStateManager.onStateChanged += OnStateChanged;
			Chunks.OnEnteredChunk += OnEnteredChunk;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnStateChanged;
			Chunks.OnEnteredChunk -= OnEnteredChunk;
		}

		private void OnStateChanged(GameState old, GameState state)
		{
			if (old == GameState.MAINMENU)
			{
				CreateRandomEvent();
			}
		}

		private void CreateRandomEvent()
		{
			var e = EntityDataManager.Instance.Events.OrderBy(x => Random.value).Take(1);
			CreateEvent(e.First(), GetRandomLocation());
		}

		public void CreateEvent(EventData data, Vector2Int chunk)
		{
			var gameEvent = new GameEvent();
			gameEvent.Initalize(data, chunk);
			events.Add(gameEvent);
		}

		public void RemoveEvent(GameEvent gameEvent)
		{
			if (events.Contains(gameEvent))
			{
				gameEvent.UnloadEvent();
			}
		}

		public void EventUnloaded(GameEvent gameEvent)
		{
			if (events.Contains(gameEvent))
			{
				events.Remove(gameEvent);
			}

			if (events.Count == 0)
				CreateRandomEvent();
		}

		public void CreateEvent(EventData data)
		{
			CreateEvent(data, GetRandomLocation());
		}

		private Vector2Int GetRandomLocation()
		{
			var pos = Camera.main.transform.position + (Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.up * (Chunks.ChunkManager.ChunkSize.x * EVENT_CREATION_DIST));
			return Chunks.ChunkManager.GetChunkIndex(pos);
		}

		private void OnEnteredChunk(Chunk chunk)
		{
			foreach (var e in events)
			{
				if (e.AreaEntered)
					continue;

				foreach (var id in e.ChunkIds)
				{
					if (id == chunk.Index)
					{
						e.EnterEventArea();
						break;
					}
				}
			}
		}
	}
}