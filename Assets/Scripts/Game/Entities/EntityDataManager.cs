using Celeritas.Scriptables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// Manages and controls the creation of game entites.
	/// </summary>
	public class EntityDataManager : Singleton<EntityDataManager>
	{
		private const float CHUNK_SIZE = 100;

		private const int DISABLE_CHUNK_DIST = 3;

		private const int UNLOAD_CHUNK_DIST = 6;

		private const float UPDATE_FREQ = 2;

		private readonly List<ShipData> ships = new List<ShipData>();
		private readonly List<WeaponData> weapons = new List<WeaponData>();
		private readonly List<ModuleData> modules = new List<ModuleData>();
		private readonly List<ModifierSystem> systems = new List<ModifierSystem>();
		private readonly List<ProjectileData> projectiles = new List<ProjectileData>();
		private readonly List<EffectCollection> effectColletions = new List<EffectCollection>();
		private readonly List<ActionData> actions = new List<ActionData>();
		private readonly List<HullData> hulls = new List<HullData>();
		private readonly List<WaveData> waves = new List<WaveData>();
		private readonly List<ShipData> playerShips = new List<ShipData>();
		private readonly List<EntityData> enviormentEntities = new List<EntityData>();

		/// <summary>
		/// All loaded ship data entites.
		/// </summary>
		public IReadOnlyList<ShipData> Ships { get => ships.AsReadOnly(); }

		/// <summary>
		/// All loaded weapon entites.
		/// </summary>
		public IReadOnlyList<WeaponData> Weapons { get => weapons.AsReadOnly(); }

		/// <summary>
		/// All loaded module entites.
		/// </summary>
		public IReadOnlyList<ModuleData> Modules { get => modules.AsReadOnly(); }

		/// <summary>
		/// All loaded system entites.
		/// </summary>
		public IReadOnlyList<ModifierSystem> Systems { get => systems.AsReadOnly(); }

		/// <summary>
		/// All loaded projectile entites.
		/// </summary>
		public IReadOnlyList<ProjectileData> Projectiles { get => projectiles.AsReadOnly(); }

		/// <summary>
		/// All loaded effect collection entites.
		/// </summary>
		public IReadOnlyList<EffectCollection> EffectCollections { get => effectColletions.AsReadOnly(); }

		/// <summary>
		/// All the action entites.
		/// </summary>
		public IReadOnlyList<ActionData> Actions { get => actions.AsReadOnly(); }

		/// <summary>
		/// All the hull entites.
		/// </summary>
		public IReadOnlyList<HullData> Hulls { get => hulls.AsReadOnly(); }

		/// <summary>
		/// All the wave entites.
		/// </summary>
		public IReadOnlyList<WaveData> Waves { get => waves.AsReadOnly(); }

		/// <summary>
		/// All the asteroid entites.
		/// </summary>
		public IReadOnlyList<EntityData> EnviromentEntities { get => enviormentEntities.AsReadOnly(); }

		/// <summary>
		/// All the player ship entries.
		/// </summary>
		public IReadOnlyList<ShipData> PlayerShips { get => playerShips.AsReadOnly(); }

		/// <summary>
		/// Has the game loaded its assets.
		/// </summary>
		public bool Loaded { get; private set; } = false;

		/// <summary>
		/// The chunk manager for the game.
		/// </summary>
		public static ChunkManager ChunkManager { get; private set; }

		public static event Action OnLoadedAssets;

		public static event Action<Entity> OnCreatedEntity;

		public static event Action<Chunk> OnCreatedChunk;

		private static Dictionary<EntityData, ObjectPool<Entity>> entites = new Dictionary<EntityData, ObjectPool<Entity>>();

		private float lastUpdate;

		private new Camera camera;

		protected override void Awake()
		{
			base.Awake();
			camera = Camera.main;
			ChunkManager = new ChunkManager(new Vector2(CHUNK_SIZE, CHUNK_SIZE));
			LoadAssets();
		}

		private void FixedUpdate()
		{
			if (!Instance.Loaded || Time.unscaledTime < lastUpdate + (1f / UPDATE_FREQ))
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
		}

		/// <summary>
		/// Create an entity.
		/// </summary>
		/// <typeparam name="T">The entity type to create.</typeparam>
		/// <param name="data">The data template to use.</param>
		/// <param name="position">The position to spawn the entity with.</param>
		/// <param name="rotation">The rotation to spawn the entity with.</param>
		/// <param name="owner">The owner entity</param>
		/// <param name="effects">The effects to start this entity with.</param>
		/// <param name="forceIsPlayer">Force this entity to be a player entity.</param>
		/// <returns>Returns the created entity.</returns>
		public static T InstantiateEntity<T>(EntityData data, Vector3 position, Quaternion rotation, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false) where T : Entity
		{
			if (!entites.ContainsKey(data))
			{
				entites[data] = new ObjectPool<Entity>(data.CapacityHint, data.Prefab, Instance.transform);
			}

			var entity = entites[data].GetPooledObject().GetComponent<T>();

			entity.transform.position = position;
			entity.transform.rotation = rotation;

			entity.Initalize(data, owner, effects, forceIsPlayer);
			OnCreatedEntity?.Invoke(entity);

			if (data.UseChunking)
			{
				if (ChunkManager.TryGetChunk(position, out var chunk))
				{
					chunk.AddEntity(entity);
				}
				else
				{
					UnityEngine.Debug.LogError($"Tried to spawn \"{data.Title}\" in a chunk which does not exist ({ChunkManager.GetChunkIndex(position).x}, {ChunkManager.GetChunkIndex(position).y}). " +
						$"Check spawn logic or disable \"useChunking\" for this object.", entity.gameObject);
				}
			}

			return entity;
		}

		/// <summary>
		/// Create an entity.
		/// </summary>
		/// <typeparam name="T">The entity type to create.</typeparam>
		/// <param name="data">The data template to use.</param>
		/// <param name="position">The position to spawn the entity with.</param>
		/// <param name="owner">The owner entity</param>
		/// <param name="effects">The effects to start this entity with.</param>
		/// <param name="forceIsPlayer">Force this entity to be a player entity.</param>
		/// <returns>Returns the created entity.</returns>
		public static T InstantiateEntity<T>(EntityData data, Vector3 position, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false) where T : Entity
		{
			return InstantiateEntity<T>(data, position, Quaternion.identity, owner, effects, forceIsPlayer);
		}

		/// <summary>
		/// Create an entity.
		/// </summary>
		/// <typeparam name="T">The entity type to create.</typeparam>
		/// <param name="data">The data template to use.</param>
		/// <param name="owner">The owner entity</param>
		/// <param name="effects">The effects to start this entity with.</param>
		/// <param name="forceIsPlayer">Force this entity to be a player entity.</param>
		/// <returns>Returns the created entity.</returns>
		public static T InstantiateEntity<T>(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false) where T : Entity
		{
			return InstantiateEntity<T>(data, Vector3.zero, Quaternion.identity, owner, effects, forceIsPlayer);
		}

		/// <summary>
		/// Unload this entity, firing no events.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		public static void UnloadEntity(Entity entity)
		{
			if (entites.ContainsKey(entity.Data))
			{
				entites[entity.Data].ReleasePooledObject(entity);
			}
			else
			{
				entity.OnDespawned();
				Destroy(entity.gameObject);
			}
		}

		/// <summary>
		/// Destroy the provided entity gracefully, recycling it if nessecary.
		/// </summary>
		/// <param name="entity">The entity to destroy.</param>
		public static void KillAndUnloadEntity(Entity entity)
		{
			entity.OnEntityKilled();
			UnloadEntity(entity);
		}

		/// <summary>
		/// Find and return all entities of the provided type.
		/// </summary>
		/// <typeparam name="T">The type to find.</typeparam>
		/// <returns>Returns a read only list of all found entities.</returns>
		public static IReadOnlyList<T> GetEntities<T>() where T : Entity
		{
			var found = new List<T>();
			foreach (var data in entites)
			{
				if (typeof(T).IsAssignableFrom(data.Key.EntityType) && data.Value.ActiveObjects != null)
				{
					foreach (var entity in data.Value.ActiveObjects)
					{
						found.Add(entity as T);
					}
				}
			}
			return found.AsReadOnly();
		}

		private async void LoadAssets()
		{
			Loaded = false;

			Stopwatch watch = new Stopwatch();
			watch.Start();

			var tasks = new Task[]
			{
				LoadTags(ships, Constants.SHIP_TAG),
				LoadTags(weapons, Constants.WEAPON_TAG),
				LoadTags(modules, Constants.MODULE_TAG),
				LoadTags(projectiles, Constants.PROJECTILE_TAG),
				LoadTags(systems, Constants.SYSTEMS_TAG),
				LoadTags(effectColletions, Constants.EFFECTS_TAG),
				LoadTags(hulls, Constants.HULL_TAG),
				LoadTags(actions, Constants.ACTION_TAG),
				LoadTags(waves, Constants.WAVES_TAG),
				LoadTags(playerShips, Constants.PLAYER_SHIP_TAG),
				LoadTags(enviormentEntities, Constants.ENVIRONMENT_TAG),
			};

			await Task.WhenAll(tasks);

			watch.Stop();
			UnityEngine.Debug.Log($"load took: {watch.ElapsedMilliseconds}ms");

			Loaded = true;
			UpdateChunks();
			OnLoadedAssets?.Invoke();
		}

		private async Task LoadTags<T>(IList<T> list, string tag)
		{
			var handle = Addressables.LoadAssetsAsync<T>(tag, (_) => { });

			await handle.Task;

			foreach (var item in handle.Result)
			{
				list.Add(item);

				if (item is EntityData entity)
				{
					var go = new GameObject(string.IsNullOrEmpty(entity.Title) ? typeof(T).Name : entity.Title);
					go.transform.parent = Instance.transform;

					entites[entity] = new ObjectPool<Entity>(entity.CapacityHint, entity.Prefab, go.transform);
				}
			}
		}
	}
}
