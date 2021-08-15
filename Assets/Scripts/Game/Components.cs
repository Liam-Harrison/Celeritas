using Celeritas.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
	public class Components
	{
		private readonly Dictionary<SystemComponent, object> components = new Dictionary<SystemComponent, object>();

		public void RegisterComponent<T>(ModifierSystem system, T component)
		{
			var sc = GetSystemComponent<T>(system);

			if (components.ContainsKey(sc))
			{
				Debug.LogError($"System \"{system.name}\" is trying to add component \"{typeof(T).Name}\" to an entity which already has this component.", system);
				return;
			}

			components.Add(sc, component);
		}

		public bool TryGetComponent<T>(ModifierSystem system, out T component) where T: class
		{
			component = GetComponent<T>(system);
			return component != null;
		}

		public void ReleaseComponent<T>(ModifierSystem system)
		{
			var sc = GetSystemComponent<T>(system);
			if (components.ContainsKey(sc))
				components.Remove(sc);
		}

		public bool HasComponent<T>(ModifierSystem system)
		{
			return components.ContainsKey(GetSystemComponent<T>(system));
		}

		public T GetComponent<T>(ModifierSystem system) where T: class
		{
			var sc = GetSystemComponent<T>(system);
			if (components.ContainsKey(sc))
				return components[sc] as T;

			return null;
		}

		private SystemComponent GetSystemComponent<T>(ModifierSystem system)
		{
			return new SystemComponent(system, typeof(T));
		}

		private struct SystemComponent
		{
			public SystemComponent(ModifierSystem system, Type componentType)
			{
				this.system = system;
				this.componentType = componentType;
			}

			ModifierSystem system;
			Type componentType;

			public override bool Equals(object obj)
			{
				if (obj == null)
					return false;

				if (obj is SystemComponent s)
				{
					return system.Equals(s.system) && componentType.Equals(s.componentType);
				}
				else
					return false;
			}

			public override int GetHashCode()
			{
				int hashCode = 1824197668;
				hashCode = hashCode * -1521134295 + EqualityComparer<ModifierSystem>.Default.GetHashCode(system);
				hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(componentType);
				return hashCode;
			}
		}
	}
}