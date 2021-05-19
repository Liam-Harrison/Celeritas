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
		private readonly List<Asteroid> asteroids = new List<Asteroid>();
		private readonly List<LootEntity> droppedLoot = new List<LootEntity>();

		/// <summary>
		/// All loaded ship data entries.
		/// </summary>
		public IReadOnlyList<ShipData> Ships { get => ships.AsReadOnly(); }

		/// <summary>
		/// All loaded weapon entries.
		/// </summary>
		public IReadOnlyList<WeaponData> Weapons { get => weapons.AsReadOnly(); }

		/// <summary>
		/// All loaded module entries.
		/// </summary>
		public IReadOnlyList<ModuleData> Modules { get => modules.AsReadOnly(); }

		/// <summary>
		/// All loaded system entries.
		/// </summary>
		public IReadOnlyList<ModifierSystem> Systems { get => systems.AsReadOnly(); }

		/// <summary>
		/// All loaded projectile entries.
		/// </summary>
		public IReadOnlyList<ProjectileData> Projectiles { get => projectiles.AsReadOnly(); }

		/// <summary>
		/// All loaded effect collection entries.
		/// </summary>
		public IReadOnlyList<EffectCollection> EffectCollections { get => effectColletions.AsReadOnly(); }

		/// <summary>
		/// All the action entries.
		/// </summary>
		public IReadOnlyList<ActionData> Actions { get => actions.AsReadOnly(); }

		/// <summary>
		/// All the hull entries.
		/// </summary>
		public IReadOnlyList<HullData> Hulls { get => hulls.AsReadOnly(); }

		/// <summary>
		/// All the wave entries.
		/// </summary>
		public IReadOnlyList<WaveData> Waves { get => waves.AsReadOnly(); }

		public IReadOnlyList<Asteroid> Asteroids { get => asteroids.AsReadOnly(); }

		public IReadOnlyList<LootEntity> DroppedLoot { get => droppedLoot.AsReadOnly(); }

		/// <summary>
		/// All the player ship entries.
		/// </summary>
		public IReadOnlyList<ShipData> PlayerShips { get => playerShips.AsReadOnly(); }

		public static event Action OnLoadedAssets;

		public static event Action<Entity> OnCreatedEntity;

		public bool Loaded { get; private set; } = false;

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
			var entity = Instantiate(data.Prefab, Instance.transform).GetComponent<T>();
			entity.Initalize(data, owner, effects, forceIsPlayer);
			OnCreatedEntity?.Invoke(entity);
			return entity;
		}

		private async void LoadAssets()
		{
			Loaded = false;

			Stopwatch watch = new Stopwatch();
			watch.Start();

			await LoadTags(ships, Constants.SHIP_TAG);
			await LoadTags(weapons, Constants.WEAPON_TAG);
			await LoadTags(modules, Constants.MODULE_TAG);
			await LoadTags(projectiles, Constants.PROJECTILE_TAG);
			await LoadTags(systems, Constants.SYSTEMS_TAG);
			await LoadTags(effectColletions, Constants.EFFECTS_TAG);
			await LoadTags(hulls, Constants.HULL_TAG);
			await LoadTags(actions, Constants.ACTION_TAG);
			await LoadTags(waves, Constants.WAVES_TAG);
			await LoadTags(playerShips, Constants.PLAYER_SHIP_TAG);

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
			}
		}
	}
}
