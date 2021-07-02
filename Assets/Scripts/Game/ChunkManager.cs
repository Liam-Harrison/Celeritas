using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages a collection of chunks, and abstracts away the details of creating, destroying and managing chunks.
	/// </summary>
	public class ChunkManager
	{
		/// <summary>
		/// The size of this chunk.
		/// </summary>
		public Vector2 ChunkSize { get; private set; }

		/// <summary>
		/// The chunks inside this chunk manager.
		/// </summary>
		public IReadOnlyCollection<Chunk> Chunks { get => chunks.Values; }

		/// <summary>
		/// The keys of chunks inside this manager.
		/// </summary>
		public IReadOnlyCollection<Vector2Int> Keys { get => chunks.Keys; }

		private readonly Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

		public ChunkManager(Vector2 size)
		{
			ChunkSize = size;
		}

		/// <summary>
		/// Get the index of a chuck with a specified position.
		/// </summary>
		/// <param name="position">The position to check.</param>
		/// <returns>The index of the chunk.</returns>
		public Vector2Int GetChunkIndex(Vector3 position)
		{
			var x = Mathf.RoundToInt(position.x / ChunkSize.x);
			var y = Mathf.RoundToInt(position.y / ChunkSize.y);
			return new Vector2Int(x, y);
		}

		/// <summary>
		/// Get manhatten distance between two indexes.
		/// </summary>
		/// <param name="a">The first index.</param>
		/// <param name="b">The second index.</param>
		/// <returns>The resulting manhatten distance.</returns>
		public int GetManhattenDistance(Vector2Int a, Vector2Int b)
		{
			var d = b - a;
			return Mathf.Abs(d.x) + Mathf.Abs(d.y);
		}

		/// <summary>
		/// Get manhatten distance between two chunks.
		/// </summary>
		/// <param name="a">The first chunk.</param>
		/// <param name="b">The second chunk.</param>
		/// <returns>The resulting manhatten distance.</returns>
		public int GetManhattenDistance(Chunk a, Chunk b)
		{
			return GetManhattenDistance(a.Index, b.Index);
		}

		/// <summary>
		/// Try to get the chunk at the specified index.
		/// </summary>
		/// <param name="index">The index to check.</param>
		/// <param name="chunk">The found chunk.</param>
		/// <returns>Returns true if the chunk was present, otherwise false.</returns>
		public bool TryGetChunk(Vector2Int index, out Chunk chunk)
		{
			chunk = null;

			if (chunks.ContainsKey(index))
				chunk = chunks[index];

			return chunk != null;
		}

		/// <summary>
		/// Try to get the chunk at the specified position.
		/// </summary>
		/// <param name="position">The position to check.</param>
		/// <param name="chunk">The found chunk.</param>
		/// <returns>Returns true if the chunk was present, otherwise false.</returns>
		public bool TryGetChunk(Vector3 position, out Chunk chunk)
		{
			return TryGetChunk(GetChunkIndex(position), out chunk);
		}

		/// <summary>
		/// Create a chunk at the specified index.
		/// </summary>
		/// <param name="index">The index of the chunk to create.</param>
		/// <returns>Returns the created chunk.</returns>
		public Chunk CreateChunk(Vector2Int index)
		{
			if (chunks.ContainsKey(index))
			{
				Debug.LogError($"Attempted to create chunk at index which already exists.");
				return chunks[index];
			}

			chunks[index] = new Chunk(this, index, ChunkSize);
			return chunks[index];
		}

		/// <summary>
		/// Find or create a chunk at the specified index.
		/// </summary>
		/// <param name="index">The index of the chunk to find or create.</param>
		/// <returns>The chunk at the specified position.</returns>
		public Chunk GetOrCreateChunk(Vector2Int index)
		{
			if (chunks.ContainsKey(index))
				return chunks[index];

			return CreateChunk(index);
		}

		/// <summary>
		/// Find or create a chunk at the specified position.
		/// </summary>
		/// <param name="position">The position of the chunk to find or create.</param>
		/// <returns>The chunk at the specified position.</returns>
		public Chunk GetOrCreateChunk(Vector3 position)
		{
			return GetOrCreateChunk(GetChunkIndex(position));
		}

		/// <summary>
		/// Remove and unload the chunk at the specified index.
		/// </summary>
		/// <param name="index">The index of the chunk.</param>
		public void UnloadChunk(Vector2Int index)
		{
			if (chunks.ContainsKey(index))
			{
				chunks[index].UnloadChunk();
				chunks.Remove(index);
			}
		}

		/// <summary>
		/// Remove and unload the chunk.
		/// </summary>
		/// <param name="chunk">The chunk to unload.</param>
		public void UnloadChunk(Chunk chunk)
		{
			UnloadChunk(chunk.Index);
		}

		/// <summary>
		/// Does this chunk manager have the current chunk.
		/// </summary>
		/// <param name="index">The index of the chunk.</param>
		/// <returns>Returns true if this chunk is avaliable.</returns>
		public bool HasChunk(Vector2Int index)
		{
			return chunks.ContainsKey(index);
		}
	}
}
