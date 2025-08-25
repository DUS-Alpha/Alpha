using UnityEngine;

[RequireComponent(typeof(BossLocomotionExecutor))]
public class BossActions : MonoBehaviour
{
    public Blackboard BB { get; private set; }
    public void SetBlackboard(Blackboard bb) { BB = bb; }

    // === 디버그용 토글(키 입력으로 바뀜) ===
    [Header("Debug Toggles")]
    public bool dbgMagazineEmpty = false;
    public bool dbgUnderHeavyFire = false;

    [Header("Debug Ranges (for gizmo)")]
    public float ShootRange = 18f;
    public float OptimalRange = 14f;
    public float TooCloseRange = 6f;
    
    [Header("애니메이션 한번만 사용 할 수있게 하기위한 파라미터 ")]
    
    
    [Header("실제 사용될 파라미터")]
    [SerializeField] float searchBoundary = 2; // 찾는 범위 
    [SerializeField] float movementSpeedRatio = 2; //  움직이는 각도 
    [SerializeField] private float movementSpeed =2 ;// 테스트 용도의 이동 속도
    
    
    [Header("애니메이션 파라미터")]
    static readonly int HashWalk = Animator.StringToHash("Walk"); //bool
    bool _isWalking;

    private bool animFinish;
    [SerializeField]
    private bool switchingClip;
    
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        animFinish= true;
        switchingClip = false;
    }
    
    //애니메이션에 스크립트에 넣을 함수 
    public void OnAnimationFinished()
    {
        animFinish= true;
        switchingClip = false;
    }
    
    

    private void Update()
    {
        // 1: 탄약 없음 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dbgMagazineEmpty = !dbgMagazineEmpty;
            Debug.Log($"[DBG] MagazineEmpty = {dbgMagazineEmpty}");
        }
        // 2: 집중 사격 받는 중 토글
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dbgUnderHeavyFire = !dbgUnderHeavyFire;
            Debug.Log($"[DBG] UnderHeavyFire = {dbgUnderHeavyFire}");
        }
    }

    // === 액션 훅(내용은 나중에 채움) ===
    public NodeState Approach()
    {
        if (BB == null || BB.Target == null) return NodeState.Failure;
        
        
        if (Vector3.Magnitude(BB.Target.position - transform.position) < searchBoundary)
        {
            return NodeState.Success;
        }

        
        if (!switchingClip)
        {
            switchingClip = true;
            // ani.SetTrigger(HashWalk);
        }

        // Vector3 targetPos = BB.Target.position;
        // targetPos.y = transform.position.y;
        // movementSpeedRatio = Mathf.Lerp(movementSpeedRatio, 1, movementSpeed * Time.deltaTime);
        // transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x,targetPos.y,targetPos.z), Time.deltaTime * movementSpeed);
        transform.LookAt(BB.Target.transform);
        
        // 걷기 상태 ‘유지’
        return NodeState.Running;
    }

    public NodeState Kick()
    {
        if (BB == null || BB.Target == null) return NodeState.Failure;
        
        if (!switchingClip)
        {
            switchingClip = true;
            ani.SetTrigger("Kick");
        }
      
        
        return NodeState.Success;
    }

  


    //주위에  플레이어가 있는지 없는지 확인
    public NodeState IsNearbyPlayer()
    {
        if (Vector3.Magnitude(BB.Target.position - transform.position) < searchBoundary)
        {
            print("옆에 있어요 ~");
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

    public NodeState Aim()
    {
        print("조준중");
        return NodeState.Success;
    }

    public NodeState Fire()
    {
        print("");
        return NodeState.Success;
    }
    public NodeState Reload()    { return NodeState.Success; }
    public NodeState KeepDistance(){ return NodeState.Success; }

    // === 조건 훅(지금은 디버그 토글/간단 true) ===
    public bool HasTarget()        => BB != null && BB.Target != null;
    public bool HasLineOfSight()
    {
        if (BB == null || BB.Target == null) return false;
        var d = Vector3.Distance(BB.OwnerTransform.position, BB.Target.position);
        return d <= ShootRange; // 아주 단순한 시야/사거리 판정
    }
    public bool IsMagazineEmpty()  => dbgMagazineEmpty;
    public bool IsUnderHeavyFire() => dbgUnderHeavyFire;
}