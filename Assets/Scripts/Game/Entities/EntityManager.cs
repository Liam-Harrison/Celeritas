using Celeritas.Scriptables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Celeritas.Game.Entities
{
	public class EntityManager : Singleton<EntityManager>
	{
		private readonly List<ShipData> ships = new List<ShipData>();
		private readonly List<WeaponData> weapons = new List<WeaponData>();
		private readonly List<ModuleData> modules = new List<ModuleData>();
		private readonly List<ModifierData> modifiers = new List<ModifierData>();
		private readonly List<ProjectileData> projectiles = new List<ProjectileData>();

		private const string SHIP_TAG = "Ships";
		private const string WEAPON_TAG = "Weapons";
		private const string MODULE_TAG = "Modules";
		private const string PROJECTILE_TAG = "Projectiles";
		private const string MODIFIERS_TAG = "Modifiers";

		public static Action OnLoadedAssets;

		private Transform parent;

		protected override void Awake()
		{
			base.Awake();
			StartCoroutine(LoadAssets());
			parent = new GameObject("Entities").transform;
			parent.transform.position = Vector3.zero;
			parent.transform.rotation = Quaternion.identity;
			parent.transform.localScale = Vector3.one;
		}

		private IEnumerator LoadAssets()
		{
			yield return LoadTags(ships, SHIP_TAG);
			yield return LoadTags(weapons, WEAPON_TAG);
			yield return LoadTags(modules, MODULE_TAG);
			yield return LoadTags(projectiles, PROJECTILE_TAG);
			yield return LoadTags(modifiers, MODIFIERS_TAG);

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

		public static T InstantiateEntity<T>(EntityData data) where T: Entity
		{
			var comp = Instantiate(data.Prefab).GetComponent<T>();
			comp.Initalize(data);
			return comp;
		}
	}
}
