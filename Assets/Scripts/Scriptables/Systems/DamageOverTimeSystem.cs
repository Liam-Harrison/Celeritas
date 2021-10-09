using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// The entity with this system will take a set amount of damage over a set amount of time (DoT)
	/// </summary>
	[CreateAssetMenu(fileName = "New DoT System", menuName = "Celeritas/Modifiers/Take DoT")]
	class DamageOverTimeSystem : ModifierSystem, IEntityEffectAdded, IEntityUpdated, IEntityEffectRemoved, IEntityResetable
	{
		public class DamageOverTimeData
		{
			public float timeApplied;
		}

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship | SystemTargets.Asteroid;

		[SerializeField, Title("DoT Characteristics", "Time measured in seconds")]
		private float duration;

		[SerializeField]
		private float damagePerSecond;

		[SerializeField]
		private float durationIncreasePerLevel;

		[SerializeField]
		private float damageExtraPerLevel;

		public override string GetTooltip(int level) => $"Deals <color=green>{GetDPS(level)}</color> damage per second over <color=green>{GetDuration(level)}</color> seconds.\n";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			if (!entity.Components.TryGetComponent(this, out DamageOverTimeData data))
			{
				data = new DamageOverTimeData();
				entity.Components.RegisterComponent(this, data);
			}

			data.timeApplied = entity.TimeAlive;
		}

		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			var data = entity.Components.GetComponent<DamageOverTimeData>(this);

			if (entity.TimeAlive - data.timeApplied > GetDuration(wrapper.Level))
			{
				entity.EntityEffects.RemoveEffect(wrapper);
				return;
			}

			float damage = Time.smoothDeltaTime * GetDPS(wrapper.Level);
			entity.TakeDamage(entity, damage);
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			entity.Components.ReleaseComponent<DamageOverTimeData>(this);
		}

		public void OnReset(Entity entity, EffectWrapper wrapper)
		{
			var data = entity.Components.GetComponent<DamageOverTimeData>(this);
			data.timeApplied = entity.TimeAlive;
		}

		private float GetDPS(int level)
		{
			return damagePerSecond + (damageExtraPerLevel * level);
		}

		private float GetDuration(int level)
		{
			return duration + (durationIncreasePerLevel * level);
		}
	}
}
