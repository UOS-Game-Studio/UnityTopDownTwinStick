using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class S_RandomAnimation : StateMachineBehaviour
    {
        [SerializeField] private AnimationClip[] clipNames = Array.Empty<AnimationClip>();

        private bool _canPickRandomAnimation = true;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_canPickRandomAnimation)
            {
                _canPickRandomAnimation = false;
                int index = Random.Range(0, clipNames.Length);
                string stateName = clipNames[index].name;
                
                animator.CrossFade(stateName, .1f, layerIndex);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _canPickRandomAnimation = true;

        }
    }
}
