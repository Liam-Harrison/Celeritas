using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	public interface IEntityResetable
	{
		void OnReset(Entity entity, EffectWrapper wrapper);
	}
}
