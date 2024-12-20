using AI;
using UnityEngine;

namespace Animation
{
    public class AttackAnimationState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            BaseAttack attack = animator.GetComponent<BaseAttack>();

            if (attack)
            {
                attack.AttackEnded();
            }
            
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
