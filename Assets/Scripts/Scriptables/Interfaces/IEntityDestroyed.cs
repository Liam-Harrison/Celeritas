using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity destroyed event.
	/// </summary>
	public interface IEntityDestroyed
	{
		/// <summary>
		/// Called when the entity is destroyed.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="level">The level of this effect.</param>
		void OnEntityDestroyed(Entity entity, ushort level);
	}
}
