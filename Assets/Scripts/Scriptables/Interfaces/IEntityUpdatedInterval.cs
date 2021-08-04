using Celeritas.Game;

namespace Assets.Scripts.Scriptables.Interfaces
{
	/// <summary>
	/// Called when the entity is updated after a set period.
	/// Time period is defined in Entity.TIME_BETWEEN_INTERVALS_S
	/// </summary>
	public interface IEntityUpdatedInterval
	{
		/// <summary>
		/// Called when the entity is updated, each time a set time interval has passed.
		/// Time interval is defined in Entity.TIME_BETWEEN_INTERVALS_S
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="level"></param>
		void OnEntityUpdatedAfterInterval(Entity entity, ushort level);
	}
}
