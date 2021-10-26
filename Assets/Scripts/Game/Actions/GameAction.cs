using System.Collections;
using System.Collections.Generic;
using Celeritas.Scriptables;
using Celeritas.Game.Controllers;
using UnityEngine;

namespace Celeritas.Game.Actions
{
	/// <summary>
	/// The base class for all game actions.
	/// </summary>
	public abstract class GameAction
	{
		/// <summary>
		/// The data attatched to this action.
		/// </summary>
		public ActionData Data { get; private set; }

		/// <summary>
		/// Is this action  initalized.
		/// </summary>
		public bool IsInitalized { get; protected set; } = false;

		/// <summary>
		/// The owner of this entity.
		/// </summary>
		public Entity Owner { get; set; }

		/// <summary>
		/// The time this action was last used.
		/// </summary>
		public float LastUsed { get; private set; }

		/// <summary>
		/// The time this action was last used.
		/// </summary>
		public float CooldownReduction { get; set; }

		/// <summary>
		/// The time this action was last used.
		/// </summary>
		public int RareMetalCost { get; set; }

		/// <summary>
		/// Charge Percentage.
		/// </summary>
		public float PercentageReady { get => Mathf.Clamp01((Time.time - LastUsed) / (Data.CooldownSeconds - CooldownReduction)); }

		public float TimeLeftOnCooldown { get => Mathf.Max(0, LastUsed + (Data.CooldownSeconds - CooldownReduction) - Time.time); }

		public abstract SystemTargets Targets { get; }

		public int Level { get; private set; }

		/// <summary>
		/// Execute this particular action.
		/// </summary>
		/// <param name="entity">The entity to use for this action.</param>
		public void ExecuteAction(Entity entity)
		{
			if (LootController.Instance.SpendRareMetals(RareMetalCost) == false)
			{
				return;
			}

			if (LastUsed + (Data.CooldownSeconds - CooldownReduction) > Time.time)
				return;

			LastUsed = Time.time;
			Execute(entity, Level);
		}

		public virtual void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			Data = data;
			Owner = owner;
			IsInitalized = true;
		}

		public void SetLevel(int level)
		{
			Level = level;
		}

		protected abstract void Execute(Entity entity, int level);
	}
}
