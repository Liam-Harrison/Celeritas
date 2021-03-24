using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using UnityEngine.AddressableAssets;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a weapon.
	/// </summary>
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Celeritas/New Weapon", order = 30)]
	public class WeaponData : ModuleData
	{
		[SerializeField] private AssetReference projectile;

		protected override void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<WeaponEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(WeaponEntity)} attatched to it.", this);
			}
		}
	}
}
