using System;
using System.Collections.Generic;
using System.Reflection;

namespace Celeritas.Commands
{
	/// <summary>
	/// Mark a static method as executable from the console system.
	/// Method must be static void.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ConsoleCommand : Attribute
	{
		public string Description { get; private set; } = "";

		public string[] ArgumentDescriptions { get; private set; } = default;

		public MethodInfo Method { get; private set; } = null;

		public bool HideInHelp { get; private set; } = false;

		public ConsoleCommand(params string[] argumentDescriptions)
		{
			ArgumentDescriptions = argumentDescriptions;
		}

		public ConsoleCommand(bool hideInHelp, params string[] argumentDescriptions) 
		{ 
			HideInHelp = hideInHelp;
			ArgumentDescriptions = argumentDescriptions;
		}

		public ConsoleCommand(string description, bool hideInHelp, params string[] argumentDescriptions)
		{
			Description = description;
			HideInHelp = hideInHelp;
			ArgumentDescriptions = argumentDescriptions;
		}

		public ConsoleCommand(string description, params string[] argumentDescriptions)
		{
			Description = description;
			ArgumentDescriptions = argumentDescriptions;
		}

		public bool DoesCommandMatch(string input)
		{
			return input.Split(' ')[0].ToLower() == Method.Name.ToLower();
		}

		public bool AutocompleteMatches(string input)
		{
			return Method.Name.ToLower().StartsWith(input.Split(' ')[0].ToLower());
		}

		public void Initalize(MethodInfo methodInfo)
		{
			Method = methodInfo;
		}

        public override bool Equals(object obj)
        {
            return obj is ConsoleCommand command &&
                   base.Equals(obj) &&
                   EqualityComparer<object>.Default.Equals(TypeId, command.TypeId) &&
                   Description == command.Description &&
                   EqualityComparer<string[]>.Default.Equals(ArgumentDescriptions, command.ArgumentDescriptions) &&
                   EqualityComparer<MethodInfo>.Default.Equals(Method, command.Method) &&
                   HideInHelp == command.HideInHelp;
        }

        public override int GetHashCode()
        {
            int hashCode = -1403016487;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(TypeId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(ArgumentDescriptions);
            hashCode = hashCode * -1521134295 + EqualityComparer<MethodInfo>.Default.GetHashCode(Method);
            hashCode = hashCode * -1521134295 + HideInHelp.GetHashCode();
            return hashCode;
        }
    }
}
