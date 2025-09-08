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
