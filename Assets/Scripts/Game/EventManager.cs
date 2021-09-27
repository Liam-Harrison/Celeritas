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
		private const int EVENT_CREATION_DIST_MIN = 3;

		private const int EVENT_CREATION_DIST_MAX = 5;

		private const int EVENT_DESTROY_DIST = 10;

		private const float UPDATE_RATE = 1 / 2;

		private const int MIN_EVENTS = 2;

		private readonly HashSet<GameEvent> events = new HashSet<GameEvent>();

		private float lastUpdate;

		public IReadOnlyCollection<GameEvent> Events { get => events; }

		private new Camera camera;

		protected override void OnGameLoaded()
		{
			camera = Camera.main;
			base.OnGameLoaded();

			if (GameStateManager.Instance.GameState != GameState.MAINMENU)
			{
				for (int i = 0; i < MIN_EVENTS; i++)
				{
					CreateRandomEvent();
				}
			}

			GameStateManager.onStateChanged += OnStateChanged;
			Chunks.OnEnteredChunk += OnEnteredChunk;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnStateChanged;
			Chunks.OnEnteredChunk -= OnEnteredChunk;
		}

		private void FixedUpdate()
		{
			if (Time.time - lastUpdate > UPDATE_RATE)
			{
				lastUpdate = Time.time;
				var toUnload = new HashSet<GameEvent>();
				foreach (var e in events)
				{
					if (e.EventData.CannotUnloadDynamically == false && Chunks.ChunkManager.GetManhattenDistance(Chunks.ChunkManager.GetChunkIndex(camera.transform.position), e.OriginChunk) > EVENT_DESTROY_DIST)
					{
						toUnload.Add(e);
					}
				}

				foreach (var e in toUnload)
				{
					e.UnloadEvent();
				}
			}
		}

		private void OnStateChanged(GameState old, GameState state)
		{
			if (old == GameState.MAINMENU)
			{
				for (int i = 0; i < MIN_EVENTS; i++)
				{
					CreateRandomEvent();
				}
			}
		}

		private void CreateRandomEvent()
		{
			var e = EntityDataManager.Instance.Events.Where(x => events.Where(y => y.EventData == x).Count() == 0).OrderBy(x => Random.value).Take(1);
			CreateEvent(e.First());
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

			for (int i = events.Count; i < MIN_EVENTS; i++)
			{
				CreateRandomEvent();
			}
		}

		public void CreateEvent(EventData data)
		{
			CreateEvent(data, GetRandomLocation(data));
		}

		private Vector2Int GetRandomLocation(EventData data)
		{
			Vector3 pos;
			int c = 0;

			do
			{
				pos = Camera.main.transform.position + (Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.up * (Chunks.ChunkManager.ChunkSize.x * Mathf.Lerp(EVENT_CREATION_DIST_MIN, EVENT_CREATION_DIST_MAX, Random.value)));
				c++;
			} while (WillEventOverlap(pos, data) && c < 10);

			return Chunks.ChunkManager.GetChunkIndex(pos);
		}

		private bool WillEventsOverlap(Vector2Int aid, EventData a, Vector2Int bid, EventData b)
		{
			var achunks = GameEvent.GetChunks(aid, a);
			var bchunks = GameEvent.GetChunks(bid, b);

			foreach (var i in achunks)
			{
				foreach (var j in bchunks)
				{
					if (i == j)
						return true;
				}
			}
			return false;
		}

		private bool WillEventOverlap(Vector3 pos, EventData data)
		{
			var p = Chunks.ChunkManager.GetChunkIndex(pos);
			foreach (var e in events)
			{
				if (WillEventsOverlap(p, data, e.OriginChunk, e.EventData))
					return true;
			}
			return false;
		}

		private bool IsPointInEvent(Vector3 pos)
		{
			var chunk = Chunks.ChunkManager.GetChunkIndex(pos);
			foreach (var e in events)
			{
				if (e.ChunkIds.Contains(chunk))
					return true;
			}
			return false;
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