using Celeritas.Extensions;
using Celeritas.Scriptables;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	public class WeaponEntity : ModuleEntity
	{
		[SerializeField]
		private Transform projectileSpawn;

		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		public override void Initalize(ScriptableObject data)
		{
			WeaponData = data as WeaponData;
			base.Initalize(data);
		}

		/// <summary>
		/// Fire the provided projectile from this weapon.
		/// </summary>
		public void Fire()
		{
			var projectile = EntityManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile);
			projectile.transform.CopyTransform(projectileSpawn);
			projectile.SetOwner(this);
		}
	}
}
