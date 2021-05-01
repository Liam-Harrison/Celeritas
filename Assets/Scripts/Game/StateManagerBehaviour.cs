using UnityEngine;

/// <summary>
/// Applied to states in the Animator in order to trigger state changes in the StateManager
/// </summary>
public class StateManagerBehaviour : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		CameraStateManager.Instance.onStateChangedTrigger();
	}
}

