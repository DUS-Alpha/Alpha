using System;
using System.Collections.Generic;
using UnityEngine;


public enum DistanceCheckType
{
    Far,    // > n2
    Mid,    // n < distance ≤ n2
    Close,  // ≤ n
    Unknown // 초기값/타겟 없음
}
[System.Serializable]
public class CheckDistanceSetting // ⬅️ 참조 타입 (class)
{
    public float _minRange;
    public float _maxRange;
    public DistanceCheckType _type;
}

[System.Serializable]
public class FlySettings // ⬅️ 참조 타입 (class)
{
    public float hoverHeight = 5f; // 기본값 유지
    public float moveSpeed = 5f;   // 기본값 유지
    public float turnSpeed = 3f;   // 기본값 유지
    public Transform hoverCenter;
}

[System.Serializable]
public class DeathSetting 
{
    public bool isDead = false;
    public bool deathAnimPlayed = false;
    public bool hasLanded = false; 
    public LayerMask groundLayer;
}

[System.Serializable]
public class AttackSetting
{
    public GameObject fireballPrefab;
    public Transform[] firePoints; // 여러 발사구 지원
    public float _fireTimer;
    public float fireInterval = 3f;
    public float fireSpeed = 30f;
    public int totalFire = 0; // 몇개 쏠 껀지
    public int maxFire = 5; // 초기 값
    
    public int _firePointIndex = 0;
}

[System.Serializable]
public class BreathSetting
{
    public bool useBreath = false; // 브레스 패턴 플래그
    public float breathCooldown = 5f; // 브레스 쿨타임
    public float lastBreathTime = 5f; // 마지막 브레스 사용 시각
    public bool _breathStarted;
    public GameObject breathPrefab;
}

public class DragonBossActions : MonoBehaviour,IDamageable
{
    public Animator animator;
    
    [Header("(Fly Settings)")]
     public FlySettings currentFlySettings = new FlySettings(); 

    [Header("(Death Settings)")]
    public DeathSetting currentDeathSettings = new DeathSetting(); 

    [Header("(Attack Settings)")] 
    public AttackSetting currentAttacksettings = new AttackSetting();
    
    [Header("(Breath Settings)")]
    public BreathSetting currentBreathsetting = new BreathSetting(); 
   
    [Header("(CheckDistance Settings)")]
    public CheckDistanceSetting  checkDistanceSetting= new CheckDistanceSetting(); 
    
    
     FlyTowardTarget _flyTowardCyle;
    
     DeathCycle _deathCycle;

     AttackCycle _attackCycle;
    
     BreathCycle _breathCycle;
    
     CheckRangeCycle _checkRangeCycle;
    
    
    private Blackboard BB;
    
    [SerializeField] private BossAudio bossAudio;
    
    
    //테스트를 위한 체력 
    [SerializeField] private float hp = 1000;
    [SerializeField]private bool deathAnimPlayed = false;
    [SerializeField] private float hitAnimCooldown = 1f; // 피격 모션 쿨타임
    private float lastHitAnimTime = -999f;               // 마지막 피격 모션 시간
    [SerializeField]private bool wasHit = false;                         // 직전에 데미지 받은 여부
    
    private Rigidbody rb;

    public void SetBlackboard(Blackboard bb)
    {
        BB = bb;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _flyTowardCyle = GetComponent<FlyTowardTarget>();
        _deathCycle = GetComponent<DeathCycle>();
        _attackCycle = GetComponent<AttackCycle>();
        _breathCycle = GetComponent<BreathCycle>();
        _checkRangeCycle = GetComponent<CheckRangeCycle>();
    }


    public NodeState Death()
    {
        currentBreathsetting.breathPrefab.SetActive(false);
        return _deathCycle.Death();
    }

    public NodeState FlyTowardTarget()
    {
        return _flyTowardCyle.Execute(BB,currentFlySettings);
    }
    public NodeState LookAtAndWalk()
    {
        return _flyTowardCyle.LookAtAndWalk(BB,currentFlySettings);
    }


    public NodeState CheckDeath()
    {
        return _deathCycle.CheckDeath(animator,currentDeathSettings);
    }

    public NodeState Fall()
    {
        return _deathCycle.Fall(animator,currentDeathSettings);
    }

    public NodeState AttackFireBall()
    {
        print("파이어볼 공격");
        return _attackCycle.FireballAttack(BB, currentAttacksettings);
    }

    public NodeState MeleeAttack()
    {

        return _attackCycle.MeleeAttack();
    }

    public NodeState DoCheckBreath()
    {
        return _breathCycle.CheckBreath(currentBreathsetting);
    }

    public NodeState Run()
    {
        return _flyTowardCyle.Run(currentFlySettings);
    }
    
    public NodeState DoBreatheFire()
    {
        return _breathCycle.BreatheFire(animator,currentBreathsetting);
    }
    
    public NodeState CheckRangeCycle()
    {
        return _checkRangeCycle.CheckRange(BB, checkDistanceSetting);
    }

    public NodeState BodyAttack()
    {
        return _attackCycle.BodyAttack(BB);
    }
    
    
    
  




    public NodeState CheckHitReaction()
    {
        print("체크리액션 진입");
        if (wasHit) // 피격 받은 경우만 체크
        {
            print("이프문 진입 ");
            // 쿨타임 체크
            if (Time.time - lastHitAnimTime >= hitAnimCooldown)
            {
                animator.SetTrigger("Hit");   // 피격 모션 발동
                lastHitAnimTime = Time.time;
                wasHit = false; // 초기화
                return NodeState.Success;     // 피격 애니메이션 발동 성공
            }
        }

        return NodeState.Failure; // 맞지 않았음
    }
    
 
    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (currentDeathSettings.isDead) return; // 이미 죽었으면 무시
        
        //데미지 밀기 
        hp -= damageMassage.damage;
        // 0 밑으로 안내려가게 Clamp
        hp = Mathf.Max(hp, 0);
        print($"{damageMassage.Damager}가 {gameObject.name}에게 {damageMassage.damage}의 데미지 \n 현재 HP : {hp}");
        
        // 단순히 피격 여부만 표시
        wasHit = true;
        
        if (hp <= 0)
        {
            currentDeathSettings.isDead = true;
            Debug.Log($"{gameObject.name} 사망 처리");
        }
    }

   
} 
