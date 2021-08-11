using Celeritas.Game.Console;
using System.Collections.Generic;
using System.Linq;

namespace Celeritas.Commands
{
	/// <summary>
	/// This class contains commands that relate to the console itself.
	/// </summary>
	public static class ConsoleCommands
    {
        [ConsoleCommand("Displays a list of commands.")]
        public static void Help()
        {
            var commands = SortCommandDictonaryByClass();

            int longest = commands.Values.Max((a) => a.Max((b) => b.Method.Name.Length));

            foreach (var commandClass in commands.Keys)
            {
                bool found = false;
				foreach (var command in commands[commandClass])
				{
                    if (found = !command.HideInHelp) break;
				}
                if (!found) continue;

                DebugConsole.PushMessage($"{commandClass}", 1);

				foreach (var command in commands[commandClass])
				{
					if (command.HideInHelp) continue;

					var name = command.Method.Name.PadRight(longest);

					if (string.IsNullOrEmpty(command.Description)) DebugConsole.PushMessage($"{name}", 2);
					else DebugConsole.PushMessage($"{name} - {command.Description}", 2);
				}

                DebugConsole.PushMessage($"");
            }
        }

        [ConsoleCommand(hideInHelp: true)]
        public static void Help(string method)
        {
			bool found = false;
			foreach (var command in CommandManager.CommandMethodPairs.Keys)
			{
				if (command.HideInHelp && command.Description == "") continue;
                if (command.Method.Name.ToLower() == method.ToLower())
				{
                    var param = command.Method.GetParameters();
                    string title = command.Method.Name;

                    for (int i = 0; i < param.Length; i++)
                    {
                        title += $" [{param[i].ParameterType.Name}]";
                    }

                    DebugConsole.PushMessage($"");
					if (found)
					{
						DebugConsole.PushMessage($"----------------------");
						DebugConsole.PushMessage($"");
					}
					DebugConsole.PushMessage(title);

                    DebugConsole.PushMessage(command.Description);
					if (!string.IsNullOrEmpty(command.Description) && param.Length > 0) DebugConsole.PushMessage("");

                    if (param.Length > 0)
                    {
                        DebugConsole.PushMessage($"Arguments:");
                        int longest = param.Max((a) => $"{a.Name} [{a.ParameterType.Name}]".Length);

                        for (int i = 0; i < param.Length; i++)
					    {
                            if (i < command.ArgumentDescriptions.Length) 
                                DebugConsole.PushMessage($"{param[i].Name} [{param[i].ParameterType.Name}]".PadRight(longest + 1) + $"- {command.ArgumentDescriptions[i]}", 1);
                            else
                                DebugConsole.PushMessage($"{param[i].Name} [{param[i].ParameterType.Name}]".PadRight(longest + 1), 1);
                        }
					}

					found = true;
				}
			}
			if (!found) DebugConsole.PushMessage($"Could not find any commands that match your input, type <color=yellow>\"help\"</color> to get a list of commands.", UnityEngine.LogType.Error, 1);
        }

        [ConsoleCommand("Clear the console.")]
        public static void Clear()
        {
            DebugConsole.Instance.ClearLog();
        }

		[ConsoleCommand("Enable or disable warnings to be printed to the console.")]
		public static void LogWarnings(bool state)
		{
			DebugConsole.Instance.PrintWarnings = state;
		}

        private static Dictionary<string, List<ConsoleCommand>> SortCommandDictonaryByClass()
        {
            Dictionary<string, List<ConsoleCommand>> commands = new Dictionary<string, List<ConsoleCommand>>();

            foreach (var command in CommandManager.CommandMethodPairs.Keys)
            {
                string classname = command.Method.DeclaringType.Namespace.AfterCharacter('.');

                if (!commands.ContainsKey(classname)) commands.Add(classname, new List<ConsoleCommand>());

                commands[classname].Add(command);
            }

            return commands;
        }

    }
}
