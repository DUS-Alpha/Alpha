using UnityEngine;

public class DeathCycle : MonoBehaviour
{
    private Rigidbody rb;
    public Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public NodeState Death()
    {
        print("죽음 시작");
        animator.SetTrigger("Dead");
        // 트리 정지
        var runner = GetComponent<BehaviorTreeRunner>();
        if (runner != null)
            runner.StopTree();
        return NodeState.Success;
    }
    

 
}
