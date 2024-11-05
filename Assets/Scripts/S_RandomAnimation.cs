using UnityEditor.Animations;
using UnityEngine;

public class S_RandomAnimation : StateMachineBehaviour
{
    [SerializeField] private AnimationClip[] clipNames = new AnimationClip[0];

    private bool canPickRandomAnimation = true;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canPickRandomAnimation)
        {
            canPickRandomAnimation = false;
            int index = UnityEngine.Random.Range(0, clipNames.Length);
            string stateName = clipNames[index].name;
        
            Debug.Log("--------------------------");
            Debug.Log(stateName);
            //Debug.Break();
            //animator.Play(stateName, layerIndex);
            animator.CrossFade(stateName, .1f, layerIndex);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("ON State update");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("on state exit");
        //Debug.Log(stateInfo.shortNameHash);
        canPickRandomAnimation = true;
        
            }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("On State Move");
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}


    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("On State macine enter");
    }

}
