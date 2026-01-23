using UnityEngine;

namespace alpha
{
    public class EmptySMB : StateMachineBehaviour
    {
        PlayerCombatManager m_combat;
        [SerializeField] private int m_layerNum;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            PlayerLocomotionManager _locomotion = animator.GetComponent<PlayerLocomotionManager>();
            
            // TODO : 컴뱃 묶기
            m_combat = animator.GetComponent<PlayerCombatManager>();
           // m_combat.SetIsCombatLock(false);
            
            //if(m_layerNum == 4)
            //    m_combat.SetNextComboNum(0);

            //m_combat.SetIsAction(false);

            animator.SetLayerWeight(m_layerNum, 0);
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetLayerWeight(m_layerNum, 1);
        }
    }
}