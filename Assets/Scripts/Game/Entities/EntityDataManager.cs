using Celeritas.Scriptables;
using System;
using System.Collections;
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

		public static event Action OnLoadedAssets;

		public static event Action<Entity> OnCreatedEntity;

		private static Dictionary<EntityData, ObjectPool<Entity>> entites = new Dictionary<EntityData, ObjectPool<Entity>>();

		protected override void Awake()
		{
			base.Awake();
			LoadAssets();
		}

		/// <summary>
		/// Create and initalize the entity of a specified type.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="Entity"/> type to create.</typeparam>
		/// <param name="data">The data to attatch to the entity.</param>
		/// <returns>Returns the created and initalized entity.</returns>
		public static T InstantiateEntity<T>(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false) where T: Entity
		{
			if (!entites.ContainsKey(data))
			{
				entites[data] = new ObjectPool<Entity>(data.CapacityHint, data.Prefab, Instance.transform);
			}

			var entity = entites[data].GetPooledObject().GetComponent<T>();

			entity.Initalize(data, owner, effects, forceIsPlayer);
			OnCreatedEntity?.Invoke(entity);

			return entity;
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
		public static void KillEntity(Entity entity)
		{
			entity.OnEntityKilled();
			UnloadEntity(entity);
		}

		/// <summary>
		/// Find and return all entities of the provided type.
		/// </summary>
		/// <typeparam name="T">The type to find.</typeparam>
		/// <returns>Returns a read only list of all found entities.</returns>
		public static IReadOnlyList<T> GetEntities<T>() where T: Entity
		{
			var found = new List<T>();
			foreach (var data in entites)
			{
				if (typeof(T).IsAssignableFrom(data.Key.EntityType))
				{
					found.AddRange(data.Value.ActiveObjects as T[]);
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
