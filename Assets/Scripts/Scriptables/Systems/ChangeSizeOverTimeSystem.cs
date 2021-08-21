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
		[SerializeField, Title("Change Size Over Time Properties", "size is a multiplier (0.5 = 50% original size)")]
		private float duration_s;

		[SerializeField]
		private float endSize;

		[SerializeField]
		private bool baseStartSizeOffWeaponCharge; // if true, should be projectile

		[SerializeField, ShowIf(nameof(baseStartSizeOffWeaponCharge))]
		private float startSizeMultiplier;

		[SerializeField, HideIf(nameof(baseStartSizeOffWeaponCharge))]
		private float startSize; // set start size if not scaling off weapon charge

		public class ChangeSizeOverTimeData
		{
			public float startTime_s; // time the effect was added
			public float sizeChangePerSecond;
		}

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			// check if data exists, if not create some
			if (!entity.Components.TryGetComponent(this, out ChangeSizeOverTimeData data))
			{
				data = new ChangeSizeOverTimeData();
				entity.Components.RegisterComponent(this, data);
			}
			data.startTime_s = entity.TimeAlive;

			if (baseStartSizeOffWeaponCharge) // calculate start size based off weapon charge
			{
				ProjectileEntity projectile = entity as ProjectileEntity;
				startSize = projectile.Weapon.Charge * startSizeMultiplier;
			}

			// calculate size delta per second
			data.sizeChangePerSecond = (endSize - startSize) / duration_s;

			// the problem is here now I think c: 
			//entity.transform.localScale = new Vector3(startSize, startSize, startSize);
			entity.transform.localScale = startSize * entity.transform.localScale;
			//entity.transform.localScale = GetVectorWithMagnitude(startSize);
			//entity.transform.localScale = new Vector3(100, 100, 100);
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			// if duration has passed, don't do anything
			var data = entity.Components.GetComponent<ChangeSizeOverTimeData>(this);
			if (entity.TimeAlive - data.startTime_s > duration_s)
			{
				return;
			}

			// increase size
			float sizeToAdd = data.sizeChangePerSecond * Time.deltaTime;
			//entity.transform.localScale += new Vector3(sizeToAdd, sizeToAdd, sizeToAdd);

			//entity.transform.localScale *= (1 + sizeToAdd); // hm?



			//entity.transform.localScale = (entity.transform.localScale / entity.transform.localScale.magnitude)
			entity.transform.localScale += sizeToAdd * entity.transform.localScale;
			//entity.transform.localScale += GetVectorWithMagnitude(sizeToAdd);

			//entity.transform.localScale +=entity.transform.localScale * sizeToAdd;

			//entity.transform.localScale = new Vector3(entity.transform.localScale.x + sizeToAdd, entity.transform.localScale.y + sizeToAdd, entity.transform.localScale.z + sizeToAdd);
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
