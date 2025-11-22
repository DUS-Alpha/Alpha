using UnityEngine;

namespace alpha
{
    public class UnIsActioninSMB : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerCombat m_combat = animator.GetComponent<PlayerCombat>();

            m_combat.IsActioning = false;
            m_combat.SetIsAction(false);
        }
    }
}