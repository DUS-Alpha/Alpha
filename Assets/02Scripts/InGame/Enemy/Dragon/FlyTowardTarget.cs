using UnityEngine;

public class FlyTowardTarget : MonoBehaviour
{
    private float MoveTimer = 1f;
    private float currentTimer = 0;
    private Animator ani;
    private bool isStarted =false;
    
    public bool isDone = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    

    public NodeState LookAtAndWalk(Blackboard bb, MoveSetting settings)
    {
        currentTimer += Time.deltaTime; 
        if (currentTimer > MoveTimer )
        {
            currentTimer = 0f; // 다음번을 위해 초기화
            isStarted = false;
            return NodeState.Success;
        }
        
        float distance = Vector3.Distance(transform.position, bb.Target.position);
        if (distance <= 15f) // 예: 0.5f 정도
        {
            currentTimer = 0f;
            isStarted = false;
            print("가까워요 ");
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
        Vector3 targetPos = new Vector3(
            bb.Target.position.x,
            transform.position.y, 
            bb.Target.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            Time.deltaTime * settings.moveSpeed
        );

        return NodeState.Running;
    }
    
    public NodeState Run(MoveSetting settings)
    {
        if (!isStarted)
        {
            ani.SetTrigger("Run");
            isStarted = true;
        }

        if (isDone)
        {
            isDone = false;
            return NodeState.Success;
        }
        
        // 최적화된 부분: 스칼라를 미리 계산
        float moveDistance = settings.moveSpeed * Time.deltaTime;
        transform.position += transform.forward *moveDistance ;
        
        return NodeState.Running;
    }


}
