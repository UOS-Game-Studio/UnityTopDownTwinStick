using System;
using UnityEngine.Events;

namespace Animation.AnimationEvents
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent onAnimationEvent;
    }
}