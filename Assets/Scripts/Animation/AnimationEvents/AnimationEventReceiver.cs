using System.Collections.Generic;
using UnityEngine;

namespace Animation.AnimationEvents
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField] private List<AnimationEvent> _animationEvents = new();
        public void OnAnimationTrigger(string eventName)
        {
            AnimationEvent matchingEvent = _animationEvents.Find(se => se.eventName == eventName);
            matchingEvent?.onAnimationEvent?.Invoke();
        }
    }
}