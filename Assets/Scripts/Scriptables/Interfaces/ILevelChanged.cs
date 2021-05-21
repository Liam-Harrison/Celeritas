using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity destroyed event.
	/// </summary>
	public interface ILevelChanged
	{
		/// <summary>
		/// Called when the effects level changes.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="previous">The previous level.</param>
		/// <param name="level">The new level</param>
		void OnLevelChanged(Entity entity, ushort previous, ushort level);
	}
}
