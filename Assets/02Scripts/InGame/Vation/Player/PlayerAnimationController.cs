using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Locomotion의 CurrentMoveSpeed 받아오기
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void SetGroundMoveAni(float moveSpeed)
    {
        m_animator.SetFloat("MoveSpeed", moveSpeed);
    }
    public void SetIsGround(bool isGrounded)
    {
        m_animator.SetBool("IsGround", isGrounded);
    }
    public void SetJumpAni(bool isJump)
    {
        m_animator.SetBool("IsJump", isJump);
    }
    public void SetIsFly(bool isFly, bool isFlyUp)
    {
        m_animator.SetBool("IsFly", isFly);
        m_animator.SetBool("IsFlyUp", isFlyUp);
    }
    public void SetFlyMove(float inputX, float inputY)
    {
        m_animator.SetFloat("InputX", inputX);
        m_animator.SetFloat("InputY", inputY);
    }
}
