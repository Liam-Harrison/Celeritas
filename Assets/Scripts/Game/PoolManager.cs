using UnityEngine;

namespace Celeritas.Game
{
	class PoolManager : Singleton<PoolManager>
	{
		public GameObject Instantiate(GameObject prefab, Transform transform)
		{
			return Instantiate(prefab, transform);
		}

		public void Destroy(GameObject item)
		{
			Destroy(item);
		}
	}
}
