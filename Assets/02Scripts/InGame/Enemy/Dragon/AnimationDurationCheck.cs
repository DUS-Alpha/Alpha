using UnityEngine;

public class AnimationDurationCheck : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var DBA = animator.GetComponent<DragonBossActions>();
        DBA.IsRunning = false;
    }
}
