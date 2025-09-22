using UnityEngine;

public class ResetPatternOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.SetInteger(AnimID.Pattern,0);
        Debug.Log("Pattern 0으로 초기화 완료");
        animator.ResetTrigger(AnimID.DoSubPatterm);
    }
}
