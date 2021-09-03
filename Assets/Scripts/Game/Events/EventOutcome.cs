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

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), InfoBox("You can use the standard string format to place variables in the dialogue content, by using the symbols {0}, {1}, ect. The symbols are in this order: Module Reward, Loot Drop Amount")]
		private LootType lootType;

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), MinMaxSlider(0, 4)]
		private Vector2Int lootRewardRange;

		[SerializeField, TitleGroup("Reward"), ShowIf(nameof(hasRewards))]
		private ModuleRewardChance[] moduleRewards;

		[SerializeField, TitleGroup("Event")]
		private bool hasEvent;

		[SerializeField, TitleGroup("Event"), ShowIf(nameof(hasEvent))]
		private EventData eventData;

		[SerializeField, TitleGroup("Waves")]
		private bool hasWaves;

		[SerializeField, TitleGroup("Waves"), ShowIf(nameof(hasWaves))]
		private EventOutcome waveOutcome;

		[SerializeField, TitleGroup("Waves"), ShowIf(nameof(hasWaves))]
		private WaveData[] waves;

		public bool HasRewards { get => hasRewards; }

		public Vector2Int ModuleRewardRange { get => lootRewardRange; }

		public ModuleRewardChance[] ModuleRewards { get => moduleRewards; }

		public bool HasWaves { get => hasWaves; }

		public WaveData[] Waves { get => waves; }

		public bool HasDialogue { get => hasDialogue; }

		public DialogueInfo Dialogue { get => dialogue; }

		public EventOutcome WaveOutcome { get => waveOutcome; }

		private readonly List<float> lastChances = new List<float>();

		private int? lootRewardOutcome = null;
		private ModuleData moduleDataRewardOutcome = null;
		private GameEvent waveParentEvent;

		public void DoEventOutcome(GameEvent gameEvent)
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
				dialogue.DynamicContent = GetFullDialogueContent(dialogue.content, lootRewardOutcome, moduleDataRewardOutcome);
				DialogueManager.Instance.ShowDialogue(dialogue, (i) => DialogueFinished(i, gameEvent));
			}
			else
			{
				CompleteOutcome(false, gameEvent);
			}
		}

		private void DialogueFinished(int i, GameEvent gameEvent)
		{
			if (i != -1)
			{
				var option = dialogue.options[i];
				if (option.outcome != null)
				{
					CompleteOutcome(true, gameEvent);
					option.outcome.DoEventOutcome(gameEvent);
				}
				else
				{
					CompleteOutcome(false, gameEvent);
				}
			}
			else
			{
				CompleteOutcome(false, gameEvent);
			}
		}

		private void CompleteOutcome(bool hasOutcome, GameEvent gameEvent)
		{
			if (moduleDataRewardOutcome != null)
				PlayerController.Instance.PlayerShipEntity.Inventory.Add(moduleDataRewardOutcome);

			if (lootRewardOutcome != null)
				LootController.Instance.GivePlayerLoot(lootType, lootRewardOutcome.Value);

			if (hasEvent && eventData != null)
				EventManager.Instance.CreateEvent(eventData);

			if (hasWaves)
			{
				waveParentEvent = gameEvent;
				WaveManager.OnWaveEnded += WaveFinished;
				foreach (var wave in waves)
				{
					WaveManager.Instance.StartWave(wave);
				}
			}
			else if (!hasOutcome)
			{
				gameEvent.EndEvent();
			}
		}

		private void WaveFinished()
		{
			WaveManager.OnWaveEnded -= WaveFinished;

			if (waveOutcome != null)
				waveOutcome.DoEventOutcome(waveParentEvent);
			else
			{
				waveParentEvent.EndEvent();
			}
		}

		private string GetFullDialogueContent(string content, int? modules, ModuleData reward)
		{
			var paramaters = new List<string>();

			AddValue(ref paramaters, reward);
			AddValue(ref paramaters, modules);

			return string.Format(content, paramaters.ToArray());
		}

		private void AddValue(ref List<string> paramaters, object value)
		{
			if (value != null)
			{
				paramaters.Add(value.ToString());
			}
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