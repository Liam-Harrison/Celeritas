namespace Celeritas.Game.Interfaces
{
	public interface IPooledObject
	{
		void OnSpawned();

		void OnDespawned();
	}
}
