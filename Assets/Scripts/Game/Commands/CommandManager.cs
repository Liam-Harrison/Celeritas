using Celeritas.Game.Console;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Celeritas.Commands
{
	/// <summary>
	/// A manager that prepares and provides access to Command methods within the assembly.
	/// </summary>
	public static class CommandManager
	{
		static CommandManager()
		{
			CacheMethods();
		}

		public static int CommandCount { get => CommandMethodPairs.Count; }

		public static Dictionary<ConsoleCommand, MethodInfo> CommandMethodPairs { get; private set; } = new Dictionary<ConsoleCommand, MethodInfo>();

		/// <summary>
		/// Precache all the found Commands within our assembly.
		/// </summary>
		private static void CacheMethods()
		{
			foreach (var type in typeof(CommandManager).Assembly.GetTypes())
			{
				foreach (var method in type.GetStaticMethods())
				{
					if (method.TryGetAttribute(out ConsoleCommand command))
					{
						if (!CommandMethodPairs.ContainsKey(command))
						{
							command.Initalize(method);
							CommandMethodPairs.Add(command, method);
						}
					}
				}
			}
		}

		/// <summary>
		/// Find a command that begins with the provided input, and if 
		/// only one command matches it will be returned.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="found">The found command.</param>
		/// <returns>Returns true if a command was found and no others match, otherwise false.</returns>
		public static bool TryGetAutocompletedCommand(string input, out ConsoleCommand found)
		{
			found = null;

			foreach (var command in CommandMethodPairs.Keys)
			{
				if (command.AutocompleteMatches(input) && found == null) found = command;
				else if (command.AutocompleteMatches(input) && found != null)
				{
					found = null;
					return false;
				}
			}

			return found != null;
		}

		/// <summary>
		/// Attempt to process the users input, checking for a matching command and executing it.
		/// </summary>
		/// <param name="input">The input string.</param>
		public static void ProcessInput(string input)
		{
			if (string.IsNullOrEmpty(input)) return;
			input = input.TrimEnd(' ');
			bool failed;

			DebugConsole.PushMessage($"\n> {input}", LogType.Log);

			foreach (var command in FindMatchingCommands(input))
			{
				var args = new List<string>(input.Split(' ', ','));
				args.RemoveAt(0);

				var method = CommandMethodPairs[command];
				failed = false;

				if (method.GetParameters().Length == 0 && args.Count == 0)
				{
					method.Invoke(null, null);
					return;
				}
				else if (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(string[]))
				{
					method.Invoke(null, args.ToArray());
					return;
				}
				else if (method.GetParameters().Length == args.Count)
				{
					object[] parameters = new object[method.GetParameters().Length];

					for (int i = 0; i < args.Count; i++)
					{
						if (method.GetParameters()[i].ParameterType == typeof(string))
						{
							parameters[i] = args[i];
						}
						else
						{
							object converted;

							try
							{
								converted = Convert.ChangeType(args[i], method.GetParameters()[i].ParameterType);
							}
							catch
							{
								failed = true;
								break;
							}

							parameters[i] = converted;
						}
					}

					if (failed) continue;

					method.Invoke(null, parameters);
					return;
				}
				else continue;
			}

			DebugConsole.PushMessage($"Could not find a command that matched your input", LogType.Error, 1);
		}

		private static void ServerCommandFailure()
		{
			DebugConsole.PushMessage($"You must be the server to execute this command", LogType.Error, 1);
		}

		/// <summary>
		/// Find a command matches the provided input.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="command">The found command.</param>
		/// <returns>Returns true if the command was found, otherwise false.</returns>
		private static ConsoleCommand[] FindMatchingCommands(string input)
		{
			List<ConsoleCommand> commands = new List<ConsoleCommand>();

			foreach (var c in CommandMethodPairs.Keys)
			{
				if (c.DoesCommandMatch(input))
				{
					commands.Add(c);
				}
			}

			return commands.ToArray();
		}
	}
}
