using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    public Vector3 RootMotionPos { get; private set; }
    public Quaternion RootMotionRot { get; private set; }
    public bool IsRootMotion { get; private set; }
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        events.SwapWeaponAction += SwapWeaponAni;
    }

    #region ================================================================================ Locomotion
    /// <summary>
    /// Locomotion의 CurrentMoveSpeed 받아오기
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void SetGroundMoveAni(float moveSpeed)
    {
        m_animator.SetFloat("MoveSpeed", moveSpeed);
    }
    public void SetIsGroundAni(bool isGrounded)
    {
        m_animator.SetBool("IsGround", isGrounded);
    }
    public void SetJumpAni(bool isJump)
    {
        m_animator.SetBool("IsJump", isJump);
    }

    public void SetIsFlyAni(bool isFlying, bool isFlyUpStart)
    {
        m_animator.SetBool("IsFlying", isFlying);
        m_animator.SetBool("IsFlyUp", isFlyUpStart);
    }
    
    public void SetFlyMoveAni(float inputX, float inputY)
    {
        m_animator.SetFloat("InputX", inputX);
        m_animator.SetFloat("InputY", inputY);
    }
    #endregion ================================================================================ /Locomotion

    #region ================================================================================ Combat
    public void SwapWeaponAni(int weaponNum)
    {
        m_animator.SetTrigger("SwapWeapon");
        
        if(weaponNum == 1)
        {
            m_animator.SetBool("IsMelee", true);
            m_animator.SetBool("IsRange", false);
        }
        else
        {
            m_animator.SetBool("IsRange", true);
            m_animator.SetBool("IsMelee", false);
        }
        m_animator.SetInteger("WeaponNum", weaponNum);
    }

    // TODO : 포지션값 받아오는 함수 따로 만들지 고민
    public void AttackAni(bool isAttack)
    {
        IsRootMotion = isAttack;
        m_animator.applyRootMotion = isAttack;
        m_animator.SetBool("IsAttack", isAttack);
    }
    #endregion ================================================================================ /Combat

    public void UpdateAnimatorTransformValue()
    {
        if(m_animator.applyRootMotion)
        {
            RootMotionPos = m_animator.deltaPosition;
            RootMotionRot = m_animator.deltaRotation;
        }
    }
}
