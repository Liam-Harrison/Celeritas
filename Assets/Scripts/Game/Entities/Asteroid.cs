using Assets.Scripts.Scriptables.Data;
using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Entity
{
	[SerializeField]
	private uint startingHealth;
	public override SystemTargets TargetType { get => SystemTargets.Asteroid; }

	private EntityStatBar health;

	public AsteroidData AsteroidData { get; private set; }

	public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false)
	{
		AsteroidData = data as AsteroidData;
		startingHealth = AsteroidData.Health;
		base.Initalize(data, owner, effects, forceIsPlayer);
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

}
