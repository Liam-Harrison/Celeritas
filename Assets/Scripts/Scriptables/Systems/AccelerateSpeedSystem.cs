using Assets.Scripts.Scriptables.Interfaces;
using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Applies a multiplier to a projectile's speed.
	/// If exponential, it will re-apply the multiplier at a set interval
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile Acceleration System", menuName = "Celeritas/Modifiers/Projectile Acceleration")]
	public class AccelerateSpeedSystem : ModifierSystem, IEntityUpdatedInterval, IEntityEffectAdded, IEntityEffectRemoved
	{
		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile;

		private int timesTriggered;

		[SerializeField, Title("Exponential (leave unticked for linear effect)")]
		bool exponential; // exponential means the effect will be re-applied every update. Linear means it will be applied once.
		[SerializeField, Title("Factor. Use <1 for dampening, >1 for acceleration")]
		float speedFactor;

		public override string GetTooltip(ushort level)
		{
			if (speedFactor > 1)
				return $"<color=green>▲</color> Speeds up motion by a factor of " + speedFactor;
			else
				return $"<color=red>▼</color> Slows down motion by a factor of "+speedFactor;
		}

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			ProjectileEntity target = entity as ProjectileEntity;
			target.SpeedModifier *= speedFactor;
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			ProjectileEntity target = entity as ProjectileEntity;
			target.SpeedModifier /= speedFactor;
			if (exponential) // untested as not used in shrapnel cannon. (Effect isn't removed on projectiles there)
			{
				target.SpeedModifier /= (speedFactor * timesTriggered);
			}
		}

		public void OnEntityUpdatedAfterInterval(Entity entity, ushort level)
		{
			ProjectileEntity target = entity as ProjectileEntity;
			target.SpeedModifier *= speedFactor; // += could be handy for laser
			timesTriggered++;
		}
	}
}
