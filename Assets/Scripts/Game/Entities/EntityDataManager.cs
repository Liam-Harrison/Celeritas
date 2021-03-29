using Celeritas.Scriptables;
using System;
using System.Collections;
using System.Collections.Generic;
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

		public static Action OnLoadedAssets;

		protected override void Awake()
		{
			base.Awake();
			StartCoroutine(LoadAssets());
		}

		/// <summary>
		/// Create and initalize the entity of a specified type.
		/// </summary>
		/// <typeparam name="T">The <seealso cref="Entity"/> type to create.</typeparam>
		/// <param name="data">The data to attatch to the entity.</param>
		/// <returns>Returns the created and initalized entity.</returns>
		public static T InstantiateEntity<T>(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null) where T: Entity
		{
			var comp = Instantiate(data.Prefab).GetComponent<T>();
			comp.Initalize(data, owner, effects);
			return comp;
		}

		private IEnumerator LoadAssets()
		{
			yield return LoadTags(ships, Constants.SHIP_TAG);
			yield return LoadTags(weapons, Constants.WEAPON_TAG);
			yield return LoadTags(modules, Constants.MODULE_TAG);
			yield return LoadTags(projectiles, Constants.PROJECTILE_TAG);
			yield return LoadTags(systems, Constants.SYSTEMS_TAG);
			yield return LoadTags(effectColletions, Constants.EFFECTS_TAG);

			OnLoadedAssets?.Invoke();
		}

		private IEnumerator LoadTags<T>(IList<T> list, string tag)
		{
			var handle = Addressables.LoadAssetsAsync<T>(tag, (_) => { });

			yield return handle;

			if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				foreach (var item in handle.Result)
				{
					list.Add(item);
				}
			}
		}
	}
}
