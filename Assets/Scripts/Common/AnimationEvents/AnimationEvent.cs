using System;
using UnityEngine.Events;

namespace Common.AnimationEvents
{
    [Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent onAnimationEvent;
    }
}