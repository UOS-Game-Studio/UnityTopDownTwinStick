using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

namespace Animation.AnimationEvents
{
    public class AnimationEventStateBehaviour : StateMachineBehaviour
    {
        public string eventName;
        [Range(0, 1f)] public float triggerTime;

        private bool _hasTriggered;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasTriggered = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float currentTime = stateInfo.normalizedTime % 1f;

            if (!_hasTriggered && currentTime >= triggerTime)
            {
                _hasTriggered = true;
                NotifyReceiver(animator);
            }
        }

        private void NotifyReceiver(Animator animator)
        {
            AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();
            if (receiver)
            {
                receiver.OnAnimationTrigger(eventName);
            }
        }
    }
}
