using Celeritas.Game.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Allows for the pooling and access of various objects.
	/// </summary>
	[System.Serializable]
	public class ObjectPool<T> where T: MonoBehaviour, IPooledObject
	{
		[SerializeField, Title("Pooled Object Settings"), DisableInPlayMode]
		private uint capacity = 16;

		private readonly List<T> pool = new List<T>();
		private readonly List<T> active = new List<T>();

		/// <summary>
		/// Get all the current active objects in this pool.
		/// </summary>
		public IReadOnlyList<T> ActiveObjects { get => active.AsReadOnly(); }

		private GameObject prefab;
		private Transform parent;

		public ObjectPool(GameObject prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;

			pool.Capacity = (int)capacity;
			active.Capacity = (int)capacity;

			for (int i = 0; i < capacity; i++)
			{
				var item = PoolManager.Instance.Instantiate(prefab, parent);
				item.SetActive(false);
				pool.Add(item.GetComponent<T>());
			}
		}

		/// <summary>
		/// Get a free object from the pool.
		/// </summary>
		/// <returns>The avaliable object from the pool.</returns>
		public T GetPooledObject()
		{
			if (pool.Count == 0)
			{
				var item = PoolManager.Instance.Instantiate(prefab, parent).GetComponent<T>();
				item.gameObject.SetActive(true);
				item.OnSpawned();
				active.Add(item);
				return item;
			}
			else
			{
				var item = pool[0];
				pool.RemoveAt(0);
				active.Add(item);
				item.gameObject.SetActive(true);
				item.OnSpawned();
				return item;
			}
		}

		/// <summary>
		/// Release an object and return it to the pool.
		/// </summary>
		/// <param name="item">The object to release.</param>
		public void ReleasePooledObject(T item)
		{
			item.gameObject.SetActive(false);
			if (active.Contains(item))
			{
				active.Remove(item);
				pool.Add(item);
				item.OnDespawned();
			}
			else if (pool.Contains(item))
			{
				Debug.Log($"Attempted to release pool item which was already released", item.gameObject);
			}
			else
			{
				item.OnDespawned();
				PoolManager.Instance.Destroy(item.gameObject);
			}
		}
	}
}
