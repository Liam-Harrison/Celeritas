﻿using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Applies a multiplier to a projectile's speed.
	/// If exponential, it will re-apply the multiplier at a set interval
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile Acceleration System", menuName = "Celeritas/Modifiers/Projectile Acceleration")]
	public class ModifySpeedSystem : ModifierSystem, IEntityUpdated, IEntityEffectAdded, IEntityEffectRemoved
	{
		public class SpeedSystemData
		{
			public float baseAmount;
			public float modifier;
		}

		[SerializeField, TitleGroup("Over Time")]
		private bool applyOvertime;

		[SerializeField, ShowIf(nameof(applyOvertime)), TitleGroup("Over Time"), InfoBox("Starts at 0, reaches SpeedFactor after duration has elapsed.")]
		private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField, TitleGroup("Over Time"), ShowIf(nameof(applyOvertime))]
		private float duration = 1;

		[SerializeField, TitleGroup("Speed"), InfoBox("Multiplier, use < 1 for dampening, > 1 for acceleration")]
		float speedFactor;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level)
		{
			if (speedFactor > 1)
				return $"<color=green>▲</color> Speeds up motion by a factor of " + speedFactor;
			else
				return $"<color=red>▼</color> Slows down motion by a factor of " +speedFactor;
		}

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			ProjectileEntity target = entity as ProjectileEntity;

			if (applyOvertime)
			{
				var data = new SpeedSystemData { baseAmount = target.SpeedModifier, modifier = 0 };
				entity.Components.RegisterComponent(this, data);
			}
			else
			{
				target.SpeedModifier *= speedFactor;
			}
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			ProjectileEntity target = entity as ProjectileEntity;

			if (applyOvertime)
			{
				target.SpeedModifier = entity.Components.GetComponent<SpeedSystemData>(this).baseAmount;
				entity.Components.ReleaseComponent<SpeedSystemData>(this);
			}
			else
			{
				target.SpeedModifier /= speedFactor;
			}
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			if (!applyOvertime)
				return;

			var projectile = entity as ProjectileEntity;
			var p = curve.Evaluate(Mathf.Clamp01(entity.TimeAlive / duration)) * speedFactor;

			var data = entity.Components.GetComponent<SpeedSystemData>(this);
			data.modifier = p;
			projectile.SpeedModifier = p;
		}
	}
}