using System.Reflection;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Extensions for system Types and reflection.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Get all the instanced methods of a type.
		/// </summary>
		/// <param name="type">The type to search through.</param>
		/// <param name="publicOnly">If true this will only return public methods, otherwise it returns all methods.</param>
		/// <returns>Returns a list of instanced methods from the provided type.</returns>
		public static MethodInfo[] GetInstancedMethods(this System.Type type, bool publicOnly = false)
		{
			if (publicOnly) return type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			else return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		/// <summary>
		/// Get all the static methods of a type.
		/// </summary>
		/// <param name="type">The type to search through.</param>
		/// <param name="publicOnly">If true this will only return public methods, otherwise it returns all methods.</param>
		/// <returns>Returns a list of static methods from the provided type.</returns>
		public static MethodInfo[] GetStaticMethods(this System.Type type, bool publicOnly = false)
		{
			if (publicOnly) return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
			else return type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		/// <summary>
		/// Get all the static nested types of a type.
		/// </summary>
		/// <param name="type">The type to search through.</param>
		/// <param name="publicOnly">If true this will only return public nested types, otherwise it returns all nested types.</param>
		/// <returns>Returns a list of static types that are nested within the provided type.</returns>
		public static System.Type[] GetStaticNestedTypes(this System.Type type, bool publicOnly = false)
		{
			if (publicOnly) return type.GetNestedTypes(BindingFlags.Static | BindingFlags.Public);
			else return type.GetNestedTypes(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		/// <summary>
		/// Check if a member has an attribute of the provided type.
		/// </summary>
		/// <typeparam name="T">The type of attribute to check for.</typeparam>
		/// <param name="member">The member to search through.</param>
		/// <param name="attribute">The outputted attribute if found.</param>
		/// <returns>Returns true if found the attribute of provided type, otherwise false.</returns>
		public static bool TryGetAttribute<T>(this MemberInfo member, out T attribute) where T : System.Attribute
		{
			attribute = member.GetCustomAttribute<T>();

			return attribute != null;
		}
	}
}
