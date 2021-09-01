using Celeritas.Game.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public class Chunks : Singleton<Chunks>
	{
		public const float CHUNK_SIZE = 100;

		private const int DISABLE_CHUNK_DIST = 3;

		private const int UNLOAD_CHUNK_DIST = 6;

		private const float UPDATE_FREQ = 2;

		private float lastUpdate;

		/// <summary>
		/// The chunk manager for the game.
		/// </summary>
		public static ChunkManager ChunkManager { get; private set; }

		public static event Action<Chunk> OnCreatedChunk;

		public static event Action<Chunk> OnEnteredChunk;

		private new Camera camera;

		protected override void Awake()
		{
			ChunkManager = new ChunkManager(new Vector2(CHUNK_SIZE, CHUNK_SIZE));
			camera = Camera.main;

			base.Awake();
		}

		private void FixedUpdate()
		{
			if (!EntityDataManager.Instance.Loaded || Time.unscaledTime < lastUpdate + (1f / UPDATE_FREQ))
				return;

			lastUpdate = Time.unscaledTime;
			UpdateChunks();
		}

		private Color green = new Color(0, 1, 0, 0.1f);
		private Color yellow = new Color(1, 0.92f, 0.016f, 0.1f);

		private void OnDrawGizmosSelected()
		{
			if (ChunkManager == null)
				return;

			foreach (var chunk in ChunkManager.Chunks)
			{
				if (chunk.Active)
					Gizmos.color = green;
				else
					Gizmos.color = yellow;

				Gizmos.DrawCube(chunk.Center, new Vector3(chunk.Size.x, chunk.Size.y, 1));
			}
		}

		protected override void OnGameLoaded()
		{
			base.OnGameLoaded();
			UpdateChunks();
		}

		private Vector2Int last;

		private void UpdateChunks()
		{
			var middle = ChunkManager.GetChunkIndex(camera.transform.position);

			for (int x = 0; x < UNLOAD_CHUNK_DIST * 2; x++)
			{
				for (int y = 0; y < UNLOAD_CHUNK_DIST * 2; y++)
				{
					var index = middle + new Vector2Int(x - UNLOAD_CHUNK_DIST, y - UNLOAD_CHUNK_DIST);

					if (ChunkManager.GetManhattenDistance(middle, index) >= UNLOAD_CHUNK_DIST)
						continue;

					if (ChunkManager.TryGetChunk(index, out var chunk))
					{
						chunk.ChunkSetActive(ChunkManager.GetManhattenDistance(middle, index) < DISABLE_CHUNK_DIST);
					}
					else
					{
						chunk = ChunkManager.CreateChunk(index);
						chunk.ChunkSetActive(ChunkManager.GetManhattenDistance(middle, index) < DISABLE_CHUNK_DIST);
						OnCreatedChunk?.Invoke(chunk);
					}
				}
			}

			var toRemove = new HashSet<Vector2Int>();
			foreach (var chunk in ChunkManager.Keys)
			{
				if (ChunkManager.GetManhattenDistance(middle, chunk) >= UNLOAD_CHUNK_DIST)
				{
					toRemove.Add(chunk);
				}
			}

			foreach (var chunk in toRemove)
			{
				ChunkManager.UnloadChunk(chunk);
			}

			if (last != middle && ChunkManager.TryGetChunk(middle, out var mid))
			{
				OnEnteredChunk?.Invoke(mid);
			}

			last = middle;
		}
	}
}
