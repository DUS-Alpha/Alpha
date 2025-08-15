using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void GroundMoveAni(float moveSpeed)
    {
        m_animator.SetFloat("MoveSpeed", moveSpeed);
    }
}
