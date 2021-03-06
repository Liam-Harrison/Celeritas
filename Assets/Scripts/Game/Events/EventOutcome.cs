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

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward")]
		private LootType lootType;

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), MinMaxSlider(0, 4)]
		private Vector2Int lootRewardRange;

		[SerializeField, TitleGroup("Reward"), ShowIf(nameof(hasRewards))]
		private ModuleRewardChance[] moduleRewards;

		[SerializeField, TitleGroup("Healing & Harming")]
		private bool changeHealth;

		[SerializeField, TitleGroup("Healing & Harming"), ShowIf(nameof(changeHealth)), PropertyRange(-1, 1)]
		private float percentChangeHealth;

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

		[SerializeField, TitleGroup("Doom")]
		private bool changeDoom;

		[SerializeField, TitleGroup("Doom"), ShowIf(nameof(changeDoom)), PropertyRange(-5, 5)]
		private int doom;


		public bool HasRewards { get => hasRewards; }

		public Vector2Int ModuleRewardRange { get => lootRewardRange; }

		public ModuleRewardChance[] ModuleRewards { get => moduleRewards; }

		public bool HasWaves { get => hasWaves; }

		public WaveData[] Waves { get => waves; }

		public bool HasDialogue { get => hasDialogue; }

		public DialogueInfo Dialogue { get => dialogue; }

		public EventOutcome WaveOutcome { get => waveOutcome; }

		private readonly List<float> lastChances = new List<float>();

		private GameEvent waveParentEvent;

		public void DoEventOutcome(GameEvent gameEvent)
		{
			int? lootRewardOutcome = null;
			ModuleData moduleDataRewardOutcome = null;
			float? healthChangeOutcome = null;

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

			if (changeHealth)
			{
				healthChangeOutcome = percentChangeHealth;
			}

			if (hasDialogue)
			{
				dialogue.DynamicContent = GetFullDialogueContent(dialogue.content, lootRewardOutcome, moduleDataRewardOutcome, healthChangeOutcome);
				if (DialogueManager.Instance != null)
				{
					DialogueManager.Instance.ShowDialogue(dialogue, (i) => DialogueFinished(i, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChangeOutcome));
				}
				else
				{
					System.Action<DialogueManager> handler = null;
					handler = (_) =>
					{
						DialogueManager.Instance.ShowDialogue(dialogue, (i) => DialogueFinished(i, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChangeOutcome));
						DialogueManager.OnCreated -= handler;
					};
					DialogueManager.OnCreated += handler;
				}

			}
			else
			{
				CompleteOutcome(false, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChangeOutcome);
			}
		}

		private void DialogueFinished(int i, GameEvent gameEvent, int? lootRewardOutcome = null, ModuleData moduleDataRewardOutcome = null, float? healthChange = null)
		{
			if (i != -1)
			{
				var option = dialogue.options[i];
				if (option.outcome != null)
				{
					CompleteOutcome(true, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChange);
					option.outcome.DoEventOutcome(gameEvent);
				}
				else
				{
					CompleteOutcome(false, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChange);
				}
			}
			else
			{
				CompleteOutcome(false, gameEvent, lootRewardOutcome, moduleDataRewardOutcome, healthChange);
			}
		}

		private void CompleteOutcome(bool hasOutcome, GameEvent gameEvent, int? lootRewardOutcome = null, ModuleData moduleDataRewardOutcome = null, float? healthChange = null)
		{
			if (moduleDataRewardOutcome != null)
				PlayerController.Instance.PlayerShipEntity.Inventory.Add(moduleDataRewardOutcome);

			if (lootRewardOutcome != null)
				LootController.Instance.GivePlayerLoot(lootType, lootRewardOutcome.Value);

			if (healthChange != null)
				PlayerController.Instance.PlayerShipEntity.Health.Damage(PlayerController.Instance.PlayerShipEntity.Health.MaxValue * -healthChange.Value);

			if (hasEvent && eventData != null)
				EventManager.Instance.CreateEvent(eventData);

			if (changeDoom)
			{
				DoomManager.Instance.ChangeDoomMeter(doom);
			}

			if (hasWaves)
			{
				waveParentEvent = gameEvent;
				WaveManager.OnWaveEnded += WaveFinished;
				
				int numberOfEnemies = 0;
				foreach (var wave in waves)
				{
					WaveManager.Instance.StartWave(wave);
					numberOfEnemies += wave.ShipPool.Length;
				}
				CombatHUD.Instance.PrintNotification($"Defeat all enemies! {numberOfEnemies} remaining.");
				EnemyManager.OnEnemyShipDestroyed += EnemyShipDestroyed;
			}
			else if (!hasOutcome)
			{
				gameEvent.EndEvent();
			}
		}

		private void WaveFinished()
		{
			WaveManager.OnWaveEnded -= WaveFinished;
			EnemyManager.OnEnemyShipDestroyed -= EnemyShipDestroyed;
			CombatHUD.Instance.PrintNotification("All enemies defeated!");

			if (waveOutcome != null)
				waveOutcome.DoEventOutcome(waveParentEvent);
			else
			{
				waveParentEvent.EndEvent();
			}
		}

		private void EnemyShipDestroyed(int count)
		{
			if (count == 1)
				CombatHUD.Instance.PrintNotification($"{count} enemy remaining.");
			else if (count != 0)
				CombatHUD.Instance.PrintNotification($"{count} enemies remaining.");
		}

		private string GetFullDialogueContent(string content, int? modules, ModuleData reward, float? health)
		{
			var paramaters = new List<string>();

			AddValue(ref paramaters, reward);
			AddValue(ref paramaters, modules);
			AddValue(ref paramaters, health, "0");

			return string.Format(content, paramaters.ToArray());
		}

		private void AddValue(ref List<string> paramaters, object value)
		{
			if (value != null)
			{
				paramaters.Add(value.ToString());
			}
		}

		private void AddValue(ref List<string> paramaters, object value, string format)
		{
			if (value != null)
			{
				paramaters.Add(string.Format("{0:" + format + "}", value.ToString()));
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