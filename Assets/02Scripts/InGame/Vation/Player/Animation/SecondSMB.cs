using UnityEngine;

namespace alpha
{
    public class SecondSMB : StateMachineBehaviour
    {
        private PlayerCombat m_combat;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_combat = animator.GetComponent<PlayerCombat>();
            m_combat.SetActionLock(false);
            //animator.SetLayerWeight(1,0);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_combat.SetActionLock(true);
            animator.SetLayerWeight(1, 1);
        }
    }
}