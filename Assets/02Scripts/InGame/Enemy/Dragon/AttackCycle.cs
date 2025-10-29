
using UnityEngine;



public class AttackCycle : MonoBehaviour
{
    private Animator ani;
    private bool _started = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0f;


    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public NodeState FireballAttack(Blackboard BB, AttackSetting attackCycle)
    {
        if (BB?.Target == null || attackCycle.fireballPrefab == null || attackCycle.firePoints.Length == 0)
            return NodeState.Failure;

        // 총 발사 횟수 초기화: AttackSetting에 _started 변수 추가 가능
        if (!_started)
        {
            attackCycle.totalFire = attackCycle.maxFire;
            attackCycle._fireTimer = 0f;
            _started = true;
            Debug.Log("[FireballAttack] 발사 시작");
        }

        // 타이머 누적
        attackCycle._fireTimer += Time.deltaTime;

        // Interval 체크
        while (attackCycle._fireTimer >= attackCycle.fireInterval && attackCycle.totalFire > 0)
        {
            attackCycle._fireTimer -= attackCycle.fireInterval;

            Transform point = GetNextFirePoint(attackCycle);

            Vector3 firePoint = RandomPosTarget(BB.Target.position,2,7);
            
            
            Vector3 randomOffset = Random.insideUnitSphere; // -1 ~ 1 범위
            
            // 2. 원하는 반경 적용
            float radius = Random.Range(5, 10);
            randomOffset *= radius; 
            Vector3 ShootPoint =  new Vector3(randomOffset.x +BB.Target.position.x , BB.Target.position.y,randomOffset.z + BB.Target.position.z);
            
            Vector3 dir = (ShootPoint - point.position).normalized;

            GameObject fireball = PoolManager.Instance.Spawn(attackCycle.fireballPrefab, point.position, Quaternion.LookRotation(dir));
            fireball.GetComponent<PooledProjectile>().Launch(dir, attackCycle.fireSpeed, false);

            attackCycle.totalFire--;
            Debug.Log($"🔥 Fireball launched! 남은 발사 횟수: {attackCycle.totalFire}");
        }

        // 남은 발사 횟수 확인
        if (attackCycle.totalFire > 0)
        {
            return NodeState.Running;
        }

        // 발사 끝
        _started = false;
        Debug.Log("[FireballAttack] 발사 완료");
        return NodeState.Success;
    }

    Vector3  RandomPosTarget(Vector3 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, 360f); // 0~360도 사이의 임의의 각도 선택 (원을 만들기 위한 기준)
        float radians = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환 (삼각함수에서 사용하기 위함)
        float radius = Random.Range(minRadius, maxRadius); // 원의 중심으로부터의 거리(반지름)를 랜덤으로 선택
        
        // 각도와 반지름을 이용해 XZ 평면에서의 좌표 오프셋 계산
        float offsetX = Mathf.Cos(radians) * radius; // 라디안 각도를 이용해 X축 방향의 거리 계산
        float offsetZ = Mathf.Sin(radians) * radius; // 라디안 각도를 이용해 Z축 방향의 거리 계산

        // 타겟의 높이는 유지 (Y좌표 그대로)
        Vector3 randomPos = new Vector3(center.x + offsetX, center.y, center.z + offsetZ);

        return randomPos;
    }

    private Transform GetNextFirePoint(AttackSetting attackCycle)
    {
        if (attackCycle.firePoints == null || attackCycle.firePoints.Length == 0)
            return transform; // 기본값: 자기 자신


        Transform point = attackCycle.firePoints[attackCycle._firePointIndex];
        attackCycle._firePointIndex = (attackCycle._firePointIndex + 1) % attackCycle.firePoints.Length;
        return point;
    }

    public NodeState MeleeAttack()
    {
        if (!isAttacking)
        {
            ani.SetTrigger("Melee");
            attackDuration = ani.GetCurrentAnimatorStateInfo(0).length; // 애니메이션 길이
            attackTimer = 0f;
            isAttacking = true;
            Debug.Log("근접 공격 시작");
            return NodeState.Running;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)
        {
            isAttacking = false;
            Debug.Log("근접 공격 종료");
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public NodeState BodyAttack(Blackboard BB)
    {
       // 1.날으는 애니메이션 실행 (한번만)
       // 4.낙하 후 다시 일어나는 애니메이션을 진행후 패턴을 종료 
       if (!isAttacking)
       {
           ani.SetTrigger("AirRun");
           isAttacking = true;
       }

       // 2.날면서 플레이어의 위치 z와 x가 동기화 될때까지 이동 
       Vector3 targetXZ = new Vector3(BB.Target.position.x, BB.OwnerTransform.position.y, BB.Target.position.z);
       BB.OwnerTransform.position = Vector3.MoveTowards(BB.OwnerTransform.position, targetXZ, 5 * Time.deltaTime);
      
       Vector3 direction = BB.Target.position - transform.position;
       if (direction != Vector3.zero)
       {
           Quaternion targetRotation = Quaternion.LookRotation(direction);
           transform.rotation = Quaternion.Slerp(
               transform.rotation,
               targetRotation,
               Time.deltaTime * 5f
           );
       }
       
       // 3.동기화 되면 낙하 애니메이션 실행 
       if (Vector3.Distance(BB.OwnerTransform.position, targetXZ) < 0.2f)
       {
           print("차이 얼마안남");
           ani.SetTrigger("Trig");
           isAttacking = false;
           return NodeState.Success;
       }


       return NodeState.Running;
    }
}
