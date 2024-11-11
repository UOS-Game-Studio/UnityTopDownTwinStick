using Player;
using UnityEngine;

namespace Animation
{
    public class RollAnimationBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log(animator.gameObject.name + ": Attack StateExit");
            RollControl rollControl = animator.GetComponent<RollControl>();

            if (rollControl)
            {
                rollControl.OnRollEnd();
            }
            
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
