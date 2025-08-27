using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    
    static readonly int MoveX = Animator.StringToHash("MoveX");
    static readonly int MoveZ = Animator.StringToHash("MoveZ");
    // 외부 의존성
    BossActions actions;
    public CombatMover combatMover;
    public Animator animator;

    // 내부 모듈
    public StanceController stance;
    public AttackSelector attacks;

    public Vector2 idleHold = new(0.3f, 0.6f);
    public Vector2 recover  = new(0.2f, 0.5f);

    enum Phase { MoveLoop, IdleHold, Attack, Recover }
    
    [SerializeField]
    Phase phase = Phase.MoveLoop;
    [SerializeField]
    float phaseTimer;
    [SerializeField]
    bool attackRunning;

    
    void ZeroLocomotion(bool snap = false)
    {
        // snap=true면 즉시 0, false면 아주 짧게 감쇠
        float damp = snap ? 0f : 0.06f;
        animator.SetFloat(MoveX, 0f, damp, Time.deltaTime);
        animator.SetFloat(MoveZ, 0f, damp, Time.deltaTime);
    }
    public void Bind(BossActions a)
    {
        actions = a;
        if (!combatMover) combatMover = GetComponent<CombatMover>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stance) stance = GetComponent<StanceController>();
        if (!attacks) attacks = GetComponent<AttackSelector>();
        stance.ResetCycle(); // 스탠스 초기화
    }

    public NodeState Tick()
    {
        if (actions.BB == null || actions.BB.Target == null) return NodeState.Failure;

        switch (phase)
        {
            case Phase.MoveLoop: DoMoveLoop(); break;
            case Phase.IdleHold: DoIdleHold(); break;
            case Phase.Attack:   DoAttack();   break;
            case Phase.Recover:  DoRecover();  break;
        }
        return NodeState.Running;
    }

    void DoMoveLoop()
    {
        // 반환값: 이번 프레임에서 공격으로 넘어갈지
        bool wantAttackNow = stance.TickAndApply(combatMover, actions.BB.Target);

        if (wantAttackNow || stance.IsCycleFinished)
        {
            phase = Phase.IdleHold;
            phaseTimer = Random.Range(idleHold.x, idleHold.y);
            return;
        }
    }


    void DoIdleHold()
    {
        ZeroLocomotion();
        
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            
            phase = Phase.Attack;
            StartAttack();
        }
    }

    void DoAttack()
    {
        ZeroLocomotion();
        
        if (!attackRunning)
        {
            phase = Phase.Recover;
            phaseTimer = Random.Range(recover.x, recover.y);
        }
    }

    void DoRecover()
    {
        
        ZeroLocomotion();
        
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            stance.ResetCycle();
            phase = Phase.MoveLoop;
        }
    }

    void StartAttack()
    {
        
        ZeroLocomotion();
        
        // 지금은 랜덤, 나중에 가중치 함수로 교체
        int idx = attacks.PickIndexRandom();
        if (idx < 0)
        {
            print("start 어택이 실행 중이나 1보다 작아서 다시 loop로 돌아감");
            stance.ResetCycle(); 
            phase = Phase.MoveLoop; return;
        }

        string trigger = attacks.GetTrigger(idx);
        Debug.Log("애니메이션 트리거 : " + trigger);
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
        attackRunning = true; // 애니 이벤트에서 false로 풀기
        
    }

    // 애니 이벤트에서 호출
    public void OnAnimationFinished_Attack() => attackRunning = false;
}
