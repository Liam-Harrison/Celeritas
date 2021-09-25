using Celeritas.Game.Events;
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
		private readonly HashSet<ShipData> ships = new HashSet<ShipData>();
		private readonly HashSet<WeaponData> weapons = new HashSet<WeaponData>();
		private readonly HashSet<ModuleData> modules = new HashSet<ModuleData>();
		private readonly HashSet<ModifierSystem> systems = new HashSet<ModifierSystem>();
		private readonly HashSet<ProjectileData> projectiles = new HashSet<ProjectileData>();
		private readonly HashSet<EffectCollection> effectColletions = new HashSet<EffectCollection>();
		private readonly HashSet<ActionData> actions = new HashSet<ActionData>();
		private readonly HashSet<HullData> hulls = new HashSet<HullData>();
		private readonly HashSet<WaveData> waves = new HashSet<WaveData>();
		private readonly HashSet<ShipData> playerShips = new HashSet<ShipData>();
		private readonly HashSet<EntityData> enviormentEntities = new HashSet<EntityData>();
		private readonly HashSet<EventData> events = new HashSet<EventData>();

		/// <summary>
		/// All loaded ship data entites.
		/// </summary>
		public IReadOnlyCollection<ShipData> Ships { get => ships; }

		/// <summary>
		/// All loaded weapon entites.
		/// </summary>
		public IReadOnlyCollection<WeaponData> Weapons { get => weapons; }

		/// <summary>
		/// All loaded module entites.
		/// </summary>
		public IReadOnlyCollection<ModuleData> Modules { get => modules; }

		/// <summary>
		/// All loaded system entites.
		/// </summary>
		public IReadOnlyCollection<ModifierSystem> Systems { get => systems; }

		/// <summary>
		/// All loaded projectile entites.
		/// </summary>
		public IReadOnlyCollection<ProjectileData> Projectiles { get => projectiles; }

		/// <summary>
		/// All loaded effect collection entites.
		/// </summary>
		public IReadOnlyCollection<EffectCollection> EffectCollections { get => effectColletions; }

		/// <summary>
		/// All the action entites.
		/// </summary>
		public IReadOnlyCollection<ActionData> Actions { get => actions; }

		/// <summary>
		/// All the hull entites.
		/// </summary>
		public IReadOnlyCollection<HullData> Hulls { get => hulls; }

		/// <summary>
		/// All the wave entites.
		/// </summary>
		public IReadOnlyCollection<WaveData> Waves { get => waves; }

		/// <summary>
		/// All the asteroid entites.
		/// </summary>
		public IReadOnlyCollection<EntityData> EnviromentEntities { get => enviormentEntities; }

		/// <summary>
		/// All the player ship entries.
		/// </summary>
		public IReadOnlyCollection<ShipData> PlayerShips { get => playerShips; }

		public IReadOnlyCollection<EventData> Events { get => events; }

		public bool Loaded { get; private set; }

		public static event Action OnLoadedAssets;

		public static event Action<Entity> OnCreatedEntity;

		private static Dictionary<EntityData, ObjectPool<Entity>> entites = new Dictionary<EntityData, ObjectPool<Entity>>();

		protected override void Awake()
		{
			base.Awake();
			LoadAssets();
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
		public static T InstantiateEntity<T>(EntityData data, Vector3 position, Quaternion rotation, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool dontPool = false) where T : Entity
		{
			if (!entites.ContainsKey(data))
			{
				entites[data] = new ObjectPool<Entity>(data.CapacityHint, data.Prefab, Instance.transform);
			}

			T entity;
			if (dontPool)
			{
				entity = entites[data].CreateUnpooledObject().GetComponent<T>();
			}
			else
			{
				entity = entites[data].GetPooledObject().GetComponent<T>();
			}

			entity.transform.position = position;
			entity.transform.rotation = rotation;

			entity.Initalize(data, owner, effects, forceIsPlayer);
			OnCreatedEntity?.Invoke(entity);

			if (data.UseChunking)
			{
				if (Chunks.ChunkManager.TryGetChunk(position, out var chunk))
				{
					chunk.AddEntity(entity);
				}
				else
				{
					UnityEngine.Debug.LogError($"Tried to spawn \"{data.Title}\" in a chunk which does not exist ({Chunks.ChunkManager.GetChunkIndex(position).x}, {Chunks.ChunkManager.GetChunkIndex(position).y}). " +
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
		public static T InstantiateEntity<T>(EntityData data, Vector3 position, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool dontPool = false) where T : Entity
		{
			return InstantiateEntity<T>(data, position, Quaternion.identity, owner, effects, forceIsPlayer, dontPool);
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
		public static T InstantiateEntity<T>(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool dontPool = false) where T : Entity
		{
			return InstantiateEntity<T>(data, Vector3.zero, Quaternion.identity, owner, effects, forceIsPlayer, dontPool);
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
		/// Unload all the entities in the game.
		/// </summary>
		public static void UnloadAllEntities()
		{
			foreach (var op in entites)
			{
				op.Value.ReleaseAllObjects();
			}
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
				LoadTags(events, Constants.EVENT_TAG),
			};

			await Task.WhenAll(tasks);

			watch.Stop();
			UnityEngine.Debug.Log($"load took: {watch.ElapsedMilliseconds}ms");

			Loaded = true;
			OnLoadedAssets?.Invoke();
		}

		private async Task LoadTags<T>(ICollection<T> list, string tag)
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
