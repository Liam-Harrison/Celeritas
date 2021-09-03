using Celeritas.Game;
using Celeritas.Game.Events;
using Celeritas.UI.Dialogue;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeritas.UI
{
	[Serializable]
	public struct Option
	{
		public OptionType type;

		public string content;

		public EventOutcome outcome;
	}

	[Serializable]
	public class DialogueInfo
	{
		[PropertySpace]
		public string title;

		[TextArea(6,12)]
		public string content;

		[PropertySpace]
		public Option[] options;

		public string DynamicContent { get; set; }
	}

	public class DialogueManager : Singleton<DialogueManager>
	{
		[SerializeField, TitleGroup("Assignments")]
		private GameObject dialogueOptionPrefab;

		[SerializeField, TitleGroup("Assignments")]
		private Transform dialogueOptionParent;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI title;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI content;

		[SerializeField, TitleGroup("Assignments")]
		private GameObject background;

		public DialogueInfo Dialogue { get; private set; }

		private void OnDisable()
		{
			Time.timeScale = 1;
		}

		public void ShowDialogue(DialogueInfo info, Action<int> callback)
		{
			Dialogue = info;
			title.text = info.title;

			if (string.IsNullOrEmpty(info.DynamicContent))
				content.text = info.content;
			else
				content.text = info.DynamicContent;

			for (int i = dialogueOptionParent.childCount - 1; i >= 0; i--)
			{
				Destroy(dialogueOptionParent.GetChild(i).gameObject);
			}

			if (info.options.Length == 0)
			{
				CreateOption("Continue...", 0, OptionType.Neutral, (_) => callback?.Invoke(-1));
			}
			else
			{
				for (int i = 0; i < info.options.Length; i++)
				{
					var option = info.options[i];
					CreateOption(option.content, i, option.type, callback);
				}
			}

			StopAllCoroutines();
			ChangeTimeScale(0);
			background.SetActive(true);
			CombatHUD.Instance.SetGameCursor(false);
		}

		private void CreateOption(string text, int index, OptionType type, Action<int> callback)
		{
			var option = Instantiate(dialogueOptionPrefab, dialogueOptionParent).GetComponent<DialogueOption>();
			option.SetOptionType(type);
			option.Text = text;
			option.OnClicked += () =>
			{
				ChangeTimeScale(1);
				CombatHUD.Instance.SetGameCursor(true);
				background.SetActive(false);
				callback?.Invoke(index);
			};
		}

		private const float DURATION = 0.25f;

		private void ChangeTimeScale(float value)
		{
			StopAllCoroutines();
			StartCoroutine(ChangeTimeScaleCoroutine(value));
		}

		private IEnumerator ChangeTimeScaleCoroutine(float value)
		{
			float startTime = Time.unscaledTime;
			float startValue = Time.timeScale;
			float p;

			do
			{
				p = Mathf.Clamp01((Time.unscaledTime - startTime) / DURATION);
				Time.timeScale = Mathf.Lerp(startValue, value, p);
				yield return null;
			} while (p < 1);

			Time.timeScale = value;

			yield break;
		}
	}
}
