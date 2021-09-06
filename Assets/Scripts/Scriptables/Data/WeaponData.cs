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
		[SerializeField, ShowIf(nameof(charge))] private bool autofire;
		[SerializeField, Title("Placeholder", "used for player weapons in ship selection menu")] private bool placeholder;

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

		/// <summary>
		/// Used to ensure that players manually select their weapon in the selection menu
		/// The vast majority of the time, this should be false
		/// </summary>
		public bool Placeholder { get => placeholder; set => placeholder = value; }

		/// <summary>
		/// For charge weapons only: does this weapon automatically fire when at max charge?
		/// </summary>
		public bool Autofire { get => autofire; }

		protected override void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<WeaponEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(WeaponEntity)} attatched to it.", this);
			}
		}
	}
}
