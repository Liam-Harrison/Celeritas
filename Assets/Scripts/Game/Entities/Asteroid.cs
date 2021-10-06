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
		public uint StartingHealth { get; set; }

		[SerializeField, Title("Loot")]
		private LootConfig lootConfig;

		public override SystemTargets TargetType { get => SystemTargets.Asteroid; }

		private EntityStatBar health;

		public EntityStatBar Health { get => health; }

		public AsteroidData AsteroidData { get; private set; }

		public Rigidbody2D Rigidbody { get; private set; }

		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			AsteroidData = data as AsteroidData;
			Rigidbody = GetComponent<Rigidbody2D>();
			health = new EntityStatBar(AsteroidData.Health.x);

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		public void SetHealth(float percent)
		{
			var hp = Mathf.Lerp(AsteroidData.Health.x, AsteroidData.Health.y, percent);
			health.MaxValue = hp;
			health.SetHealth(hp);
		}

		public override void OnSpawned()
		{
			base.OnSpawned();

		}

		public override void OnDespawned()
		{
			base.OnDespawned();
		}

		public override void TakeDamage(Entity attackingEntity, float damage)
		{
			if (attackingEntity is ProjectileEntity || attackingEntity is ShipEntity || attackingEntity == this)
			{
				base.TakeDamage(attackingEntity, damage);
				if (Dying)
					return;

				ShowDamage(damage);

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
