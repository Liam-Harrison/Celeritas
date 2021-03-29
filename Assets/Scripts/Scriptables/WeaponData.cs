using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a weapon.
	/// </summary>
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Celeritas/New Weapon", order = 30)]
	public class WeaponData : ModuleData
	{
		[SerializeField, Title("Projectile")] private ProjectileData projectile;

		[SerializeField, Title("Aimming")] private bool aims;
		[SerializeField, ShowIf(nameof(aims))] private float aimSpeed;

		/// <summary>
		/// Get the projectile attatched to this weapon.
		/// </summary>
		public ProjectileData Projectile { get => projectile; }

		/// <summary>
		/// Get if this weapon aims or not.
		/// </summary>
		public bool Aims { get => aims; }

		/// <summary>
		/// The speed this weapon aims at.
		/// </summary>
		public float AimSpeed { get => aimSpeed; }

		protected override void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<WeaponEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(WeaponEntity)} attatched to it.", this);
			}
		}
	}
}
