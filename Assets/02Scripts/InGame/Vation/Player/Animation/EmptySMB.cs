using UnityEngine;

namespace alpha
{
    public class EmptySMB : StateMachineBehaviour
    {
        PlayerCombat m_combat;
        [SerializeField] private int m_layerNum;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            PlayerLocomotion _locomotion = animator.GetComponent<PlayerLocomotion>();
            _locomotion.SetLocomotionLock(false);
            
            
            // TODO : 컴뱃 묶기
            m_combat = animator.GetComponent<PlayerCombat>();
            m_combat.SetIsCombatLock(false);
            
            if(m_layerNum == 4)
                m_combat.SetNextComboNum(0);

            m_combat.SetIsAction(false);

            animator.SetLayerWeight(m_layerNum, 0);
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetLayerWeight(m_layerNum, 1);
        }
    }
}