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
		/// <param name="target">The target entity.</param>
		void OnEntityUpdated(Entity entity);
	}
}
