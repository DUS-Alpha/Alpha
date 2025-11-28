using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class FlyFireball : MonoBehaviour
{
    [Header("Only Debuging")]
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
    
    private bool splineStarted = false; // 스플라인으로 이동해서  썼는지 확인하는법 

    void Awake()
    {
        
        patternStartPos = container.EvaluatePosition(0f);
        splineAnimate.PlayOnAwake = false;
        transform.position = originalPos;
        transform.rotation = Quaternion.identity;   
        
    }

    
    public NodeState MoveToSplineStart()
    {
        print("BT시작"+splineAnimate.IsPlaying);
        
        Vector3 direction = (patternStartPos - transform.position).normalized;
    
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }
    
        transform.position = Vector3.MoveTowards(transform.position, patternStartPos, moveSpeed * Time.deltaTime);
    
        if (Vector3.Distance(transform.position, patternStartPos) < arriveDistance)
        {
            if (!splineStarted)
            {
                splineStarted = true;

                splineAnimate.Play();
                //이벤트로 종료 추가
                splineAnimate.Completed += OnSplineComplete;

                fireballRoutine = StartCoroutine(SpawnFireballs());
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
    
    private void OnSplineComplete()
    {
        Debug.Log("스플라인 완료!");

        // 화염구 코루틴 종료
      
        if (fireballRoutine != null)
        {
            StopCoroutine(fireballRoutine);
        }
        
        fireballRoutine = null;
        
        

        splineCompleted = true;
        // 이벤트 중복 호출 방지
        splineAnimate.Completed -= OnSplineComplete;
    }


    private IEnumerator SpawnFireballs()
    {
        print("코루틴 실행중");
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
