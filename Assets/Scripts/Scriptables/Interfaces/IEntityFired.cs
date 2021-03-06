using Celeritas.Game;
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
		/// <param name="projectile">The fired projectile entity.</param>
		/// <param name="weapper">The level of this effect.</param>
		void OnEntityFired(WeaponEntity entity, ProjectileEntity projectile, EffectWrapper wrapper);
	}
}
