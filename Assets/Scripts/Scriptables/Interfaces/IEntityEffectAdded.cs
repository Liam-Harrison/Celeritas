using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity effect added event.
	/// </summary>
	public interface IEntityEffectAdded
	{
		/// <summary>
		/// Called when the effect is added to the provided entity.
		/// </summary>
		/// <param name="entity">The entity which added this effect.</param>
		/// <param name="level">The level of the effect.</param>
		void OnEntityEffectAdded(Entity entity, ushort level);
	}
}
