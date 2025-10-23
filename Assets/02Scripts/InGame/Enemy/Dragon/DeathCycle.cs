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
        // 트리 정지
        var runner = GetComponent<BehaviorTreeRunner>();
        if (runner != null)
            runner.StopTree();
        return NodeState.Success;
    }

    public NodeState CheckDeath(Animator animator,DeathSetting deathSetting)
    {
        if (deathSetting.isDead && !deathSetting.deathAnimPlayed)
        {
            // 죽었을 때는 Die가 아니라 Fall부터 실행
            animator.SetTrigger("Fall");
            deathSetting.deathAnimPlayed = true;
            return NodeState.Success; // 성공하면 트리에서 Fall 노드로 넘어감
        }
        return NodeState.Failure;
    }
    
    public NodeState Fall(Animator animator,DeathSetting deathSetting)
    {
        // 이미 착지한 상태라면 성공 처리
        if (deathSetting.hasLanded)
        {
            return NodeState.Success;
        }

        // 아직 착지하지 않았다면 낙하 활성화
        rb.isKinematic = false;
        rb.useGravity = true;

        // 💡 새로운 바닥 체크 로직: SphereCast 사용 (더 안정적임)
        if (CheckGroundWithSphereCast(deathSetting))
        {
            Land(animator,deathSetting);
            print("바닥 착지 (SphereCast)"); // 출력 메시지 수정
            return NodeState.Success;
        }

        // 착지하지 않았다면 계속 낙하 중
        return NodeState.Running;
    }
    
    private bool CheckGroundWithSphereCast(DeathSetting deathSetting)
    {
        // ⚠️ 이 값들은 캐릭터 컨트롤러 또는 콜라이더 컴포넌트에서 가져와야 정확함
        // 여기서는 예시로 값 설정 (캐릭터 크기에 맞게 조정 필요)
        float characterRadius = 0.5f;  // 캐릭터 콜라이더의 반지름
        float checkDistance = 0.2f;    // 바닥과 아주 가까운 거리만 체크 (RaycastDistance)

        // Raycast의 시작점: 캐릭터의 중심이 아닌, 콜라이더의 밑 부분 바로 위
        Vector3 origin = transform.position;
    
        // 💡 디버그용 시각화
        Debug.DrawRay(origin, Vector3.down * (characterRadius + checkDistance), Color.yellow);

        // SphereCastAll을 사용하여 구체가 바닥에 닿는지 체크
        // (시작 위치, 반지름, 방향, 최대 거리, 레이어마스크)
        if (Physics.SphereCast(origin, characterRadius, Vector3.down, out RaycastHit hit, checkDistance, deathSetting.groundLayer))
        {
            // Debug.DrawLine(origin, hit.point, Color.green, 0.5f); // 착지 성공 시 디버그
            return true;
        }
    
        return false;
    }
    
    private void Land(Animator animator,DeathSetting deathSetting)
    {
        deathSetting.hasLanded = true;
        rb.isKinematic = true;
        rb.useGravity = false;

        // 애니메이션 재생
        animator.SetBool("Dodie", true);

        // 트리 정지
        var runner = GetComponent<BehaviorTreeRunner>();
        if (runner != null)
            runner.StopTree();

        Debug.Log("뒤졌음 ㅇㅇ");
    }

 
}
