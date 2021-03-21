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
		/// An event which allows an instance to notify subscribers of changes
		/// to its state.
		/// </summary>
		/// <param name="sender">The instance whose state has changed</param>
		public delegate void StateHandeler(T sender);

		/// <summary>
		/// Get the current instance of this class.
		/// </summary>
		public static T Instance
		{
			get => instance;
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
		public static event StateHandeler OnCreated;

		/// <summary>
		/// An event which is fired when the instance is destroyed.
		/// </summary>
		public static event StateHandeler OnDestroyed;

		private static T instance = null;

		protected virtual void Awake()
		{
			if (instance != null)
			{
				Debug.LogError($"Created an instance of {nameof(T)} when an instance was still registered.");
			}
			else
			{
				instance = this as T;
				OnCreated?.Invoke(instance);
			}
		}

		protected virtual void OnDestroy()
		{
			if (instance == this)
			{
				OnDestroyed?.Invoke(instance);
				instance = null;
			}
		}
	}
}

