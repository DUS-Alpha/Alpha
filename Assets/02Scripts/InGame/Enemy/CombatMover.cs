using UnityEngine;

public class CombatMover : MonoBehaviour {
    // public Transform target; 
    public Animator animator;
    public MovePolicy policy = new MovePolicy();
    public BossLocomotionExecutor exec;

    void Awake() {
        if (!exec) exec = GetComponent<BossLocomotionExecutor>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        
    }
    public NodeState CombatMove(Transform target) {
        if (!target) return NodeState.Failure;
        exec.target = target; exec.animator = animator;
        var mode = policy.Decide(transform.position, target);
        exec.Tick(mode, policy.idealRadius, Time.deltaTime);
        return NodeState.Running; // ← 이동은 항상 Running
    }
}