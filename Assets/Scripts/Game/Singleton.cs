using Celeritas.Game.Entities;
using System;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Registers and manages a single instance of your class type, also
	/// manages and fires events that are relevant to your classes lifecycle.
	/// </summary>
	/// <typeparam name="T">Type of your class instance.</typeparam>
	public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
	{
		/// <summary>
		/// Get the current instance of this class.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
				}
				return instance;
			}
		}

		/// <summary>
		/// Check if the instance exists. Returns true if an instance exists, otherwise false.
		/// </summary>
		public static bool Exists
		{
			get => instance != null;
		}

		/// <summary>
		/// An event which is fired when the instance is created.
		/// </summary>
		public static event Action<T> OnCreated;

		/// <summary>
		/// An event which is fired when the instance is destroyed.
		/// </summary>
		public static event Action<T> OnDestroyed;

		private static T instance = null;

		protected virtual void Awake()
		{
			instance = this as T;

			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				OnGameLoaded();
			else
				EntityDataManager.OnLoadedAssets += OnGameLoaded;
		}

		protected virtual void OnDestroy()
		{
			if (instance == this)
			{
				OnDestroyed?.Invoke(instance);
				instance = null;
			}
		}

		protected virtual void OnGameLoaded()
		{
			EntityDataManager.OnLoadedAssets -= OnGameLoaded;
			OnCreated?.Invoke(instance);
		}
	}
}
