using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Celeritas.Game.Controllers;

namespace Celeritas.Game.Events
{
	[System.Serializable]
	public struct ModuleRewardChance
	{
		[PropertyRange(0, 1)]
		public float chance;

		public ModuleData module;
	}

	[CreateAssetMenu(fileName = "New Event", menuName = "Celeritas/Events/Outcomes/New Generic Outcome")]
	public class EventOutcome : ScriptableObject
	{
		[SerializeField, TitleGroup("Dialogue")]
		public bool hasDialogue;

		[SerializeField, TitleGroup("Dialogue"), ShowIf(nameof(hasDialogue))]
		public DialogueInfo dialogue;

		[SerializeField, TitleGroup("Reward")]
		private bool hasRewards;

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), InfoBox("You can use the results of the rewards in your dialogue content string with the {0} and {1} symbols. The {0} will be the module, if possible, otherwise it will be the loot amount. If {0} is used then {1} will be the loot amount.")]
		private LootType lootType;

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), MinMaxSlider(0, 4)]
		private Vector2Int lootRewardRange;

		[SerializeField, TitleGroup("Reward"), ShowIf(nameof(hasRewards))]
		private ModuleRewardChance[] moduleRewards;

		[SerializeField, TitleGroup("Waves")]
		private bool hasWaves;

		[SerializeField, TitleGroup("Waves"), ShowIf(nameof(hasWaves))]
		private WaveData[] waves;

		public bool HasRewards { get => hasRewards; }

		public Vector2Int ModuleRewardRange { get => lootRewardRange; }

		public ModuleRewardChance[] ModuleRewards { get => moduleRewards; }

		public bool HasWaves { get => hasWaves; }

		public WaveData[] Waves { get => waves; }

		public bool HasDialogue { get => hasDialogue; }

		public DialogueInfo Dialogue { get => dialogue; }

		private readonly List<float> lastChances = new List<float>();

		private int? lootRewardOutcome = null;
		private ModuleData moduleDataRewardOutcome = null;
		private string originalDialogueContent;

		public void DoEventOutcome()
		{
			if (hasRewards)
			{
				lootRewardOutcome = Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp(lootRewardRange.x, lootRewardRange.y, Random.value)), lootRewardRange.x, lootRewardRange.y);
				moduleDataRewardOutcome = GetReward();
			}
			else
			{
				lootRewardOutcome = null;
				moduleDataRewardOutcome = null;
			}

			if (hasDialogue)
			{
				if (string.IsNullOrEmpty(originalDialogueContent))
					originalDialogueContent = dialogue.content;

				dialogue.content = GetFullDialogueContent(originalDialogueContent, lootRewardOutcome, moduleDataRewardOutcome);

				DialogueManager.Instance.ShowDialogue(dialogue, DialogueFinished);
			}
			else
			{
				CompleteOutcome();
			}
		}

		private void DialogueFinished(int i)
		{
			CompleteOutcome();

			var option = dialogue.options[i];
			if (option.outcome != null)
				option.outcome.DoEventOutcome();
		}

		private void CompleteOutcome()
		{
			if (moduleDataRewardOutcome != null)
				PlayerController.Instance.PlayerShipEntity.Inventory.Add(moduleDataRewardOutcome);

			if (lootRewardOutcome != null)
				LootController.Instance.GivePlayerLoot(lootType, lootRewardOutcome.Value);

			if (hasWaves)
			{
				foreach (var wave in waves)
				{
					WaveManager.Instance.StartWave(wave);
				}
			}
		}

		private string GetFullDialogueContent(string content, int? modules, ModuleData reward)
		{
			var a = "";
			var b = "";

			string result = "No Dialogue.";

			if (reward != null)
			{
				a = reward.Title;
				if (modules != null)
					b = modules.Value.ToString();
			}
			else if (modules != null)
			{
				a = modules.Value.ToString();
			}

			result = string.Format(content, a, b);

			return result;
		}

		public ModuleData GetReward()
		{
			float i = Random.value;
			foreach (var item in ModuleRewards.OrderBy((a) => a.chance))
			{
				if (item.chance >= i)
					return item.module;
			}

			if (ModuleRewards.Length > 0)
				return ModuleRewards[0].module;

			return null;
		}

		private void OnValidate()
		{
			if (moduleRewards.Length == 1)
			{
				moduleRewards[0].chance = 1;
				return;
			}

			if (moduleRewards.Length == lastChances.Count)
			{
				for (int i = 0; i < moduleRewards.Length; i++)
				{
					if (lastChances[i] != moduleRewards[i].chance)
					{
						RebalanceAll(i);
						break;
					}
				}
			}
			else if (moduleRewards.Length > 0)
			{
				RebalanceAll(-1);
			}

			for (int i = 0; i < moduleRewards.Length; i++)
			{
				if (i >= lastChances.Count)
					lastChances.Add(moduleRewards[i].chance);
				else
					lastChances[i] = moduleRewards[i].chance;
			}

			for (int i = lastChances.Count - 1; i > moduleRewards.Length; i--)
			{
				lastChances.RemoveAt(i);
			}
		}

		private void RebalanceAll(int expect)
		{
			float total = 0;
			int k = 0;

			while (!Mathf.Approximately(total, 1) && k < 100)
			{
				float delta = 0;
				foreach (var item in moduleRewards)
				{
					delta += item.chance;
				}
				delta = 1 - delta;

				var toApply = delta / moduleRewards.Length;

				if (expect != -1)
				{
					var item = moduleRewards[expect];
					var n = Mathf.Clamp01(item.chance + toApply);

					var d = n - item.chance;
					toApply += d / (moduleRewards.Length - 1);
				}

				total = 0;
				for (int i = 0; i < moduleRewards.Length; i++)
				{
					if (expect == i)
					{
						total += moduleRewards[expect].chance;
						continue;
					}

					var item = moduleRewards[i];

					var n = Mathf.Clamp01(item.chance + toApply);

					if (n == 1 || n == 0)
					{
						var d = n - item.chance;
						toApply += d / (moduleRewards.Length - (expect > -1 ? 1 : 0));
					}

					total += n;

					item.chance = n;
					moduleRewards[i] = item;
				}
				k++;
			}
		}
	}
}