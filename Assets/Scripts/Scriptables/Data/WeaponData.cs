using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a weapon.
	/// </summary>
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Celeritas/New Weapon", order = 30)]
	public class WeaponData : ModuleData
	{
		[SerializeField, Title("Projectile")] private ProjectileData projectile;
		[SerializeField, Title("RateOfFire")] private uint rateOfFire;

		[SerializeField, Title("Charge")] private bool charge;
		[SerializeField, ShowIf(nameof(charge))] private float maxCharge;

		/// <summary>
		/// Get the projectile attatched to this weapon.
		/// </summary>
		public ProjectileData Projectile { get => projectile; }

		/// <summary>
		/// The speed this weapon fires at.
		/// </summary>
		public uint RateOfFire { get => rateOfFire; }

		/// <summary>
		/// Is this a charge weapon?
		/// </summary>
		public bool Charge { get => charge; }

		/// <summary>
		/// The maximum limit to charge to.
		/// </summary>
		public float MaxCharge { get => maxCharge; }

		protected override void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<WeaponEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(WeaponEntity)} attatched to it.", this);
			}
		}
	}
}
