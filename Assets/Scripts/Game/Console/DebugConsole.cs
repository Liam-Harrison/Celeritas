using Celeritas.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.Game.Console
{
	public class DebugConsole : Singleton<DebugConsole>, Actions.IConsoleActions
	{
		/// <summary>
		/// A container for meta data about every entry in the console.
		/// </summary>
		public struct Entry
		{
			public void Initialize(string message, LogType type, string trace = "")
			{
				this.message = message;
				this.trace = trace;
				this.type = type;
				time = DateTime.Now;
				occured = 1;
			}

			public string message;
			public string trace;
			public DateTime time;
			public LogType type;
			public int occured;
		}

		[Header("General Assignments")]
		[SerializeField] private GameObject background;

		[Header("Console Assignments")]
		[SerializeField] private HorizontalLayoutGroup textLayoutGroup;
		[SerializeField] private TMP_InputField consoleText;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Scrollbar scrollbar;

		[Header("Console Settings")]
		[SerializeField] private int textBufferLength = 256;
		[SerializeField] private int entryBufferLength = 6;
		[SerializeField] private float entryChangeTime = 0.25f;
		[SerializeField] private bool printWarnings = true;

		/// <summary>
		/// Check if the console is currently open.
		/// </summary>
		public bool IsOpened { get; private set; } = false;

		/// <summary>
		/// Console will print warnings if this value is true.
		/// </summary>
		public bool PrintWarnings { get => printWarnings; set => printWarnings = value; }

		private LoopingArray<Entry> Buffer { get; set; }
		private List<string> entryBuffer = new List<string>();
		private Actions.ConsoleActions actions = default;

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(this);

			actions = new Actions.ConsoleActions(new Actions());
			actions.SetCallbacks(this);
			actions.Enable();

			Buffer = new LoopingArray<Entry>(textBufferLength);

			Debug.developerConsoleVisible = false;
			Application.logMessageReceived += PushMessage;

			PushMessage("Console Started");
			PushMessage($"Loaded {CommandManager.CommandCount} command{(CommandManager.CommandCount == 1 ? "" : "s")}", 1);
		}

		protected override void OnDestroy()
		{
			Application.logMessageReceived -= PushMessage;
			actions.Disable();
		}

		private bool contentDirty = false;
		private bool layoutDirty = false;

		private void Update()
		{
			if (IsOpened)
			{
				if (contentDirty)
				{
					UpdateText();
				}

				if (layoutDirty)
				{
					UpdateUILayout();
				}
			}
		}

		/// <summary>
		/// Attempt to see if any commands match the input so far, and if possible complete the command.
		/// </summary>
		private void AutoCompleteCommand()
		{
			if (CommandManager.TryGetAutocompletedCommand(inputField.text, out var command))
			{
				inputField.text = command.Method.Name.ToLower() + " ";
				inputField.caretPosition = inputField.text.Length;
			}
		}

		private int entryBufferIndex = -1;
		private float lastEntryChange = 0;

		/// <summary>
		/// Process the users input.
		/// </summary>
		private void HandleConsoleInput()
		{
			var input = inputField.text;

			entryBuffer.Insert(0, input);
			while (entryBuffer.Count > entryBufferLength) entryBuffer.RemoveAt(entryBuffer.Count - 1);
			entryBufferIndex = -1;

			inputField.text = "";
			FocusConsoleInputField();

			CommandManager.ProcessInput(input);
		}

		/// <summary>
		/// Open or close the console, and preform any logic nessecary.
		/// </summary>
		private void ToggleConsole()
		{
			IsOpened = !IsOpened;

			if (IsOpened)
			{
				background.SetActive(true);
				FocusConsoleInputField();
			}
			else
			{
				inputField.OnDeselect(new BaseEventData(EventSystem.current));
				background.SetActive(false);
			}
		}

		/// <summary>
		/// Decrease the entry buffers value by 1 and update the input field accordingly.
		/// </summary>
		private void DecrementEntryBuffer()
		{
			lastEntryChange = Time.time + 0.25f;

			if (entryBufferIndex >= -1 && entryBuffer.Count > 0)
			{
				if (entryBufferIndex > -1) entryBufferIndex--;

				if (entryBufferIndex == -1)
				{
					inputField.text = "";
					FocusConsoleInputField();
				}
				else
				{
					inputField.text = entryBuffer[entryBufferIndex];
					inputField.caretPosition = inputField.text.Length;
				}
			}
		}

		/// <summary>
		/// Increase the entry buffers value by 1 and update the input field accordinly.
		/// </summary>
		private void IncrementEntryBuffer()
		{
			lastEntryChange = Time.time + 0.25f;

			if (entryBufferIndex + 1 < entryBuffer.Count && entryBuffer.Count > 0)
			{
				entryBufferIndex++;
				inputField.text = entryBuffer[entryBufferIndex];
				inputField.caretPosition = inputField.text.Length;
			}
		}

		/// <summary>
		/// Focus the console input field.
		/// </summary>
		public void FocusConsoleInputField()
		{
			inputField.Select();
			inputField.ActivateInputField();
		}

		/// <summary>
		/// Add a new message to the console. Preforming any logic nessecary to maintain the Consoles state and content.
		/// </summary>
		/// <param name="message">The message that was logged.</param>
		/// <param name="stackTrace">The stacktrace of the log method that was raised.</param>
		/// <param name="type">The type of the message.</param>
		private void PushMessage(string message, string stackTrace, LogType type)
		{
			contentDirty = true;

			// Check if the message we're adding was the last message added to the Queue, 
			// if so increment the times it occured instead of adding a new entry.
			if (Buffer.Length > 0)
			{
				if (Buffer.GetLastRef().message == message && Buffer.GetLastRef().type == type && Buffer.GetLastRef().message != "")
				{
					Buffer.GetLastRef().occured++;

					return;
				}
			}

			Buffer.GetNextAvailable().Initialize(message, type, stackTrace);
		}

		/// <summary>
		/// Add a new message to the console. Preforming any logic nessecary to maintain the Consoles state and content.
		/// </summary>
		/// <param name="message">The message that was logged.</param>
		/// <param name="type">The type of the message.</param>
		/// <param name="indent">The level of indentation this message should be drawn with from 0 to 6.</param>
		public static void PushMessage(string message, LogType type = LogType.Log, int indent = 0)
		{
			if (indent > 0)
				Instance.PushMessage($"<indent={2 * Mathf.Clamp(indent, 0, 6)}%>{message}</indent>", "", type);
			else
				Instance.PushMessage(message, "", type);
		}

		/// <summary>
		/// Add a new message to the console. Preforming any logic nessecary to maintain the Consoles state and content.
		/// </summary>
		/// <param name="message">The message that was logged.</param>
		/// <param name="indent">The level of indentation this message should be drawn with from 0 to 6.</param>
		public static void PushMessage(string message, int indent = 0)
		{
			PushMessage(message, LogType.Log, indent);
		}

		/// <summary>
		/// Add a new message to the console. Preforming any logic nessecary to maintain the Consoles state and content.
		/// </summary>
		/// <param name="message">The message that was logged.</param>
		public static void PushMessage(string message)
		{
			Instance.PushMessage(message, "", LogType.Log);
		}

		/// <summary>
		/// Clear the current console log.
		/// </summary>
		public void ClearLog()
		{
			Buffer.Clear();

			UpdateText();
		}

		/// <summary>
		/// Open the log file with the default application.
		/// </summary>
		public void OpenLog()
		{
			if (string.IsNullOrEmpty(Application.consoleLogPath)) return;

			System.Diagnostics.Process.Start(Application.consoleLogPath);
		}

		/// <summary>
		/// Open the log folder with explorer or the Operating Systems equivalent.
		/// </summary>
		public void OpenLogFolder()
		{
			if (string.IsNullOrEmpty(Application.consoleLogPath)) return;

			System.Diagnostics.Process.Start(new FileInfo(Application.consoleLogPath).Directory.FullName);
		}

		private void UpdateText()
		{
			string text = "";

			for (int i = 0; i < Buffer.Length; i++)
			{
				text += GetFormattedLine(ref Buffer.GetRefValue(i));
			}

			consoleText.text = text;

			layoutDirty = true;
			contentDirty = false;
		}

		private void UpdateUILayout()
		{
			foreach (var rect in GetComponentsInChildren<RectTransform>())
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
			}
			Canvas.ForceUpdateCanvases();
			textLayoutGroup.enabled = false;
			textLayoutGroup.enabled = true;
			layoutDirty = false;
		}

		private string GetFormattedLine(ref Entry line)
		{
			if (!ShouldPrintLine(line.type)) return "";

			string message = "";

			if (line.type == LogType.Exception)
			{
				message += $"[{line.time:T}] ";
			}

			message += line.message;

			if (line.occured > 1) message += $" [{line.occured}]";

			if (ShouldPrintStack(ref line)) message += $"\n<indent=2%>{line.trace.Substring(0, line.trace.Length - 2)}</indent>";

			return $"{ColorText(message, line.type)}\n";
		}

		private bool ShouldPrintLine(LogType type)
		{
			return type != LogType.Warning || printWarnings;
		}

		private bool ShouldPrintStack(ref Entry line)
		{
			if (string.IsNullOrEmpty(line.trace)) return false;

			return line.type == LogType.Exception || line.type == LogType.Assert || line.type == LogType.Error;
		}

		private static string ColorText(string message, LogType type)
		{
			switch (type)
			{
				case LogType.Error:
				case LogType.Exception:
					return $"<color=red>{message}</color>";
				case LogType.Warning:
					return $"<color=yellow>{message}</color>";
				case LogType.Assert:
					return $"<color=white>{message}</color>";
				default:
					return message;
			}
		}

		public void OnToggle(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				ToggleConsole();
			}
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
			if (IsOpened && context.performed)
			{
				HandleConsoleInput();
			}
		}

		public void OnUpBuffer(InputAction.CallbackContext context)
		{
			if (Time.time - lastEntryChange > entryChangeTime && IsOpened && context.performed)
			{
				IncrementEntryBuffer();
			}
		}

		public void OnDownBuffer(InputAction.CallbackContext context)
		{
			if (Time.time - lastEntryChange > entryChangeTime && IsOpened && context.performed)
			{
				DecrementEntryBuffer();
			}
		}

		public void OnFocus(InputAction.CallbackContext context)
		{
			if (IsOpened && !inputField.isFocused && context.performed)
			{
				FocusConsoleInputField();
			}
			else if (IsOpened && inputField.isFocused && context.performed)
			{
				AutoCompleteCommand();
			}
		}
	}
}
