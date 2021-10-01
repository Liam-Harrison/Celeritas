using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// When this entity hits another entity, add a system to that other entity.
	/// </summary>
	[CreateAssetMenu(fileName = "New ApplyEffectOnHit System", menuName = "Celeritas/Modifiers/Apply System On Hit")]
	class ApplySystemOnHitSystem : ModifierSystem, IEntityHit
	{
		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile;

		[SerializeField, Title("Effects to apply")]
		private EffectWrapper toApply;

		public override string GetTooltip(ushort level)
		{
			var result = $"Applies following effects on hit:";
			IReadOnlyList<ModifierSystem> systems = toApply.EffectCollection.Systems;

			foreach (ModifierSystem m in systems)
			{
				result += $"\n<indent=7.5%>• {m.GetTooltip(level)}</indent>";
			}
			return result;
		}

		public void OnEntityHit(Entity entity, Entity other, EffectWrapper wrapper)
		{
			toApply.Level = wrapper.Level;
			other.EntityEffects.AddEffect(toApply);
		}
	}
}
