using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Celeritas.Game
{
    public class StateManager : Singleton<StateManager>
    {
        public static class States {
            public const string BUILD = "Build";
            public const string PLAY = "Play";
        }

        public delegate void StateChanged();
        public static event StateChanged OnStateChanged;
        
        public static Animator animator;
        [Required]
        public Animator baseAnimator;

        private void Start() {
            animator = baseAnimator;
        }

        public static bool IsInState(string state) {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
        }

        public static void StateHasChange() { OnStateChanged(); }

        public static void ChangeTo(string state) {
            animator.Play(state);
        }
    }
}
