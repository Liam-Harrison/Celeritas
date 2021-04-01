using Celeritas.Game.Entities;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the weapon fired event.
	/// </summary>
	public interface IEntityFired
	{
		/// <summary>
		/// Called when the weapon entity is fired.
		/// </summary>
		/// <param name="entity">The target weapon entity.</param>
		/// <param name="level">The level of this effect.</param>
		void OnEntityFired(WeaponEntity entity, ProjectileEntity projectile, ushort level);
	}
}
