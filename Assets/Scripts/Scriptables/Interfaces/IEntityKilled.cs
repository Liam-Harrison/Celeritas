using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity destroyed event.
	/// </summary>
	public interface IEntityKilled
	{
		/// <summary>
		/// Called when the entity is destroyed.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="level">The level of this effect.</param>
		void OnEntityKilled(Entity entity, ushort level);
	}
}
