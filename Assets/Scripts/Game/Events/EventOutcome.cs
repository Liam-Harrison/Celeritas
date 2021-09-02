using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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

		[SerializeField, TitleGroup("Dialogue")]
		public DialogueInfo dialogue;

		[SerializeField, TitleGroup("Reward")]
		private bool hasRewards;

		[SerializeField, ShowIf(nameof(hasRewards)), TitleGroup("Reward"), MinMaxSlider(0, 4), InfoBox("Minimum and maximum amount of modules to possibly recieve.")]
		private Vector2Int moduleRewardRange;

		[SerializeField, TitleGroup("Reward"), ShowIf(nameof(hasRewards))]
		private ModuleRewardChance[] rewards;

		[SerializeField, TitleGroup("Waves")]
		private bool hasWaves;

		[SerializeField, TitleGroup("Waves"), ShowIf(nameof(hasWaves))]
		private WaveData[] waves;

		public bool HasRewards { get => hasRewards; }

		public Vector2Int ModuleRewardRange { get => moduleRewardRange; }

		public ModuleRewardChance[] Rewards { get => rewards; }

		public bool HasShips { get => hasWaves; }

		public WaveData[] Waves { get => waves; }

		public bool HasDialogue { get => hasDialogue; }

		public DialogueInfo Dialogue { get => dialogue; }

		private readonly List<float> lastChances = new List<float>();

		private void OnValidate()
		{
			if (rewards.Length == lastChances.Count)
			{
				for (int i = 0; i < rewards.Length; i++)
				{
					if (lastChances[i] != rewards[i].chance)
					{
						RebalanceAll(i);
						break;
					}
				}
			}
			else if (rewards.Length > 0)
			{
				RebalanceAll(-1);
			}


			for (int i = 0; i < rewards.Length; i++)
			{
				if (i >= lastChances.Count)
					lastChances.Add(rewards[i].chance);
				else
					lastChances[i] = rewards[i].chance;
			}

			for (int i = lastChances.Count - 1; i > rewards.Length; i--)
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
				foreach (var item in rewards)
				{
					delta += item.chance;
				}
				delta = 1 - delta;

				var toApply = delta / rewards.Length;

				if (expect != -1)
				{
					var item = rewards[expect];
					var n = Mathf.Clamp01(item.chance + toApply);

					var d = n - item.chance;
					toApply += d / (rewards.Length - 1);
				}

				total = 0;
				for (int i = 0; i < rewards.Length; i++)
				{
					if (expect == i)
					{
						total += rewards[expect].chance;
						continue;
					}

					var item = rewards[i];

					var n = Mathf.Clamp01(item.chance + toApply);

					if (n == 1 || n == 0)
					{
						var d = n - item.chance;
						toApply += d / (rewards.Length - (expect > -1 ? 1 : 0));
					}

					total += n;

					item.chance = n;
					rewards[i] = item;
				}
				k++;
			}
		}
	}
}