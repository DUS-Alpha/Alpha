
using UnityEngine;



public class AttackCycle : MonoBehaviour
{
    private Animator ani;
    private bool _started = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0f;
    public bool isTakeoff = false;
    public GameObject BreathPos;

    private float currentSpeed = 0f;
    public float startSpeed = 5f;       // 초기 속도
    public float acceleration = 25f;    // 시간에 따라 붙는 가속도 (값은 원하는 감각에 맞게 조절)
    
    private bool canRotateDuringAttack = false; // 회전을 위한  함수

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }


    public NodeState MeleAttackTest()
    {
        ani.SetTrigger("Melee");
        return NodeState.Success;
    }
  


    public NodeState MeleeAttack(Blackboard bb)
    {
        if (bb == null || bb.Target == null)
            return NodeState.Failure;

        // 1️⃣ 공격 시작 시점
        if (!isAttacking)
        {
            ani.SetTrigger("Melee");
            attackDuration = ani.GetCurrentAnimatorStateInfo(0).length; // 애니메이션 길이
            attackTimer = 0f;
            isAttacking = true;
            canRotateDuringAttack = true;
            Debug.Log("근접 공격 시작");
            return NodeState.Running;
        }

        // 2️⃣ 공격 중 (플레이어 바라보기)
        attackTimer += Time.deltaTime;

        if (canRotateDuringAttack)
        {
            Vector3 direction = bb.Target.position - transform.position;
            direction.y = 0f; // 수평 회전만

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * 4f * 2f // 공격 중엔 빠른 회전
                );
            }
        }

        // 3️⃣ 공격 종료 시점
        if (attackTimer >= attackDuration)
        {
            isAttacking = false;
            Debug.Log("근접 공격 종료");
            return NodeState.Success;
        }

        return NodeState.Running;
    }
    
    public NodeState BiteAttack(Blackboard bb)
    {
        if (bb == null || bb.Target == null)
            return NodeState.Failure;

        // 1️⃣ 공격 시작 시점
        if (!isAttacking)
        {
            ani.SetTrigger("Bite");
            attackDuration = ani.GetCurrentAnimatorStateInfo(0).length; // 애니메이션 길이
            attackTimer = 0f;
            isAttacking = true;
            canRotateDuringAttack = true;
            Debug.Log("근접 공격 시작");
            return NodeState.Running;
        }

        // 2️⃣ 공격 중 (플레이어 바라보기)
        attackTimer += Time.deltaTime;

        if (canRotateDuringAttack)
        {
            Vector3 direction = bb.Target.position - transform.position;
            direction.y = 0f; // 수평 회전만

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * 4f * 2f // 공격 중엔 빠른 회전
                );
            }
        }

        // 3️⃣ 공격 종료 시점
        if (attackTimer >= attackDuration)
        {
            isAttacking = false;
            Debug.Log("근접 공격 종료");
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    // public NodeState BodyAttack(Blackboard BB)
    // {
    //    // 1.날으는 애니메이션 실행 (한번만)
    //    // 4.낙하 후 다시 일어나는 애니메이션을 진행후 패턴을 종료 
    //    if (!isAttacking)
    //    {
    //        ani.SetTrigger("AirRun");
    //        isAttacking = true;
    //    }
    //
    //    // 2.날면서 플레이어의 위치 z와 x가 동기화 될때까지 이동 
    //    Vector3 targetXZ = new Vector3(BB.Target.position.x, BB.OwnerTransform.position.y, BB.Target.position.z);
    //    BB.OwnerTransform.position = Vector3.MoveTowards(BB.OwnerTransform.position, targetXZ, 5 * Time.deltaTime);
    //   
    //    Vector3 direction = BB.Target.position - transform.position;
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            targetRotation,
    //            Time.deltaTime * 5f
    //        );
    //    }
    //    
    //    // 3.동기화 되면 낙하 애니메이션 실행 
    //    if (Vector3.Distance(BB.OwnerTransform.position, targetXZ) < 0.2f)
    //    {
    //        print("차이 얼마안남");
    //        ani.SetTrigger("Trig");
    //        isAttacking = false;
    //        return NodeState.Success;
    //    }
    //
    //
    //    return NodeState.Running;
    // }
    
    
    public NodeState BodyAttack(Blackboard BB)
    {
        if (!isAttacking)
        {
            ani.SetTrigger("Test");
            isAttacking = true;
            currentSpeed = startSpeed; // 공격 시작 시 속도 초기화
        }

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);

        if (isTakeoff)
        {
            BreathPos.SetActive(true);

            // ✅ 가속도 적용
            currentSpeed += acceleration * Time.deltaTime;

            transform.position = Vector3.MoveTowards(
                transform.position,
                BB.Target.position,
                currentSpeed * Time.deltaTime
            );
        }

        // 타겟과 매우 가까워지면 낙하 애니메이션 실행
        if (Vector3.Distance(transform.position, BB.Target.position) < 0.2f)
        {
            ani.ResetTrigger("Test");
            ani.SetTrigger("Trig");
            isAttacking = false;
            isTakeoff = false;
            BreathPos.SetActive(false);
        }

        // 낙하 애니메이션 완료 시 Success 반환
        if (stateInfo.IsName("EditLanding"))
        {
            if (stateInfo.normalizedTime >= 0.95f)
            {
                return NodeState.Success;
            }
        }

        return NodeState.Running;
    }


    public void StopAttackRotation()
    {
        canRotateDuringAttack = false;
        Debug.Log("회전 종료 이벤트 호출됨");
    }

}
