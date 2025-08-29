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

    public void SetIsFlyAni(bool isFly, bool isFlyUpStart)
    {
        m_animator.SetBool("IsFly", isFly);
        m_animator.SetBool("IsFlyUp", isFlyUpStart);
    }
    
    public void SetFlyMoveAni(float inputX, float inputY)
    {
        m_animator.SetFloat("InputX", inputX);
        m_animator.SetFloat("InputY", inputY);
    }
    #endregion ================================================================================ /Locomotion

    public void ChangeWeaponAni(bool isChange)
    {
        m_animator.SetTrigger("WeaponChange");
        m_animator.SetBool("IsWeaponChange", isChange);
    }
    public void SetWeaponIDAni(int weaponID)
    {
        m_animator.SetInteger("WeaponID", weaponID);
    }

    // TODO : 포지션값 받아오는 함수 따로 만들지 고민
    public void AttackAni(bool isAttack)
    {
        IsRootMotion = isAttack;
        m_animator.applyRootMotion = isAttack;
        m_animator.SetBool("IsAttack", isAttack);
    }

    public void UpdateAnimatorTransformValue()
    {
        if(m_animator.applyRootMotion)
        {
            RootMotionPos = m_animator.deltaPosition;
            RootMotionRot = m_animator.deltaRotation;
        }
    }
}
