using UnityEngine;


// 현재 애니메이터 레이는 전신을 사용하는 Base와 병렬로 사용되는 UpperBody레이어로 구성
// 전신에는 Combat 지상공격
// 그 외 원거리 공격 및 Fly하면서의 Melee 공격들은 UpperBody 병행처리
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    public Animator m_animator;
    public Vector3 RootMotionPos { get; private set; }
    public Quaternion RootMotionRot { get; private set; }
    public bool IsRootMotion => m_animator.applyRootMotion;
    public bool IsPlayAni {  get; private set; }
    private PlayerCombat m_combat;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    private void Start()
    {
        SetAnimatorWeight(1, 0);
        SetAnimatorWeight(2, 0);
        SetAnimatorWeight(3, 0);
    }

    public void InitializeModule(PlayerCombat combat)
    {
        m_combat = combat;
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        //events.SwapWeaponAction += SwapWeaponAni;
    }

    #region ================================================================================ Locomotion

    public void MoveAni(float moveSpeed)
    {
        m_animator.SetFloat("MoveSpeed", moveSpeed);
    }

    public void CombatMoveAni(float inputX, float inputY)
    {
        // 값이 바로 전환되는 것을 부드럽게 변환
        float dampTime = 0.1f;

        m_animator.SetFloat("InputX", inputX, dampTime, Time.deltaTime);
        m_animator.SetFloat("InputY", inputY, dampTime, Time.deltaTime);
    }
    public void SetIsGroundAni(bool isGrounded)
    {
        m_animator.SetBool("IsGround", isGrounded);
    }

    public void JumpAni()
    {
        m_animator.SetTrigger("Jump");
    }
    public void FlyUpAni()
    {
        m_animator.SetTrigger("FlyUp");
    }
    public void SetFlyingAni(bool isFlying)
    {
        m_animator.SetBool("IsFlying", isFlying);
    }


    public void HitAni()
    {
        m_animator.SetTrigger("Hit");
    }
    public void DieAni()
    {
        m_animator.SetTrigger("EnterDie");
    }
    #endregion ================================================================================ /Locomotion


    #region ================================================================================ CombatFlags
    public void SetIsCombatAni(bool isCombat)
    {
        m_animator.SetBool("IsCombat", isCombat);
    }
    public void SetAnimatorWeight(int index,float value)
    {
        m_animator.SetLayerWeight(index, value);
    }
    public void SwapWeaponAni(int currentNum)
    {
        m_animator.SetInteger("WeaponNum", currentNum);
        m_animator.SetTrigger("SwapWeapon");
    }
    public void AttackAni(bool isAttack, int currentNum)
    {
        m_animator.SetBool("IsAttack", isAttack);

        if(currentNum > 1 && isAttack) RangeShootingAni();
    }

    private void RangeShootingAni()
    {
        m_animator.SetTrigger("RangeShooting");
    }
    public void ReloadAni()
    {
        m_animator.SetTrigger("Reload");
    }
    public void SkillAni()
    {

    }
  
    public void UpdateAnimatorTransformValue()
    {
        if (m_animator.applyRootMotion)
        {
            RootMotionPos = m_animator.deltaPosition;
            RootMotionRot = m_animator.deltaRotation;
        }
    }
    #endregion ================================================================================ /Combat

}
