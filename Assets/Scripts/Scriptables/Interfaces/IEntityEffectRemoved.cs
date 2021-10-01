using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity effect removed event.
	/// </summary>
	public interface IEntityEffectRemoved
	{
		/// <summary>
		/// Called when the effect is removed from the provided entity.
		/// </summary>
		/// <param name="entity">The entity which removed this effect.</param>
		/// <param name="wrapper">The level of the effect.</param>
		void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper);
	}
}
