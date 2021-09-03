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
		private const int EVENT_CREATION_DIST = 4;

		private const int EVENT_DESTROY_DIST = 10;

		private const float UPDATE_RATE = 1 / 2;

		private readonly HashSet<GameEvent> events = new HashSet<GameEvent>();

		private float lastUpdate;

		public IReadOnlyCollection<GameEvent> Events { get => events; }

		private new Camera camera;

		protected override void OnGameLoaded()
		{
			camera = Camera.main;
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