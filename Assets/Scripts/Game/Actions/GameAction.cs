using Celeritas.Scriptables;
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
		/// Charge Percentage.
		/// </summary>
		public float PercentageReady { get => Mathf.Clamp01((Time.time - LastUsed) / Data.CooldownSeconds); }

		public float TimeLeftOnCooldown { get => Mathf.Max(0, LastUsed + Data.CooldownSeconds - Time.time); }

		public abstract SystemTargets Targets { get; }

		/// <summary>
		/// Execute this particular action.
		/// </summary>
		/// <param name="entity">The entity to use for this action.</param>
		public void ExecuteAction(Entity entity)
		{
			if (LastUsed + Data.CooldownSeconds > Time.time)
				return;

			LastUsed = Time.time;
			Execute(entity);
		}

		protected abstract void Execute(Entity entity);

		public virtual void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			Data = data;
			Owner = owner;
			IsInitalized = true;
		}
	}
}
