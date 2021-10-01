using Celeritas.Game;
using Celeritas.Game.Entities;
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
			public float startTime_s; // time the effect was added
			public float damageRemainder; // used to preserve values lost after rounding float -> integer
			public float damagePerSecond;
			public float netDuration;
		}

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship | SystemTargets.Asteroid;

		[SerializeField, Title("DoT Characteristics", "Time measured in seconds")]
		private float duration;

		[SerializeField]
		private float damagePerSecond;

		[SerializeField]
		private float durationReductionPerLevel;

		[SerializeField]
		private float damageExtraPerLevel;

		public override string GetTooltip(int level) => $"Projectiles do <color=green>{damagePerSecond + (damageExtraPerLevel * level)}</color> extra damage over <color=green>{duration - (durationReductionPerLevel * level)}</color> seconds.\n";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			if (!entity.Components.TryGetComponent(this, out DamageOverTimeData data))
			{
				data = new DamageOverTimeData();
				entity.Components.RegisterComponent(this, data);
			}

			var netDamage = damagePerSecond + (damageExtraPerLevel * wrapper.Level);
			var netDuration = duration - (durationReductionPerLevel * wrapper.Level);

			data.startTime_s = entity.TimeAlive;
			data.damageRemainder = 0;
			data.netDuration = netDuration;
			data.damagePerSecond = netDamage / netDuration;
		}

		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			var data = entity.Components.GetComponent<DamageOverTimeData>(this);

			if (entity.TimeAlive - data.startTime_s > data.netDuration)
			{
				entity.EntityEffects.RemoveEffect(wrapper);
				return;
			}

			float damageToDo = (Time.deltaTime * data.damagePerSecond) + data.damageRemainder;
			
			if (damageToDo > 0)
			{ 
				entity.TakeDamage(entity, damageToDo);
			}
			data.damageRemainder = damageToDo - (int)damageToDo;
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			entity.Components.ReleaseComponent<DamageOverTimeData>(this);
		}

		public void OnReset(Entity entity, EffectWrapper wrapper)
		{
			var data = entity.Components.GetComponent<DamageOverTimeData>(this);

			data.startTime_s = entity.TimeAlive;
		}
	}
}
