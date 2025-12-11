using UnityEngine;

namespace alpha
{
    public class GroundAndGravityUitility
    {
        private bool m_isGrounded;
        private float m_lastGroundTime;
        private float m_groundDistance = 0.25f;

        public bool GetIsGround(CharacterController characterController, LayerMask groundMask)
        {
            // m_characterController.center 바닥에서 조금 띄어져있는 상태
            Vector3 worldCenter = characterController.transform.TransformPoint(characterController.center);
            float _height = characterController.height;

            Vector3 _colliderButtomtr = worldCenter - Vector3.up * (characterController.height * 0.5f - characterController.skinWidth);

            bool _groundCheck = Physics.CheckSphere(_colliderButtomtr, m_groundDistance, groundMask);

            if (_groundCheck)
            {
                m_lastGroundTime = Time.time;
            }

            m_isGrounded = (Time.time - m_lastGroundTime) <= 0.1f;
            //Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
            return m_isGrounded;
        }

        public void ApplyGravity()
        {

        }
    }
}