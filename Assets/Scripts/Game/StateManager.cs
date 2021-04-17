using UnityEngine;
using Sirenix.OdinInspector;
using Celeritas.Game;

/// <summary>
/// A Global instance which determined what state the game is currently in eg, Play, Build, Boss, Spacestation.
/// States are managed using the Animator assigned to it. Each state/action in the Animator controller needs to include
/// The StateManagerBehaviour script.
/// </summary>
public class StateManager : Singleton<StateManager>
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

	// Inspector
	[Required]
	[SerializeField]
	private Animator baseAnimator;

	// Internal
	private static Animator animator;

	private void Start()
	{
		animator = baseAnimator;
	}

	/// <summary>
	/// Checks what state the manager is currently in
	/// </summary>
	/// <returns>True if it matches the specified state, otherwise false</returns>
	public static bool IsInState(States state)
	{
		string stateEnumString = States.GetName(typeof(States), state);
		return animator.GetCurrentAnimatorStateInfo(0).IsName(stateEnumString);
	}

	public delegate void StateChanged();
	public static event StateChanged onStateChanged;
	/// <summary>
	/// Triggers the onStateChanged Event
	/// </summary>
	public static void onStateChangedTrigger()
	{
		if (onStateChanged != null) onStateChanged();
	}

	/// <summary>
	/// Changes the game state to the specified
	/// </summary>
	public static void ChangeTo(States state)
	{
		string stateEnumString = States.GetName(typeof(States), state);
		animator.Play(stateEnumString);
	}
}

