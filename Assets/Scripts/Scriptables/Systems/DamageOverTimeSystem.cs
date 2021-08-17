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
	class DamageOverTimeSystem : ModifierSystem, IEntityEffectAdded, IEntityUpdated, IEntityEffectRemoved
	{

		public class DamageOverTimeData
		{
			public float startTime_s; // time the effect was added
			public float damageRemainder; // used to preserve values lost after rounding float -> integer
		}

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship | SystemTargets.Asteroid;

		[SerializeField, Title("DoT Characteristics", "Time measured in seconds")]
		private float duration;

		[SerializeField]
		private float damage;

		[SerializeField]
		private float durationReductionPerLevel;

		[SerializeField]
		private float damageExtraPerLevel;

		private float netDamage; // factoring in level
		private float netDuration; // note: this must be > 0. If it is too close to 0, system will not have enough time to do any damage.

		private float damagePerSecond;

		public override string GetTooltip(ushort level) => $"<color=green>▲</color> Projectiles do 150% damage over {duration} seconds.\n<color=red>▼</color> Projectiles do 50% less initial damage.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			if (!entity.Components.TryGetComponent(this, out DamageOverTimeData data))
			{ // if no data existed beforehand, create some
				data = new DamageOverTimeData();
				entity.Components.RegisterComponent(this, data);
			}
			data.startTime_s = entity.TimeAlive;
			data.damageRemainder = 0;

			// factor in level
			netDamage = damage + (damageExtraPerLevel * level);
			netDuration = duration - (durationReductionPerLevel * level); 

			damagePerSecond = netDamage / netDuration;
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			var data = entity.Components.GetComponent<DamageOverTimeData>(this);

			if (entity.TimeAlive - data.startTime_s > netDuration)
			{ // if DoT has expired, return
				return; // how to remove system ?
			}

			float damageToDo = (Time.deltaTime * damagePerSecond) + data.damageRemainder;
			
			if ((int)damageToDo > 0) { 
				entity.TakeDamage(entity, (int)damageToDo);
				//Debug.Log((int)damageToDo);
			}
			data.damageRemainder = damageToDo - (int)damageToDo; // if damagetodo was < 1, store it as remainder
			
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			entity.Components.ReleaseComponent<DamageOverTimeData>(this);
		}
	}
}
