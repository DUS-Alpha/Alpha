
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class BossActions : MonoBehaviour
{
    // ■ A. 외부 참조 / 기본 제어
    [SerializeField] private BehaviorTreeRunner m_treeRunner;   // 죽음 처리용 (BT 제어)
    public bool IsDead
    {
        get { return m_TestHp <= 0f; }
    }
    public Blackboard BB { get; private set; }                  // 블랙보드 (타깃/위치 등 공유 상태)
    public void SetBlackboard(Blackboard bb) => BB = bb; // 함수 필요 
    public Animator animator;                                   // 보스 애니메이터
    public GameObject Dolly;                                    // 돌리 액션 제어용 오브젝트
    public Transform ReadyPos;                                  // 격돌 준비 위치
    public GameObject PatternPos;                               // 특수 패턴/격돌 포지션
    
    // ■ B. 전투 / 거리 관련
    public float MidRange = 10f;        // 중거리 기준
    public float CloseRange = 5f;       // 근거리 기준
    public float StopRange = 3f;        // 더 이상 접근하지 않는 최소 거리
    const float StopRadius = 3f;        // 플레이어 접근 최소 거리(상수)
    float stopSqr = StopRadius * StopRadius;  // StopRadius 제곱 (거리 비교용)

    public enum CombatRange { Close, Middle, Far }  // 전투 거리 구분
    [SerializeField] private CombatRange m_range = CombatRange.Far;

    // ■ C. 이동 / 타이머
    public float currentTime = 2;           // 현재 이동 타이머
    [SerializeField] private float m_Timer = 2;    // 이동 유지 시간(초)
    [SerializeField] float faceTurnSpeed = 7f;     // 회전 속도 (보간)
    public bool Isarrived = false;                 // 준비 위치 도착 여부

    // ■ D. 패턴 / 특수 상태
    [SerializeField] private bool m_isRun = false; // 특수 패턴 이미 실행 여부
    [SerializeField] public float m_TestHp = 100;  // 테스트용 HP
    [SerializeField] private bool _attackStarted;  // 공격 애니메이션 시작 여부
    private bool IsSpeicalPattern = false;         // 특수 패턴 실행 여부

    // ■ E. 게이트 (QTE) 관련
    [SerializeField] private float gateWindowDuration = 2f; // QTE 윈도우 지속 시간
    [SerializeField] private float gateWindowDeadline;      // 윈도우 마감 시각
    private bool gateWindowActive;                          // 윈도우 활성 여부
    public bool GateOpen { get; private set; }              // 게이트 성공 여부

    [SerializeField] float m_AnswerTimer = 2;   // 입력 타이밍 목표값 (격돌 타이머)
    [SerializeField] float m_Window = 0.25f;    // 입력 허용 오차(±초)

    // ■ F. 패턴 카운터 / 내부 상태
    public int SuccessCount;     // 격돌 패턴 성공 횟수
    public int FailureCount;     // 격돌 패턴 실패 횟수
    public int _resolvedCount;   // 패턴 처리 횟수

    bool m_Started;              // 내부: 패턴 시작 여부
    int m_Index;                 // 내부: 진행 인덱스
    float m_NextTime;            // 내부: 다음 동작 시간

    // ■ G. 테스트 / 임시
    public List<GameObject> TestGaks;   // 테스트용 객체 리스트


    

    // 1) 죽음 조건을 액션으로 (Success=죽음, Failure=생존)
    public NodeState CheckDeathCondition()
    {
        return IsDead ? NodeState.Success : NodeState.Failure;
    }

    // 2) 죽음 정리(이동/연출 끄기 등)
    public NodeState CleanupOnDeath()
    {
        animator.SetBool(AnimID.DoWalk, false);
        if (PatternPos) PatternPos.SetActive(false);
        if (Dolly) Dolly.SetActive(false);
        // Rigidbody/NavMeshAgent 쓰면 여기서 정지 처리
        return NodeState.Success;
    }

    // 3) 죽음 애니메이션 트리거
    public NodeState PlayDieAnim()
    {
        animator.ResetTrigger("DieAni");
        animator.SetTrigger("DieAni");
        // 애니 이벤트로 후처리할 거면 Running으로 두고 완료 시 Success로 바꿔도 됨
        return NodeState.Success;
    }

    // 4) BT 중지
    public NodeState StopTreeAction()
    {
        if (m_treeRunner) m_treeRunner.StopTree();
        
        return NodeState.Success;
    }

    //움직이는 노드
    public NodeState Move()
    {
        currentTime  -= Time.deltaTime;
        
        
        // 1) 타겟을 향한 수평 벡터 (거리)
        Vector3 to = BB.Target.position - BB.OwnerTransform.position;
        to.y = 0f;
        if (to.sqrMagnitude > 0.0001f)
        {
            var look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * faceTurnSpeed);
        }
        
        // 4) 이동 중이므로 Running 유지 (정지/도착 조건에서만 Success)
        //특정 거리에 들어오면  DoWalk=fasle하고  Success 보내기
        //거리 계산 
        //정지 조건 1 플레이어에게 붙을 수 있는 최소 거리에 도착 했을때 
        //정지 조건 2 move를 m_Timer초 이상했을때 
        if (to.sqrMagnitude <= stopSqr || currentTime <= 0f)
        {

            if (to.sqrMagnitude <= 3)
            {
                print("최소 거리 도착 → 이동 종료");
            }
            else
            {
                print($"이동 5초 경과 → 이동 종료");
            }
            currentTime = m_Timer; //초기값으로 정상화
            print($"정상화 된 타이머 시간 {currentTime}");
            
            return NodeState.Success;
        }
        return NodeState.Running;
    }

    //범위 확인
    public NodeState SearchRange()
    {
        //플레이어와 적의 거리를 구해서 그 거리가 설정한 범위와 비교 후 enum 보내기 
        Vector3 to = BB.Target.position - BB.OwnerTransform.position;
        to.y = 0f;
        
        float d2 = to.sqrMagnitude;        // 제곱 거리
        float close2 = CloseRange * CloseRange; // 예: 5f 등
        float mid2   = MidRange   * MidRange;   // 예: 10f 등
        
        if (d2 <= close2)       m_range = CombatRange.Close;
        else if (d2 <= mid2)    m_range = CombatRange.Middle;
        else                    m_range = CombatRange.Far;
        
        return NodeState.Success;
    }
    
    // BossActions.cs (일부)

    /// <summary>
    /// 범위와 애니메이션 인덱스를 받아서 공격을 실행하는 공통 함수
    /// </summary>
    private NodeState PlayAttack(CombatRange needRange, int animId, bool triggerSubPatternOnFar = false)
    {
        if (m_range != needRange)
            return NodeState.Failure;

        // [시작 블록] ─ 한 번만 실행
        if (!_attackStarted)
        {
            Initanitriger(animId);
            _attackStarted = true;
            return NodeState.Running;
        }

        // [진행 블록] ─ 매 틱 실행 (여기에 둬야 함!)
        if (triggerSubPatternOnFar)
        {
            float dis = Vector3.Distance(BB.Target.position, BB.OwnerTransform.position);
            if (dis < StopRange)
                animator.SetTrigger(AnimID.DoSubPatterm);
        }
        
        // 애니가 끝났는지 확인
        if (animator.GetInteger(AnimID.Pattern) != 0)
            return NodeState.Running;

        _attackStarted = false;
        return NodeState.Success;
    }

// ──────────────────────────────────────────────
// 이하 개별 액션들은 단순히 공통 함수 호출만
// ──────────────────────────────────────────────

    public NodeState CloseRangeAttack1()
        => PlayAttack(CombatRange.Close, (int)AnimID.ClosePatternList[0]);

    public NodeState CloseRangeAttack2()
        => PlayAttack(CombatRange.Close, (int)AnimID.ClosePatternList[1]);

    public NodeState MidRangeAttack1()
        => PlayAttack(CombatRange.Middle, (int)AnimID.MiddlePatternList[0]);

    public NodeState MidRangeAttack2()
        => PlayAttack(CombatRange.Middle, (int)AnimID.MiddlePatternList[1]);

    public NodeState FarRangeAttack1()
        => PlayAttack(CombatRange.Far, (int)AnimID.FarPatternList[0], triggerSubPatternOnFar: true);

    public NodeState FarRangeAttack2()
        => PlayAttack(CombatRange.Far, (int)AnimID.FarPatternList[1]);
    

    #region 특수 패턴
  
    public void QTESuccessAddOne()
    {
        SuccessCount++;
        _resolvedCount++;
        Debug.Log($"QTE 성공 누적: {SuccessCount}");
    }
    public void QTEFailureAddOne()
    {
        FailureCount++;
        _resolvedCount++;
        Debug.Log($"QTE 성공 누적: {FailureCount}");
    }

    public void OpenSpecialGate()    // ← 이벤트가 호출
    {
        // Zone에서 G가 눌렸을 때 호출됨
        if (!gateWindowActive)
        {
            print("안들어왔음");
            return; // 윈도우 아닐 때 누른 건 무시
        }
        
        GateOpen = true;
        // 장판 비주얼 끄고 싶으면 여기서: if (PatternPos) PatternPos.SetActive(false);
    }
    
    // 1) 윈도우 시작: 장판을 켜고, 2초 타이머 오픈
    public NodeState StartGateWindow()
    {
        GateOpen = false;
        gateWindowActive = true;
        gateWindowDeadline = Time.time + gateWindowDuration;
        if (PatternPos) PatternPos.SetActive(true); // 장판 ON
        return NodeState.Success;
    }
    
    // 2) 게이트 대기(성공/실패 판정 포함)
    //   - GateOpen==true → 성공(장판 끄고 윈도우 종료)
    //   - 시간 초과 → 실패(장판 끄고 윈도우 종료)
    public NodeState WaitGateOrTimeout()
    {
        if (GateOpen)
        {
            // 성공 진입
            gateWindowActive = false;
            if (PatternPos) PatternPos.SetActive(false); // 장판 OFF
            Debug.Log("Gate success within time window.");
            return NodeState.Success;
        }

        if (Time.time >= gateWindowDeadline)
        {
            // 실패(시간 초과)
            gateWindowActive = false;
            if (PatternPos) PatternPos.SetActive(false); // 장판 OFF
            Debug.Log("Gate failed: no input within time window.");
            // 필요한 실패 카운팅/패널티가 있으면 여기서
            FailureCount++;
            _resolvedCount++;
            return NodeState.Failure;
        }

        return NodeState.Running;
    }
    
    public NodeState OnGateFailed()
    {
        // 예: 실패 연출(애니메이션 트리거 등)
        // animator.SetTrigger("FailTaunt");
        Debug.Log("Special gate failed → fallback behavior.");
        // 필요하면 상태 초기화, 쿨다운 설정 등 추가
        return NodeState.Success; // 셀렉터의 실패 브랜치를 성공으로 마무리
    }
    

    public void TrueArrived()
    {
        Isarrived = true;
    }

    /*
     * 특별 패턴(격돌)을 진입하기전 조건을 확인
     * 격돌에 필요한 체력 이하가 됐는가?,시전을 했는가 ?
     * 보스가 격돌을 시전하기 위해 저장된 위치로 이동
     * 그 후 격돌 준비 자세를 실행
     */
    public NodeState ConditionPattern()
    {
        if (m_isRun)
        {
            print("패턴이 이미 한번 실행했음");
            return NodeState.Failure;
        }
        if (m_TestHp > 50) // 패턴 테스트 hp 나중에 수정
        {
            print("패턴 조건 안맞음");
            return NodeState.Failure;
        }

        // 1) ReadyPos 쪽으로 바라보며 이동
        Vector3 toReady = ReadyPos.position - BB.OwnerTransform.position;
        toReady.y = 0f;
        if (toReady.sqrMagnitude > 0.0001f)
        {
            var look = Quaternion.LookRotation(toReady.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * faceTurnSpeed);
        }

        animator.SetBool(AnimID.DoWalk, true);

        // 2) 도착 후: PatternPos 쪽으로 정렬 → 정지 → 플레이어(타깃) 주시
        // 2) 도착 후: 플레이어(타깃)만 바라보기 → 각도 수렴 시 Success
        if (Isarrived)
        {
            animator.SetBool(AnimID.DoWalk, false);

            Transform target = BB.Target; // 타깃 참조
            if (target != null)
            {
                transform.LookAt(target);
            }

            // 충분히 플레이어를 보게 되었으면 성공
            return NodeState.Success;
        }

        return NodeState.Running;
    }


    public NodeState ShowPatternPos()      // 선택: 장판 켜기용
    {
        if (PatternPos) PatternPos.SetActive(true);
        return NodeState.Success;
    }
    public NodeState HidePatternPos()      // 선택: 장판 끄기용
    {
        if (PatternPos) PatternPos.SetActive(false);
        return NodeState.Success;
        
    }

   
    public NodeState SpecialPattern()
    {
        //특수 패턴에 들어가는 조건을 추가
        //예) 피가 50 아래면 실행  나중에 추가하기 
        /*
         * 1. 특수 패턴을  시작하게 할 장판 오브젝트를 보이게하기
         * 2. 장판안에서 시작하게하는 특수키(현재는 G로 지정)를 입력시 아래 패턴을 실행하게고정 
         */
        // 1) 최초 진입 시 초기화
        if (!m_Started)
        {
            m_Index   = 0;
            m_NextTime = Time.time; // 첫 개체는 즉시 켜기
            m_Started = true;
            return NodeState.Running;
        }
        
        // 2) 시간 간격마다 다음 오브젝트 활성화
        if (m_Index < TestGaks.Count && Time.time >= m_NextTime)
        {
            var go = TestGaks[m_Index];
            if (go) go.SetActive(true);

            m_Index++;
            m_NextTime = Time.time + Mathf.Max(0.01f, 0.3f);
        }
        
        // 아직 켤 게 남아 있으면 계속 진행
        if (m_Index < TestGaks.Count) return NodeState.Running;

        if (_resolvedCount == 4)
        {
            if (SuccessCount > 2 )
            {
                print("성공");
                return NodeState.Success;
            }
            /*m_Started = false;*/
            print("실패~");
            return NodeState.Failure;
        }
        
        return NodeState.Running;
    }
    
    #endregion

    #region 패턴 랜덤 실행기

    //근거리
    public NodeState CloseRandomAni()
    {
        
        if (m_range != CombatRange.Close)
        {
            return NodeState.Failure;
        }
        int index = Random.Range(0,AnimID.ClosePatternList.Count);
        switch (index)
        {
            case 0:
                return CloseRangeAttack1();
            case 1:
                return CloseRangeAttack2();
                
        }
        return NodeState.Running;
    }
    
    //중거리
    public NodeState MiddleRandomAni()
    {
        
        if (m_range != CombatRange.Middle)
        {
            return NodeState.Failure;
        }
        int index = Random.Range(0,AnimID.MiddlePatternList.Count);
        switch (index)
        {
            case 0:
                return MidRangeAttack1();
            case 1:
                return MidRangeAttack2();
                
        }
        return NodeState.Running;
        
    }
    //원거리
    public NodeState FarRandomAni()
    {
        
        if (m_range != CombatRange.Far)
        {
            return NodeState.Failure;
        }
        int index = Random.Range(0,AnimID.FarPatternList.Count);
        switch (index)
        {
            case 0:
                return FarRangeAttack1();
            case 1:
                return FarRangeAttack2();
                
        }
        return NodeState.Running;
    }
    
    #endregion

    //결과 보고 
   




    //애니메이션의 초기화함수 
    void Initanitriger(int picked)
    {
        _attackStarted = true;
        animator.ResetTrigger(AnimID.DoSubPatterm); // 잔여 트리거 제거
        animator.SetInteger(AnimID.Pattern, picked);     // 이번 패턴 지정
    }

    public NodeState SetFalseDolly()
    {
        Dolly.SetActive(false);
        m_isRun = true;
        return NodeState.Success;
    }



}