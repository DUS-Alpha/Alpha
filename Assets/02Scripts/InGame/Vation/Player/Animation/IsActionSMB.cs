using UnityEngine;

namespace alpha
{
    public class IsActionSMB : StateMachineBehaviour
    {
        [SerializeField]
        private int comboIndex;
        private PlayerCombat m_combat;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_combat == null)
                m_combat = animator.GetComponent<PlayerCombat>();

            m_combat.IsActioning = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float animLength = stateInfo.length;
            float currentTime = stateInfo.normalizedTime * animLength;
            if (currentTime >= animLength && !animator.IsInTransition(layerIndex))
            {
                // 종료 시점 로직
                m_combat.IsActioning = false;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}