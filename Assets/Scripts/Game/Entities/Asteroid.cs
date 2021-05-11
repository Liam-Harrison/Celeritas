using Assets.Scripts.Scriptables.Data;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities {
	/// <summary>
	/// Asteroid objects that float around space, will give player rare metal when destroyed
	/// todo: rubble effect when asteroids are destroyed
	/// </summary>
	public class Asteroid : Entity, ITractorBeamTarget
	{
		[SerializeField]
		private uint startingHealth;

		[SerializeField, Title("Loot")]
		private LootConfig lootConfig;

		public override SystemTargets TargetType { get => SystemTargets.Asteroid; }

		private EntityStatBar health;

		public AsteroidData AsteroidData { get; private set; }

		public Rigidbody2D Rigidbody { get; private set; }

		[SerializeField]
		private GameObject onDestroyEffectPrefab;

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false)
		{
			AsteroidData = data as AsteroidData;
			startingHealth = AsteroidData.Health;
			Rigidbody = GetComponent<Rigidbody2D>();
			base.Initalize(data, owner, effects, forceIsPlayer);

			onDeathEffect = onDestroyEffectPrefab;
		}

		// Start is called before the first frame update
		void Start()
		{
			health = new EntityStatBar(startingHealth);
		}

		protected override void Update()
		{
			base.Update();
		}

		public override void TakeDamage(Entity attackingEntity, int damage)
		{
			if (attackingEntity is ProjectileEntity || attackingEntity is ShipEntity)
			{
			
				base.TakeDamage(attackingEntity, damage);

				health.Damage(damage);

				if (health.IsEmpty())
				{
					Died = true;
				}

			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		public override void OnEntityHit(Entity other)
		{
			//graphicalEffect = Instantiate(onDestroyEffectPrefab, transform);
			ApplyCollisionDamage(Rigidbody, other);
			base.OnEntityHit(other);
		}

		protected override void OnDestroy()
		{
			// drop loot when destroyed
			if (LootController.Instance != null)
				LootController.Instance.LootDrop(lootConfig.Gain, DropType.Asteroid, Position);

			base.OnDestroy();
		}

	}
}
