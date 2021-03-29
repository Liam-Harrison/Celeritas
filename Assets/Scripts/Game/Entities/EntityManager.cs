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
	public class EntityManager : Singleton<EntityManager>
	{
		private readonly List<ShipData> ships = new List<ShipData>();
		private readonly List<WeaponData> weapons = new List<WeaponData>();
		private readonly List<ModuleData> modules = new List<ModuleData>();
		private readonly List<ModifierData> modifiers = new List<ModifierData>();
		private readonly List<ProjectileData> projectiles = new List<ProjectileData>();

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
		public static T InstantiateEntity<T>(EntityData data) where T: Entity
		{
			var comp = Instantiate(data.Prefab).GetComponent<T>();
			comp.Initalize(data);
			return comp;
		}

		private IEnumerator LoadAssets()
		{
			yield return LoadTags(ships, Constants.SHIP_TAG);
			yield return LoadTags(weapons, Constants.WEAPON_TAG);
			yield return LoadTags(modules, Constants.MODULE_TAG);
			yield return LoadTags(projectiles, Constants.PROJECTILE_TAG);
			yield return LoadTags(modifiers, Constants.MODIFIERS_TAG);

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
