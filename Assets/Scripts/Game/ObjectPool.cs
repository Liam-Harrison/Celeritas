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
	public class ObjectPool<T> where T : MonoBehaviour, IPooledObject<T>
	{
		[SerializeField, Title("Pooled Object Settings"), DisableInPlayMode]
		private int capacity = 16;

		private readonly List<T> pool = new List<T>();
		private readonly List<T> active = new List<T>();
		private readonly HashSet<T> unpooled = new HashSet<T>();

		private int _capacity;

		public int Capacity
		{
			get
			{
				return _capacity;
			}
			set
			{
				_capacity = value;
			}
		}

		/// <summary>
		/// Get all the current active objects in this pool.
		/// </summary>
		public IReadOnlyList<T> ActiveObjects { get => active; }

		private GameObject prefab;
		private Transform parent;

		public ObjectPool(int capacity, GameObject prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;

			Capacity = capacity;

			Setup();
		}

		public ObjectPool(GameObject prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;

			Capacity = capacity;

			Setup();
		}

		public ObjectPool()
		{
			Setup();
		}

		private void Setup()
		{
			for (int i = 0; i < Capacity; i++)
			{
				var item = Object.Instantiate(prefab, parent).GetComponent<T>();
				item.gameObject.SetActive(false);
				item.OwningPool = this;
				pool.Add(item);
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
				var item = Object.Instantiate(prefab, parent).GetComponent<T>();
				item.OwningPool = this;
				item.gameObject.SetActive(true);
				active.Add(item);
				item.OnSpawned();
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

		public T CreateUnpooledObject()
		{
			var item = Object.Instantiate(prefab, parent).GetComponent<T>();
			item.OwningPool = this;
			item.gameObject.SetActive(true);
			unpooled.Add(item);
			return item;
		}

		public void ReleaseAllObjects()
		{
			foreach (var item in active)
			{
				pool.Add(item);
				item.OnDespawned();
				item.transform.parent = parent;
			}
			foreach (var item in unpooled)
			{
				item.OnDespawned();
				Object.Destroy(item.gameObject);
			}

			active.Clear();
			unpooled.Clear();
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
				item.transform.parent = parent;
			}
			else if (pool.Contains(item) == false)
			{
				item.OnDespawned();
				active.Remove(item);

				if (unpooled.Contains(item))
					unpooled.Remove(item);

				Object.Destroy(item.gameObject);
			}
		}
	}
}
