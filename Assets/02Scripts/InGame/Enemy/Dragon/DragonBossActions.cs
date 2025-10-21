using System;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossActions : MonoBehaviour,IDamageable
{
    public Animator animator;
    public Transform hoverCenter;
    public float hoverHeight = 5f;
    public float moveSpeed = 5f;
    public float turnSpeed = 3f;
    
    
    private Blackboard BB;
    [SerializeField]private bool _attackStarted;
    
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform[] firePoints; // 여러 발사구 지원
   
    [SerializeField] private GameObject BreathsPrefab;
    [SerializeField] private Transform BreathsPos;
    
    [SerializeField] private float fireInterval = 3f;
    [SerializeField] private float fireSpeed = 30f;
    
    [SerializeField] private BossAudio bossAudio;
    
    
    //테스트를 위한 체력 
    [SerializeField] private float hp = 100;
    //투사체 데미지 테스트
    private int damage = 10;
    [SerializeField]private bool deathAnimPlayed = false;
    
    [SerializeField] private float hitAnimCooldown = 5f; // 피격 모션 쿨타임
    private float lastHitAnimTime = -999f;               // 마지막 피격 모션 시간
    private bool wasHit = false;                         // 직전에 데미지 받은 여부
    
    [SerializeField] public bool useBreath = false; // 브레스 패턴 플래그
    [SerializeField] private float breathCooldown = 5f;  // 브레스 쿨타임
    private float lastBreathTime = 5f;                 // 마지막 브레스 사용 시각
    
    [SerializeField] private LayerMask groundLayer;
    private bool hasLanded = false;

    //죽었는지 확인하는 방법
    [SerializeField]private bool isDead = false;
    
    private float wingSoundTimer = 0f;
    [SerializeField] private float wingSoundInterval = 2f;

    private int _firePointIndex = 0;
    private float _fireTimer;
    
    private Rigidbody rb;

    public void SetBlackboard(Blackboard bb)
    {
        BB = bb;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Transform GetNextFirePoint()
    {
        if (firePoints == null || firePoints.Length == 0)
            return transform; // 기본값: 자기 자신


        Transform point = firePoints[_firePointIndex];
        _firePointIndex = (_firePointIndex + 1) % firePoints.Length;
        return point;
    }
    
    public NodeState AscendToHoverHeight()
    {
        if (hoverCenter == null) return NodeState.Failure;

        float targetY = hoverCenter.position.y + hoverHeight;
        Vector3 current = transform.position;
        Vector3 target = new Vector3(current.x, targetY, current.z);

        transform.position = Vector3.MoveTowards(current, target, Time.deltaTime * moveSpeed);

        if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
        {
            print("성공");
            return NodeState.Success;
        }
        
        return NodeState.Running;
    }
    
    public NodeState FlyTowardTargetAndFire()
    {
        if (BB?.Target == null || hoverCenter == null || fireballPrefab == null || firePoints.Length == 0)
            return NodeState.Failure;

        // 1. 위치 추적
        Vector3 target = new Vector3(BB.Target.position.x, BB.Target.position.y + hoverHeight, BB.Target.position.z);
        Vector3 direction = target - transform.position;
        Vector3 MyTransform = new Vector3(transform.position.x, BB.Target.position.y + hoverHeight, transform.position.z);
        Vector3 flatDirection = direction;
        flatDirection.y = 0f;
        if (flatDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        }

        transform.position = Vector3.MoveTowards(MyTransform, BB.Target.position, Time.deltaTime * moveSpeed);

        // 2. 타이머 체크 후 투사체 발사
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireInterval)
        {
            _fireTimer = 0f;

            Transform point = GetNextFirePoint();
            // 풀에서 꺼내오기
            Vector3 dir = (BB.Target.position - point.position).normalized;
            GameObject fireball = PoolManager.Instance.Spawn(fireballPrefab, point.position, Quaternion.LookRotation(dir));

            var pp = fireball.GetComponent<PooledProjectile>();
            if (pp != null)
            {
                pp.Launch(dir, fireSpeed, false);
            }

            Debug.Log("🔥 Fireball launched!");
        }

        return NodeState.Success;
    }
    
   
    
    public NodeState CheckHitReaction()
    {
        if (wasHit) // 피격 받은 경우만 체크
        {
            // 쿨타임 체크
            if (Time.time - lastHitAnimTime >= hitAnimCooldown)
            {
                animator.SetTrigger("Hit");   // 피격 모션 발동
                lastHitAnimTime = Time.time;
                wasHit = false; // 초기화
                return NodeState.Success;     // 피격 애니메이션 발동 성공
            }
            else
            {
                wasHit = false; // 데미지는 들어가지만 모션은 쿨타임 때문에 건너뜀
                return NodeState.Failure;
            }
        }

        return NodeState.Failure; // 맞지 않았음
    }

    
    
    
    public NodeState FireballAttack()
    {
        if (BB?.Target == null || fireballPrefab == null || firePoints.Length == 0)
            return NodeState.Failure;

        _fireTimer += Time.deltaTime;

        if (_fireTimer >= fireInterval)
        {
            _fireTimer = 0f;

            
            Transform point = GetNextFirePoint();
            // 풀에서 꺼내오기
            Vector3 dir = (BB.Target.position - point.position).normalized;
            GameObject fireball = PoolManager.Instance.Spawn(fireballPrefab, point.position, Quaternion.LookRotation(dir));
            
            fireball.GetComponent<PooledProjectile>().Launch(dir, fireSpeed, false);
            
            bossAudio.Play("Projectile_Explosion");
            Debug.Log("🔥 Fireball launched!");
        }
        
        return NodeState.Success;
    }


    public NodeState FlyTowardTarget()
    {
        if (BB?.Target == null || hoverCenter == null)
            return NodeState.Failure;

        // 1. 위치 추적
        Vector3 target = new Vector3(BB.Target.position.x, BB.Target.position.y + hoverHeight, BB.Target.position.z);
        Vector3 direction = target - transform.position;
        Vector3 MyTransform = new Vector3(transform.position.x, BB.Target.position.y + hoverHeight, transform.position.z);

        // Y축 평면 회전
        Vector3 flatDirection = direction;
        flatDirection.y = 0f;
        if (flatDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        }

        // 이동
        transform.position = Vector3.MoveTowards(MyTransform, BB.Target.position, Time.deltaTime * moveSpeed);
        
        wingSoundTimer += Time.deltaTime;
        return NodeState.Success; 
    }
    
    
    public NodeState HoverIdle()
    {
        if (hoverCenter == null) return NodeState.Failure;

        float sin = Mathf.Sin(Time.time * 2f) * 0.5f;
        Vector3 target = new Vector3(hoverCenter.position.x, hoverCenter.position.y + hoverHeight + sin, hoverCenter.position.z);

        if (BB?.Target != null)
        {
            Vector3 lookDir = BB.Target.position - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
            }
        }

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 2f);

        return NodeState.Running;
    }
    
    public NodeState CheckBreath()
    {
        float elapsed = Time.time - lastBreathTime;
        bool canUse = elapsed >= breathCooldown;

        /*Debug.Log($"[CheckBreath] elapsed={elapsed:F2} / cooldown={breathCooldown} / useBreath={useBreath}");*/

        if (!useBreath && canUse)
        {
            useBreath = true;
            //Debug.Log("[CheckBreath] ✅ 브레스 발동 준비 완료");
            return NodeState.Success;
        }

        //Debug.Log("[CheckBreath] ❌ 실패 (쿨타임 중이거나 이미 발동됨)");
        return NodeState.Failure;
    }

    public void trueBreathParticles()
    {
        BreathsPrefab.SetActive(true);
    }
    public void falseBreathParticles()
    {
        BreathsPrefab.SetActive(false);
    }

    public NodeState BreatheFire()
    {
        Debug.Log($"[BreatheFire] useBreath={useBreath}, attackStarted={_attackStarted}");

        if (!useBreath) 
        {
            //Debug.Log("[BreatheFire] ❌ 발동 조건 미충족 (useBreath == false)");
            return NodeState.Failure;
        }

        if (!_attackStarted)
        {
            //Debug.Log("[BreatheFire] ▶ FireBreath 애니메이션 트리거 발동");
            animator.SetTrigger("FireBreath");
            _attackStarted = true;
            return NodeState.Running;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);
        /*Debug.Log($"[BreatheFire] 현재 상태: {state.fullPathHash}, normalizedTime={state.normalizedTime:F2}");*/

        if (state.IsName("FireBreath"))
        {
            if (state.normalizedTime >= 0.95f)
            {
                Debug.Log("[BreatheFire] ✅ 브레스 완료");
                _attackStarted = false;
                useBreath = false;
                lastBreathTime = Time.time;
                return NodeState.Success;
            }
            else
            {
                Debug.Log("[BreatheFire] ⏳ 브레스 애니메이션 진행 중");
            }
        }
        else
        {
            Debug.LogWarning("[BreatheFire] ⚠ FireBreath 상태가 아님 — Animator 상태 이름 확인 필요");
        }

        return NodeState.Running;
    }

   

    public NodeState Fall()
    {
        // 아직 착지하지 않았다면 낙하 활성화
        if (!hasLanded)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Raycast로 바닥 체크 (콜리전 대신)
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f, groundLayer))
            {
                Land();
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        // 이미 착지한 상태라면 성공 처리
        return NodeState.Success;
    }
    
    private void Land()
    {
        hasLanded = true;
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

    public NodeState Die()
    {
        return NodeState.Success;
    }


    public NodeState CheckDeath()
    {
        if (isDead && !deathAnimPlayed)
        {
            // 죽었을 때는 Die가 아니라 Fall부터 실행
            animator.SetTrigger("Fall");
            deathAnimPlayed = true;
            bossAudio.Play("Death");
            return NodeState.Success; // 성공하면 트리에서 Fall 노드로 넘어감
        }
        return NodeState.Failure;
    }

    
    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (isDead) return; // 이미 죽었으면 무시
        
        //데미지 밀기 
        hp -= damageMassage.Damage;
        // 0 밑으로 안내려가게 Clamp
        hp = Mathf.Max(hp, 0);
        print($"{damageMassage.Damager}가 {gameObject.name}에게 {damageMassage.Damage}의 데미지 \n 현재 HP : {hp}");
        
        // 단순히 피격 여부만 표시
        wasHit = true;
        
        if (hp <= 0)
        {
            isDead = true;
            Debug.Log($"{gameObject.name} 사망 처리");
        }
    }

   
} 
