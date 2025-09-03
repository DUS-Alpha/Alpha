using UnityEngine;

public class CombatMover : MonoBehaviour {
    
    public Animator animator;

    // [Header("정책(무엇을 할지)")]
    public MovePolicy policy = new MovePolicy();

    // [Header("실행기(어떻게 할지)")]
    public BossLocomotionExecutor exec;

    void Awake() {
        // 같은 오브젝트에 Executor가 없다면 자동으로 찾아서 채워줌
        if (!exec) exec = GetComponent<BossLocomotionExecutor>();
        // Animator도 비어 있으면 자식에서 자동 검색
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// BT에서 매 프레임 호출되는 이동 액션.
    /// target은 보통 enemy.BB.Target을 넘겨준다.
    /// </summary>
    public NodeState CombatMove(Transform target) {
        if (!target) return NodeState.Failure; // 타깃 없으면 실패로 반환(상위에서 처리)

        // Executor에 이번 프레임 사용할 참조 주입
        exec.target   = target;
        exec.animator = animator;

        // 정책으로 모드 결정 → 실행기에서 실제 이동 처리
        var mode = policy.Decide(transform.position, target);
        
        exec.Tick(mode,policy.idealRadius ,Time.deltaTime);

        // 이동은 지속행동이므로 항상 Running 유지
        return NodeState.Running;
    }
}