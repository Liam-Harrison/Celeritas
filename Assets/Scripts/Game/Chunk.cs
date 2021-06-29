using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Manages the entities within a chunk of the game.
	/// </summary>
	public class Chunk
	{
		/// <summary>
		/// The index of this chunk.
		/// </summary>
		public Vector2Int Index { get; private set; }

		/// <summary>
		/// The size of this chunk.
		/// </summary>
		public Vector2 Size { get; private set; }

		/// <summary>
		/// The centre of this chunk.
		/// </summary>
		public Vector3 Center { get; private set; }

		/// <summary>
		/// The boundary of this chunk.
		/// </summary>
		public Bounds Boundary { get; private set; }

		/// <summary>
		/// The entities attatched to this chunk.
		/// </summary>
		public IReadOnlyCollection<Entity> Entites { get => entities; }

		private readonly List<Entity> entities = new List<Entity>();

		public Chunk(Vector2Int index, Vector2 size)
		{
			var centre = new Vector3(index.x * size.x, index.y * size.y, 0);

			Index = index;
			Size = size;
			Center = centre;
			Boundary = new Bounds(centre, size);
		}

		/// <summary>
		/// Set the active state of all the gameobjects in this chunk.
		/// </summary>
		/// <param name="value">The value to set.</param>
		public void ChunkSetActive(bool value)
		{
			foreach (var entity in entities)
			{
				entity.gameObject.SetActive(value);
			}
		}

		/// <summary>
		/// Unload and remove all entities inside this chunk.
		/// </summary>
		public void UnloadChunk()
		{
			foreach (var entity in entities)
			{
				entity.UnloadEntity();
			}
			entities.Clear();
		}

		/// <summary>
		/// Add an entity to this chunk.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddEntity(Entity entity)
		{
			entities.Add(entity);
			entity.Chunk = this;
		}

		/// <summary>
		/// Remove the provided entity from this chunk.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public void RemoveEntity(Entity entity)
		{
			if (entities.Contains(entity))
			{
				entities.Remove(entity);

				if (entity.Chunk == this)
					entity.Chunk = null;
			}
		}

		/// <summary>
		/// Get a random world position inside the boundary.
		/// </summary>
		/// <returns>The random point inside this chunk.</returns>
		public Vector3 GetRandomPositionInChunk()
		{
			float x = Random.Range(Center.x - Boundary.extents.x, Center.x + Boundary.extents.x);
			float y = Random.Range(Center.y - Boundary.extents.y, Center.y + Boundary.extents.y);
			return new Vector3(x, y, 0);
		}
	}
}
