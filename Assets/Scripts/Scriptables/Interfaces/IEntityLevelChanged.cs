using Celeritas.Game;

namespace Celeritas.Scriptables.Interfaces
{
	public interface IEntityLevelChanged
	{
		void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper);
	}
}
