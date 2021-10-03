using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity update event.
	/// </summary>
	public interface IEntityUpdated
	{
		/// <summary>
		/// Called when the entity is updated.
		/// </summary>
		/// <param name="entity">The entity which is updating.</param>
		/// <param name="wrapper">The level of the effect.</param>
		void OnEntityUpdated(Entity entity, EffectWrapper wrapper);
	}
}
