using UnityEngine;

public class IsActionSMB : StateMachineBehaviour
{
    private PlayerCombat m_combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_combat == null)
            m_combat = animator.GetComponent<PlayerCombat>();

        m_combat.SetIsAction(true);
        m_combat.IsActioning = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(layerIndex))
        {
            // 종료 시점 로직
            m_combat.IsActioning = false;
        }
        else if(!m_combat.IsAttack)
        {
            m_combat.IsActioning = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_combat.IsAttack) m_combat.SetIsAction(false);
    }

    private void EndActionIfPossible(bool force = false)
    {
        if (m_combat == null) return;

        // 강제 종료 또는 공격이 아닌 상태라면 종료
        if (force || !m_combat.IsAttack)
        {
            m_combat.IsActioning = false;
            m_combat.SetIsAction(false);
        }
    }
}
