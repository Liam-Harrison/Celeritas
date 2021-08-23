using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// A Global instance which determined what state the game is currently in eg, Play, Build, Boss, Spacestation.
	/// States are managed using the Animator assigned to it. Each state/action in the Animator controller needs to include
	/// The StateManagerBehaviour script.
	/// </summary>
	public class CameraStateManager : Singleton<CameraStateManager>
	{

		[SerializeField, Required]
		private Animator animator;

		public static CinemachineBrain CinemachineBrain { get; private set; }

		protected override void Awake()
		{
			CinemachineBrain = FindObjectOfType<CinemachineBrain>();

			base.Awake();
		}

		private void OnEnable()
		{
			GameStateManager.onStateChanged += OnStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnStateChanged;
		}

		private void OnStateChanged(GameState old, GameState state)
		{
			animator.Play(state.ToString());
		}
	}

}
