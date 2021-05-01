using UnityEngine;
using Sirenix.OdinInspector;
using Celeritas.Game;
using Cinemachine;

/// <summary>
/// A Global instance which determined what state the game is currently in eg, Play, Build, Boss, Spacestation.
/// States are managed using the Animator assigned to it. Each state/action in the Animator controller needs to include
/// The StateManagerBehaviour script.
/// </summary>
public class CameraStateManager : Singleton<CameraStateManager>
{
	/// <summary>
	/// States that match the names of actions inside the animator.
	/// </summary>
	public enum States
	{
		BUILD,
		PLAY,
		WAVE,
		BOSS,
		SPACE_STATION
	}

	[SerializeField, Required]
	private Animator animator;

	public static CinemachineBrain CinemachineBrain { get; private set; }

	protected override void Awake()
	{
		CinemachineBrain = FindObjectOfType<CinemachineBrain>();

		base.Awake();
	}

	/// <summary>
	/// Checks what state the manager is currently in
	/// </summary>
	/// <returns>True if it matches the specified state, otherwise false</returns>
	public bool IsInState(States state)
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName(state.ToString());
	}

	public delegate void StateChanged();
	public static event StateChanged onStateChanged;
	/// <summary>
	/// Triggers the onStateChanged Event
	/// </summary>
	public void onStateChangedTrigger()
	{
		if (onStateChanged != null) onStateChanged();
	}

	/// <summary>
	/// Changes the game state to the specified
	/// </summary>
	public void ChangeTo(States state)
	{
		animator.Play(state.ToString());
	}
}

