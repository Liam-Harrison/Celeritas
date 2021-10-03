using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	/// <summary>
	/// An interface for registering for the entity destroyed event.
	/// </summary>
	public interface IEntityBeforeDie
	{
		/// <summary>
		/// Called when the entity is scheduled to be destroyed.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="wrapper">The level of this effect.</param>
		void OnEntityBeforeDie(Entity entity, EffectWrapper wrapper);
	}
}
