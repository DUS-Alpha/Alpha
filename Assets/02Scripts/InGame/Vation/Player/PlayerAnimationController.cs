using UnityEngine;


// 현재 애니메이터 레이는 전신을 사용하는 Base와 병렬로 사용되는 UpperBody레이어로 구성
// 전신에는 Combat 지상공격
// 그 외 원거리 공격 및 Fly하면서의 Melee 공격들은 UpperBody 병행처리
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    public Vector3 RootMotionPos { get; private set; }
    public Quaternion RootMotionRot { get; private set; }
    public bool IsRootMotion => m_animator.applyRootMotion;
    public bool IsPlayAni {  get; private set; }
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        //events.SwapWeaponAction += SwapWeaponAni;
    }

    #region ================================================================================ Locomotion
    /// <summary>
    /// Locomotion의 CurrentMoveSpeed 받아오기
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void MoveAni(float moveSpeed)
    {
        m_animator.SetFloat("MoveSpeed", moveSpeed);
    }
    public void SetIsGroundAni(bool isGrounded)
    {
        m_animator.SetBool("IsGround", isGrounded);
    }

    public void JumpAni()
    {
        m_animator.SetTrigger("Jump");
    }
    public void FlyAni(bool isFlying, bool isFlyUpStart)
    {
        m_animator.SetBool("IsFlying", isFlying);
        m_animator.SetBool("IsFlyUp", isFlyUpStart);
    }
    /// <summary>
    /// Fly와 Aim일때 사용됨
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    public void DirMoveAni(float inputX, float inputY)
    {
        // 값이 바로 전환되는 것을 부드럽게 변환
        float dampTime = 0.1f;

        m_animator.SetFloat("InputX", inputX, dampTime, Time.deltaTime);
        m_animator.SetFloat("InputY", inputY, dampTime, Time.deltaTime);
    }
    #endregion ================================================================================ /Locomotion


    #region ================================================================================ CombatFlags
    public void SetAnimatorWeight(float value)
    {

    }
    public void AimAni(bool isAim)
    {
        m_animator.SetBool("IsAim",isAim);
    }
    public void SwapWeaponAni(int currentNum)
    {
        m_animator.SetInteger("WeaponNum", currentNum);
        m_animator.SetTrigger("SwapWeapon");
    }
    public void MeleeAttackAni(bool isAttack)
    {
        m_animator.SetBool("IsMeleeAttack", isAttack);
    }
    public void RangeShootingAni()
    {
        m_animator.SetTrigger("RangeShooting");
    }
    public void SkillAni()
    {

    }
    public void ReloadAni()
    {
        m_animator.SetTrigger("Reload");
    }

    public void SetApplyRootMotion(bool isRoot)
    {
        m_animator.applyRootMotion = isRoot;
    }
    /// <summary>
    /// 현재 상태가 Combo 태그를 가진 애니메이션인지 체크
    /// Input의 IsAttack이 false가 되더라도 해당 애니메이션이 끝나야 상태가 변환이 되도록하기 위한 체크
    /// </summary>
    /// <returns></returns>
    public bool CheckComboAnimation()
    {
        return m_animator.GetCurrentAnimatorStateInfo(3).IsTag("Combo");
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
