using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity creation event.
	/// </summary>
	public interface IEntityCreated
	{
		/// <summary>
		/// Called when the entity is created.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		void OnEntityCreated(Entity entity);
	}
}
