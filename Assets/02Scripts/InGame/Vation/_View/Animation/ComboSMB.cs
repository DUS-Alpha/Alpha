using alpha;
using Unity.VisualScripting;
using UnityEngine;

public class ComboSMB : StateMachineBehaviour
{
    private PlayerCombatManager m_combat;
    public int NextComboNum;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_combat = animator.GetComponent<PlayerCombatManager>();
        m_combat.SetIsAction(true);
        m_combat.SetIsNextCombo(false);

        PlayerLocomotionManager _locomotion = animator.GetComponent<PlayerLocomotionManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float _normalized = stateInfo.normalizedTime;

        // 진행률(0~1)
        float percent = _normalized % 1f;

        // 진행 90%일 때 다음 콤보 가능상태 및 Num정보 전달
        if (percent >= 0.9f)
        {
            m_combat.SetIsNextCombo(true);
            m_combat.SetNextComboNum(NextComboNum);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_combat.SetIsNextCombo(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
