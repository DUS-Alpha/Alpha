using UnityEngine;

public class FlyTowardTarget : MonoBehaviour
{
    private float MoveTimer = 2f;
    private float currentTimer = 0;
    private Animator ani;
    private bool isStarted =false;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public NodeState Execute(Blackboard bb,FlySettings settings)
    {
        if (bb.Target == null || settings.hoverCenter == null)
            return NodeState.Failure;

        // 1. 위치 추적
        Vector3 target = new Vector3(bb.Target.position.x, bb.Target.position.y + settings.hoverHeight, bb.Target.position.z);
        Vector3 direction = target - transform.position;
        Vector3 MyTransform = new Vector3(transform.position.x, bb.Target.position.y + settings.hoverHeight, transform.position.z);

        // Y축 평면 회전
        Vector3 flatDirection = direction;
        flatDirection.y = 0f;
        if (flatDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * settings.turnSpeed);
        }

        // 이동
        transform.position = Vector3.MoveTowards(MyTransform, bb.Target.position, Time.deltaTime * settings.moveSpeed);
        
        return NodeState.Success; 
    }

    public NodeState LookAtAndWalk(Blackboard bb, FlySettings settings)
    {
        currentTimer += Time.deltaTime; 
        if (currentTimer > MoveTimer)
        {
            currentTimer = 0f; // 다음번을 위해 초기화
            isStarted = false;
            return NodeState.Success;
        }

        if (bb == null || bb.Target == null)
            return NodeState.Failure;
        
        if (!isStarted)
        {
            ani.SetTrigger("Walk");
            isStarted = true;
        }
        
        Vector3 direction = bb.Target.position - transform.position;
        direction.y = 0f; // 상하 회전 제외, 수평 방향만 바라봄

        // 2. 회전 보간
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * settings.turnSpeed
            );
        }
        
        // 3. 이동 (플레이어 쪽으로 다가가기)
        // 목표 위치 (플레이어 위치 + 일정 높이 유지)
        Vector3 targetPos = new Vector3(
            bb.Target.position.x,
            transform.position.y, // 높이는 그대로 유지
            bb.Target.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            Time.deltaTime * settings.moveSpeed
        );

        return NodeState.Running;
    }
    
    public NodeState Run(FlySettings settings)
    {
        currentTimer += Time.deltaTime; 
        if (currentTimer > MoveTimer)
        {
            currentTimer = 0f; // 다음번을 위해 초기화
            isStarted = false;
            ani.SetTrigger("Test");
            return NodeState.Success;
        }
        if (!isStarted)
        {
            ani.SetTrigger("Run");
            isStarted = true;
        }
        
        // 최적화된 부분: 스칼라를 미리 계산
        float moveDistance = settings.moveSpeed * Time.deltaTime;
        transform.position += transform.forward * moveDistance;
        
        return NodeState.Running;
    }


}
