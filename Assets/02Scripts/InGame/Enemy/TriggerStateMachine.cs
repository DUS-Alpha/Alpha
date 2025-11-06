using UnityEngine;

public class TriggerStateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var AttackSc = animator.GetComponent<AttackCycle>();
        if (AttackSc != null && !AttackSc.isTakeoff)
        {
            AttackSc.isTakeoff = true;
        }
    }
}
