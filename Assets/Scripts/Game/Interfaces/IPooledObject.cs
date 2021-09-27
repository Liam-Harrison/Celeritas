using UnityEngine;

namespace Celeritas.Game.Interfaces
{
	public interface IPooledObject<T> where T : MonoBehaviour, IPooledObject<T>
	{
		void OnSpawned();

		void OnDespawned();

		ObjectPool<T> OwningPool { get; set; }
	}
}
