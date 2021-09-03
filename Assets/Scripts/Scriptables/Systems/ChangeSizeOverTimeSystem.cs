using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Changes an entity's size over a set duration
	/// Created for Charge Weapon's main projectile, so the initial size is set by the weapon's charge level
	/// Will then increase/decrease the projectile's size by the set amount over the set duration.
	/// </summary>
	[CreateAssetMenu(fileName = "New ChangeSizeOverTime System", menuName = "Celeritas/Modifiers/ChangeSizeOverTime")]
	class ChangeSizeOverTimeSystem : ModifierSystem, IEntityEffectAdded, IEntityUpdated, IEntityEffectRemoved
	{
		[SerializeField, Title("Change Size Over Time Properties", "(0.5 = (0.5, 0.5, 0.5) in prefab scale)")]
		private bool baseStartSizeOffWeaponCharge; // if true, should be projectile

		[SerializeField, ShowIf(nameof(baseStartSizeOffWeaponCharge))]
		private float startSizeMultiplier;

		[SerializeField, HideIf(nameof(baseStartSizeOffWeaponCharge))]
		private float startSize; // set start size if not scaling off weapon charge

		[SerializeField]
		private float endSize;

		[SerializeField, Title("Duration of size change properties")]
		private bool durationProportionalToWeaponCharge;

		[SerializeField, HideIf(nameof(durationProportionalToWeaponCharge))]
		private float duration;

		[SerializeField, ShowIf(nameof(durationProportionalToWeaponCharge))]
		private float durationMultiplier;

		public class ChangeSizeOverTimeData
		{
			public float startTime_s; // time the effect was added
			public float sizeChangePerSecond;
			public bool firstUpdate;
		}

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"Size increases over <color=\"green\">{duration}</color> seconds.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			// check if data exists, if not create some
			if (!entity.Components.TryGetComponent(this, out ChangeSizeOverTimeData data))
			{
				data = new ChangeSizeOverTimeData();
				entity.Components.RegisterComponent(this, data);
				data.firstUpdate = true;
			}
			data.startTime_s = entity.TimeAlive;

			if (baseStartSizeOffWeaponCharge) // calculate start size based off weapon charge
			{
				ProjectileEntity projectile = entity as ProjectileEntity;
				startSize = projectile.Weapon.Charge * startSizeMultiplier;
			}

			if (durationProportionalToWeaponCharge)
			{
				ProjectileEntity projectile = entity as ProjectileEntity;
				duration = projectile.Weapon.Charge * durationMultiplier;
			}

			// calculate size delta per second
			data.sizeChangePerSecond = (endSize - startSize) / duration;

			// entity.transform.localScale = startSize * Vector3.one; // must be done in update apparently.
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			// if duration has passed, don't do anything
			var data = entity.Components.GetComponent<ChangeSizeOverTimeData>(this);
			if (entity.TimeAlive - data.startTime_s > duration)
			{
				return;
			}
			if (data.firstUpdate) // doing this here instead of OnEffectAdded, as that one gets over-ridden to (1,1,1)
			{
				entity.transform.localScale = startSize * Vector3.one;
				data.firstUpdate = false;
			}
			else
			{
				// increase size
				float sizeToAdd = data.sizeChangePerSecond * Time.deltaTime;
				entity.transform.localScale += sizeToAdd * Vector3.one;
			}

		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			entity.Components.ReleaseComponent<ChangeSizeOverTimeData>(this);
		}

		private Vector3 GetVectorWithMagnitude(float magnitude)
		{
			// note: as game is 2d not 3d, should it be /2?
			magnitude = magnitude / 3;
			float sqrt = Mathf.Sqrt(Mathf.Abs(magnitude));
			if (magnitude < 0)
				return (new Vector3(sqrt, sqrt, sqrt) * -1);
			return new Vector3(sqrt, sqrt, sqrt);
			//return new Vector3(magnitude, magnitude, magnitude);
		}
	}
}
