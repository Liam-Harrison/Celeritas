using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity hit event.
	/// </summary>
	public interface IEntityHit
	{
		/// <summary>
		/// Called when the entity is hit or hits another entity.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="other">The other entity hit.</param>
		/// <param name="wrapper">The level of this effect.</param>
		void OnEntityHit(Entity entity, Entity other, EffectWrapper wrapper);
	}
}
