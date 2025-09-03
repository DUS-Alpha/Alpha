using UnityEngine;
using Combat;

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

    enum m_Phase { MoveLoop, IdleHold, Attack, Recover }
    
    

    [SerializeField]
    CombatRange m_RagneState = CombatRange.Close;
    [SerializeField]
     m_Phase mPhase = m_Phase.MoveLoop;
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
    
    /// <summary>
    /// 초기화 함수 
    /// </summary>
    /// <param name="a"></param>
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

        switch (mPhase)
        {
            case m_Phase.MoveLoop: DoMoveLoop(); break;
            case m_Phase.IdleHold: DoIdleHold(); break;
            case m_Phase.Attack:   DoAttack();   break;
            case m_Phase.Recover:  DoRecover();  break;
        }
        return NodeState.Running;
    }
    
    // 반환형 버전만 남기기 (기존 void SearchRange()는 삭제 or 이름 변경)
    CombatRange SearchRange()
    {
        float dis= Vector3.Distance(transform.position, actions.BB.Target.position);
        //clsoe 사거리과 작거나 같으면 
        if (dis <= actions.CloseRange)
        {
            return CombatRange.Close;
        }
        //close 사거리보단 크고 mid 사거리와 작거나 같으면
        if (dis > actions.CloseRange && dis <= actions.MidRange)
        {
            return CombatRange.Mid;
        }

        
        return CombatRange.Far;
    }

    void DoMoveLoop()
    {
      
        
        // 반환값: 이번 프레임에서 공격으로 넘어갈지
        bool wantAttackNow = stance.TickAndApply(combatMover, actions.BB.Target);

        if (wantAttackNow || stance.IsCycleFinished)
        {
            mPhase = m_Phase.IdleHold;
            phaseTimer = Random.Range(idleHold.x, idleHold.y);
        }
    }
    
    void DoIdleHold()
    {
        ZeroLocomotion();
        
        phaseTimer -= Time.deltaTime;
        if (phaseTimer <= 0f)
        {
            mPhase = m_Phase.Attack;
            StartAttack();
        }
    }
    
    //어택 중이 아닐때를 위한 함수 
    void DoAttack()
    {
        ZeroLocomotion();
        m_RagneState=SearchRange();

        // Sprint 패턴 실행 중
        if (animator.GetBool("Sprint"))
        {
            // 일정 거리 이내 들어오면 Sprint 종료
            if (m_RagneState != CombatRange.Far)
            {
                animator.SetBool("Sprint", false);
                attackRunning = false;
            }
        }
        
        if (!attackRunning)
        {
            mPhase = m_Phase.Recover;
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
            mPhase = m_Phase.MoveLoop;
        }
    }
  
    

   // StartAttack 교체
    void StartAttack()
    {
        ZeroLocomotion();

        m_RagneState=SearchRange();                      // ✅ 바로 범위 얻기
        
        string trig = attacks.PickTriggerRandom(m_RagneState);   // AttackSelector가 거리별 배열에서 랜덤 반환

        if (string.IsNullOrEmpty(trig))
        {
            Debug.LogWarning($"[CombatDirector] {m_RagneState} 범위 트리거가 없음 → 루프 복귀");
            stance.ResetCycle();
            mPhase = m_Phase.MoveLoop;
            return;
        }
        // Sprint 패턴일 경우
        if (trig == "Sprint")
        {
            animator.SetBool("Sprint", true);  // 루프 재생
            attackRunning = true;
            return;
        }
     
        //애니메이션 넣고  테스트 해보기 
        animator.ResetTrigger(trig);
        animator.SetTrigger(trig);
        attackRunning = true; // 애니 이벤트에서 해제
    }


    // 애니 이벤트에서 호출
    public void OnAnimationFinished_Attack() => attackRunning = false;
}
