using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public enum DistanceCheckType
{
    Far,    // > n2
    Mid,    // n < distance ≤ n2
    Close,  // ≤ n
    Unknown // 초기값/타겟 없음
}
[System.Serializable]
public class CheckDistanceSetting //  참조 타입 (class)
{
    public float _minRange;
    public float _maxRange;
    public DistanceCheckType _type;
}

[System.Serializable]
public class MoveSetting
{
    public float moveSpeed = 5f;   // 기본값 유지
    public float turnSpeed = 3f;   // 기본값 유지
}


[System.Serializable]
public class DeathSetting 
{
    public bool isDead = false;
}

[System.Serializable]
public class BreathSetting
{
    public GameObject breathPrefab;
}

public class DragonBossActions : MonoBehaviour,IDamageable
{
    public Animator animator;

    [Header("(Move Settings)")] 
    public MoveSetting currentMoveSetting; 
    
    [Header("(Death Settings)")]
    public DeathSetting currentDeathSettings; 
    
    [Header("(Breath Settings)")]
    public BreathSetting currentBreathsetting; 
   
    [Header("(CheckDistance Settings)")]
    public CheckDistanceSetting  checkDistanceSetting; 
    
    
     MoveCycle _moveCycle;
    
     DeathCycle _deathCycle;
     
     CheckRangeCycle _checkRangeCycle;
     
     FlyFireball _flyFireball;
     
    
    private Blackboard BB;
    
    [SerializeField] private BossAudio bossAudio;
    
    //테스트를 위한 체력 
    [SerializeField] private float hp = 1000;
    [SerializeField] private float hitAnimCooldown = 1f; // 피격 모션 쿨타임
    private float lastHitAnimTime = -999f;               // 마지막 피격 모션 시간
    [SerializeField]bool wasHit = false;
    
    [Header("(Animation Control)")]
    public bool IsRunning =false; //진행 중인지
    public bool IsComplete = false; //종료 됐는지
    
    [Header("Decal")]
    [SerializeField] GameObject decalPrefab;
    

    public void SetBlackboard(Blackboard bb)
    {
        BB = bb;
    }

    private void Awake()
    {
        _moveCycle = GetComponent<MoveCycle>();
        _deathCycle = GetComponent<DeathCycle>();
        _checkRangeCycle = GetComponent<CheckRangeCycle>();
        _flyFireball = GetComponent<FlyFireball>();
    }
    

    #region 포효 공격
    //코루틴으로 실행하기 위한 함수 Roar에서 데칼을 소환시키기위한 함수 
    //애니메이션 이벤트로 실행
     IEnumerator Boom()
     {
         print("실행중");
         int cnt = 0;
         
         
         while (cnt < 20)
         {
             // GameObject obj = INSTANCE.GetObject("TestOBj");
             GameObject obj = PoolManager.Instance.Spawn(decalPrefab,BB.OwnerTransform.position,Quaternion.identity);
             float x = Random.Range(-20f, 20f);
             float z = Random.Range(-20f, 20f);
             obj.transform.position = new Vector3(BB.OwnerTransform.position.x+x, 0, BB.OwnerTransform.position.z+z);
             cnt++;
             yield return new WaitForSeconds(0.05f);
         }
         
     }
     
     
     public NodeState Roar()
     {
         if (!IsRunning)
         {
             animator.SetTrigger("Roar");
             IsRunning = true;
             IsComplete = false;
         }

         if (!IsComplete)
             return NodeState.Running;

         // 애니메이션 종료 후
         IsRunning = false;
         return NodeState.Success;
     }
     #endregion
  


     #region 공중 공격실행
     
     public NodeState Flyfrieball()
     {
         return _flyFireball.MoveToSplineStart();
     }
     
     public NodeState Takeoff()
     {
         
         animator.SetTrigger("Takeoff");
         return NodeState.Success;
     }
     public NodeState Landing()
     {
         
         animator.SetTrigger("Landing");
         
         return NodeState.Success;
     }

     public bool IsLowHp()
     {
         return hp <= 500;
     }

     #endregion

 


    public NodeState Death()
    {
        currentBreathsetting.breathPrefab.SetActive(false);
        return _deathCycle.Death();
    }
    
     //이동
    public NodeState LookAtAndWalk()
    {
        return _moveCycle.LookAtAndWalk(BB,currentMoveSetting);
    }
    
    public NodeState MeleeAttack()
    {

        if (!IsRunning)
        {
            animator.SetTrigger("Melee");
            IsRunning = true;
            IsComplete = false;
        }

        if (!IsComplete)
            return NodeState.Running;

        // 애니메이션 종료 후
        IsRunning = false;
        return NodeState.Success;
    }
    
    public NodeState BiteAttack()
    {
        if (!IsRunning)
        {
            animator.SetTrigger("Bite");
            IsRunning = true;
            IsComplete = false;
        }

        if (!IsComplete)
            return NodeState.Running;

        // 애니메이션 종료 후
        IsRunning = false;
        return NodeState.Success;
    }
    
    public NodeState Run()
    {
        return _moveCycle.Run(currentMoveSetting);
    }
    
    public NodeState DoBreatheFire2()
    {
        if (!IsRunning)
        {
            animator.SetTrigger("Breath");
            IsRunning = true;
            IsComplete = false;
        }

        if (!IsComplete)
            return NodeState.Running;

        // 애니메이션 종료 후
        IsRunning = false;
        return NodeState.Success;
    }
    
    public NodeState CheckRangeCycle()
    {
        return _checkRangeCycle.CheckRange(BB, checkDistanceSetting);
    }
    
    
    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (currentDeathSettings.isDead) return; // 이미 죽었으면 무시
        
        //데미지 밀기 
        hp -= damageMassage.Damage;
        // 0 밑으로 안내려가게 Clamp
        hp = Mathf.Max(hp, 0);
        print($"{damageMassage.Damager}가 {gameObject.name}에게 {damageMassage.Damage}의 데미지 \n 현재 HP : {hp}");

        
        // 단순히 피격 여부만 표시
        wasHit = true;
        
        if (hp <= 0)
        {
            currentDeathSettings.isDead = true;
            Debug.Log($"{gameObject.name} 사망 처리");
        }
    }

   
} 
