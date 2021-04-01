using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a projectile.
	/// </summary>
	public class ProjectileEntity : Entity
	{
		/// <summary>
		/// The attatched projectile data.
		/// </summary>
		public ProjectileData ProjectileData { get; private set; }

		/// <summary>
		/// The weapon this projectile was fired from.
		/// </summary>
		public WeaponEntity Weapon { get; private set; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Projectile; }

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null)
		{
			ProjectileData = data as ProjectileData;
			Weapon = owner as WeaponEntity;
			base.Initalize(data, owner, effects);
		}

		private void OnTriggerEnter(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		protected override void Update()
		{
			transform.position += transform.forward * ProjectileData.Speed * Time.smoothDeltaTime;

			if (TimeAlive >= ProjectileData.Lifetime) Destroy(gameObject);

			base.Update();
		}
	}
}
