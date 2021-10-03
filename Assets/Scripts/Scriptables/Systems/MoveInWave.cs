using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Makes an entity move in a sin wave pattern
	/// </summary>
	[CreateAssetMenu(fileName = "New SinWaveMotion System", menuName = "Celeritas/Modifiers/Entity Movement/Animation Curve")]
	class MoveInWave : ModifierSystem, IEntityUpdated
	{
		[SerializeField]
		float frequencyMultiplier;

		[SerializeField, Title("Right/Left motion")]
		bool rightLeftMotion; // if false, will use up/down axis instead.

		[SerializeField, ShowIf(nameof(rightLeftMotion))]
		float rightLeftAmplitude;

		[SerializeField, ShowIf(nameof(rightLeftMotion))]
		AnimationCurve rightLeftMotionCurve;

		[SerializeField, ShowIf(nameof(rightLeftMotion))]
		float rightLeftPhase;

		[SerializeField, Title("Forward/Back motion")]
		bool forwardBackMotion;

		[SerializeField, ShowIf(nameof(rightLeftMotion))]
		float forwardBackAmplitude;

		[SerializeField, ShowIf(nameof(forwardBackMotion))]
		AnimationCurve forwardBackMotionCurve;

		[SerializeField, ShowIf(nameof(forwardBackMotion))]
		float forwardBackPhase;

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile; // should be able to work for anything though

		public override string GetTooltip(int level) => $"Moves on a curve.";

		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			if (Time.deltaTime==0) // if paused
				return;

			if (rightLeftMotion)
				entity.transform.position += rightLeftAmplitude * entity.transform.right * rightLeftMotionCurve.Evaluate((frequencyMultiplier * entity.TimeAlive) + rightLeftPhase);

			if (forwardBackMotion)
			{
				entity.transform.position += forwardBackAmplitude * entity.Forward * forwardBackMotionCurve.Evaluate((frequencyMultiplier * entity.TimeAlive) + forwardBackPhase);
			}
		}
	}
}
