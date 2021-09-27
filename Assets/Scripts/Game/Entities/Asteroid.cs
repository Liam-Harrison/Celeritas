using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// Asteroid objects that float around space, will give player rare metal when destroyed
	/// todo: rubble effect when asteroids are destroyed
	/// </summary>
	public class Asteroid : Entity, ITractorBeamTarget
	{
		[SerializeField]
		private uint startingHealth;

		public uint StartingHealth { get => startingHealth; set => startingHealth = value; }

		[SerializeField, Title("Loot")]
		private LootConfig lootConfig;

		public override SystemTargets TargetType { get => SystemTargets.Asteroid; }

		private EntityStatBar health;

		public AsteroidData AsteroidData { get; private set; }

		public Rigidbody2D Rigidbody { get; private set; }

		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			AsteroidData = data as AsteroidData;
			startingHealth = AsteroidData.Health;
			Rigidbody = GetComponent<Rigidbody2D>();

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		void Start()
		{
			health = new EntityStatBar(startingHealth);
		}

		public override void TakeDamage(Entity attackingEntity, float damage)
		{
			// uncomment if you'd like asteroids to collide with one-another
			if (attackingEntity is ProjectileEntity || attackingEntity is ShipEntity)// || attackingEntity is Asteroid) 
			{
				base.TakeDamage(attackingEntity, damage);
				if (Dying)
					return;

				if (ShowDamageOnEntity == true)
				{
					ShowDamageLocation = this.transform.position;
				}

				ShowDamage(damage, ShowDamageLocation);

				health.Damage(damage);

				if (health.IsEmpty())
					KillEntity();
			}
		}

		public override void OnEntityKilled()
		{
			LootController.Instance.LootDrop(lootConfig.Gain, DropType.Asteroid, Position);

			base.OnEntityKilled();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!IsInitalized)
				return;

			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		public override void OnEntityHit(Entity other)
		{
			ApplyCollisionDamage(Rigidbody, other);
			base.OnEntityHit(other);
		}
	}
}
