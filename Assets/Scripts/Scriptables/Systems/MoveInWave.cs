using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Makes an entity move in a sin wave pattern
	/// </summary>
	[CreateAssetMenu(fileName = "New SinWaveMotion System", menuName = "Celeritas/Modifiers/Entity Movement/SinWave")]
	class MoveInWave : ModifierSystem, IEntityUpdated
	{
		[SerializeField]
		float amplitude;

		[SerializeField]
		AnimationCurve motionCurve;

		[SerializeField]
		float phase;

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile; // should be able to work for anything though

		public override string GetTooltip(ushort level) => $"";

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			entity.transform.position += amplitude * entity.transform.right * motionCurve.Evaluate(entity.TimeAlive + phase);
		}
	}
}
