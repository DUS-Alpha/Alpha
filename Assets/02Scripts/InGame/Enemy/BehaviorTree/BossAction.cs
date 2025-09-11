
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class BossActions : MonoBehaviour
{
    public Blackboard BB { get; private set; }
    public float MidRange = 10f;
    public float CloseRange = 5f;
    public float StopRange = 3f; // 플레이어에게 접근 할 수 있는 최소 거리 
    public float currentTime = 2; //현재 타이머
    [SerializeField]private float m_Timer = 2; // 몇초동안 움직일 껀지 
    [SerializeField]private bool m_isRun = false;  //패턴을 실행했나 ?

    [SerializeField]public float m_TestHp = 100;
    
    public enum CombatRange
    {
        Close,
        Middle,
        Far
    }
    
    [SerializeField]private CombatRange m_range = CombatRange.Far;
    
    public Animator animator;
    
    const float StopRadius = 3f;// 플레이어에게 접근 할 수 있는 최소 거리 
    float stopSqr = StopRadius * StopRadius;
    
    // 필드
    [SerializeField]private bool _attackStarted; // 공격 시작했는지
    
    [SerializeField] float faceTurnSpeed = 7f;    // 회전 보간,자연스럽게 회전하기 
    
    
    public void SetBlackboard(Blackboard bb) => BB = bb;
    
    [SerializeField]float m_AnswerTimer = 2; // m_AnswerTimer초에 가깝게 누를 수 있도록하기 격돌타이머
    [SerializeField]float m_Window      = 0.25f;  // 허용 오차(±초)

    
    public List<GameObject> TestGaks;


    public int SuccessCount; // 격돌 패턴 횟수 카운팅

    public int FailureCount;

    public int _resolvedCount;
    // 격돌 내부 상태
    bool  m_Started;
    int   m_Index;
    float m_NextTime;
    
    //Zone키는 함수
    public bool GateOpen { get; private set; }

    //특수 패턴을 했나 안했나 ?
    private bool IsSpeicalPattern = false;
    //격돌 진입 포지션 
    public GameObject PatternPos;

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
    

    #region 근거리패턴
    public NodeState CloseRangeAttack1()
    {
        if (m_range != CombatRange.Close)
        {
            return NodeState.Failure;
        }
        int picked = (int)AnimID.ClosePatternList[0]; //선택된 애니메이션 실행값
        
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            animator.SetTrigger(AnimID.DoSubPatterm);   // 시작!
            return NodeState.Running;
        }

        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        print($"애니메이션 실행 : {picked}");
        
        
        return NodeState.Success;
    }
    //뒤로 열심히 빼기
    public NodeState CloseRangeAttack2()
    {
        if (m_range != CombatRange.Close)
        {
            return NodeState.Failure;
        }
        int picked = (int)AnimID.ClosePatternList[1]; //선택된 애니메이션 실행값
        
        
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            animator.SetTrigger(AnimID.DoSubPatterm);   // 시작!
            return NodeState.Running;
        }
     

        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        print($"애니메이션 실행 : {picked}");
      
        
        return NodeState.Success;
    }
    

    #endregion

    #region 중거리패턴
    public NodeState MidRangeAttack1()
    {
       
        if (m_range != CombatRange.Middle)
        {
            return NodeState.Failure;
        }
        
        int picked = (int)AnimID.MiddlePatternList[0]; //선택된 애니메이션 실행값
        
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            animator.SetTrigger(AnimID.DoSubPatterm);   // 시작!
            return NodeState.Running;
        }

        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        print($"애니메이션 실행 : {picked}");
        
    
        
        return NodeState.Success;
    }
    public NodeState MidRangeAttack2()
    {
       
        if (m_range != CombatRange.Middle)
        {
            return NodeState.Failure;
        }
        
        int picked = (int)AnimID.MiddlePatternList[1]; //선택된 애니메이션 실행값
        
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            animator.SetTrigger(AnimID.DoSubPatterm);   // 시작!
            return NodeState.Running;
        }

        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        print($"애니메이션 실행 : {picked}");
    
        
        return NodeState.Success;
    }
    
    #endregion

    #region 원거리패턴

    public NodeState FarRangeAttack1()
    {
        
        if (m_range != CombatRange.Far)
        {
            return NodeState.Failure;
        }
        
        int picked = (int)AnimID.FarPatternList[0]; //선택된 애니메이션 실행값
        var dis= Vector3.Distance(BB.Target.position, BB.OwnerTransform.position);
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            return NodeState.Running;
        }

        //특정 거리 미만일때 다음 연계 애니메이션을 실행하도록 만들자 
        if (dis < StopRange) {animator.SetTrigger(AnimID.DoSubPatterm);}
        
        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        
        return NodeState.Success;
    }
    
    public NodeState FarRangeAttack2()
    {
        
        if (m_range != CombatRange.Far)
        {
            return NodeState.Failure;
        }
        
        int picked = (int)AnimID.FarPatternList[1]; //선택된 애니메이션 실행값
        
        // 처음 한 번만 시작
        if (!_attackStarted)
        {
            Initanitriger(picked);
            return NodeState.Running;
        }
        // 진행 중이면 계속 대기
        if (animator.GetInteger(AnimID.Pattern) != 0)   // 아직 종료 신호(0) 아님
            return NodeState.Running;
        
        //공격 했으니까 다시 초기화 
        _attackStarted = false;
        print($"애니메이션 실행 : {picked}");
        return NodeState.Success;
    }

    #endregion

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
        GateOpen = true;
        // 장판 비주얼 끄고 싶으면 여기서: if (PatternPos) PatternPos.SetActive(false);
    }

    public NodeState ResetSpecialGate()
    {
        GateOpen = false;
        return NodeState.Success;
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



}