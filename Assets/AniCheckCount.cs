using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class AniCheckCount : StateMachineBehaviour
{
  
    [SerializeField]private float count = 0;


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("나가요");
        
        var anim = animator.GetComponent<FlyTowardTarget>();
        anim.isDone = true;
        count = 0;
        animator.ResetTrigger("Done");

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count += Time.deltaTime;
        if (count > 6)
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Done");
        }
    }
    

    




}
