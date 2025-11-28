using UnityEngine;

public class SetTriger : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("진입");
        animator.SetTrigger(AnimID.DoSubPatterm);
    }

  
    
    
}
