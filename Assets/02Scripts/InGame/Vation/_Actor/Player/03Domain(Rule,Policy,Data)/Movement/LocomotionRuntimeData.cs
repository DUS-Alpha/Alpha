using UnityEngine;

namespace alpha
{
    // RuntimeData
    public class LocomotionRuntimeData
    {
        // StateData
        public bool IsGrounded => m_isGrounded;
        private bool m_isGrounded;

        public float CurrentSpeed => m_currentSpeed;
        private float m_currentSpeed;

        // 지상확인
        public void SetGround(bool isGrounded)
        {
            m_isGrounded = isGrounded;
        }
    }
}