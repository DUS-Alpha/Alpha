using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class FlyFireball : MonoBehaviour
{
    [Header("디버깅용")]
    [SerializeField] private bool isPattern = false;

    [Header("스플라인 관련")]
    [SerializeField] private SplineContainer container;
    [SerializeField] private SplineAnimate splineAnimate;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;    
    [SerializeField] private float rotateSpeed = 10f; 
    [SerializeField] private float arriveDistance = 0.1f;

    [Header("화염구 설정")]
    [SerializeField] private GameObject fireballPrefab; // 🔥 소환할 화염구 프리팹
    [SerializeField] private float spawnInterval = 1f;  // 🔥 1초마다 소환

    private Vector3 patternStartPos;
    [SerializeField]private Vector3 originalPos;
    private Coroutine fireballRoutine;
    
    private bool splineCompleted = false;
    
    public Transform TargetPos;

  

    void Awake()
    {
        
        patternStartPos = container.EvaluatePosition(0f);
        splineAnimate.PlayOnAwake = false;
        splineAnimate.Restart(false); 
        transform.position = originalPos;
        
    }

    
    public NodeState MoveToSplineStart()
    {
        print("BT시작");
        Vector3 direction = (patternStartPos - transform.position).normalized;
    
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }
    
        transform.position = Vector3.MoveTowards(transform.position, patternStartPos, moveSpeed * Time.deltaTime);
    
        if (Vector3.Distance(transform.position, patternStartPos) < arriveDistance)
        {
            if (!splineAnimate.IsPlaying)
            {
                splineAnimate.Play();

                // Completed 이벤트 등록
                splineAnimate.Completed += OnSplineComplete;

                 StartCoroutine(SpawnFireballs());
            }
        }
        
        // 스플라인 완료 여부에 따라 NodeState 반환
        if (splineCompleted)
        {
            splineCompleted = false; // 다음 실행을 위해 초기화
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    // private void MoveToSplineStart()
    // {
    //     Vector3 direction = (patternStartPos - transform.position).normalized;
    //
    //     if (direction != Vector3.zero)
    //     {
    //         Quaternion targetRot = Quaternion.LookRotation(direction);
    //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
    //     }
    //
    //     transform.position = Vector3.MoveTowards(transform.position, patternStartPos, moveSpeed * Time.deltaTime);
    //
    //     if (Vector3.Distance(transform.position, patternStartPos) < arriveDistance)
    //     {
    //         splineAnimate.Play();
    //         isPattern = false;
    //
    //         // 🔥 화염구 생성 코루틴 시작
    //         if (fireballRoutine == null)
    //             fireballRoutine = StartCoroutine(SpawnFireballs());
    //     }
    // }
    
    private void OnSplineComplete()
    {
        Debug.Log("스플라인 완료!");

        // 화염구 코루틴 종료
      
        StopAllCoroutines();
        fireballRoutine = null;
        

        splineCompleted = true;
        splineAnimate.Restart(true);
        // 이벤트 중복 호출 방지
        splineAnimate.Completed -= OnSplineComplete;
    }


    private IEnumerator SpawnFireballs()
    {
        while (splineAnimate.IsPlaying)
        {
            
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y+20, transform.position.z);
            
            
            // 화염구의 위치와 플레이어의 위치 거리를 정상화 
            Vector3 dir = (TargetPos.position - newPos).normalized;
            
            
            // 현재 위치에서 화염구 생성
            var fireball = PoolManager.Instance.Spawn(fireballPrefab,newPos,transform.rotation);
            fireball.GetComponent<PooledProjectile>().Launch(dir, 20f,false);
            
            yield return new WaitForSeconds(spawnInterval);
        }

        // 스플라인 이동 끝나면 자동 종료
        fireballRoutine = null;
    }
}
