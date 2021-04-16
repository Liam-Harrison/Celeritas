using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Celeritas.Game
{
    public class StateManagerBehaviour : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateManager.StateHasChange();
        }
    }
    
}
