using System;
using UnityEngine;

namespace alpha
{
    [Serializable]
    public struct GravityConfig
    {
        public float BaseGravity;
        public float FlyingGravity;
        public float AntiGravity;
    }
    public class GroundAndGravityUitility
    {
        private GravityConfig m_gravityConfig;


        // ========== Ground
        public bool IsGround { get; private set; }
        private float m_lastGroundTime = 0;
        private float m_groundDistance = 0.25f;

        private CharacterController m_characterController;

        public GroundAndGravityUitility(CharacterController characterController, GravityConfig gravityConfig)
        {
            m_characterController = characterController;
            m_gravityConfig = gravityConfig;
        }

        public void CheckedGround(LayerMask groundMask)
        {
            // m_characterController.center 바닥에서 조금 띄어져있는 상태
            Vector3 worldCenter = m_characterController.transform.TransformPoint(m_characterController.center);
            float _height = m_characterController.height;

            Vector3 _colliderButtomtr = worldCenter - Vector3.up * (m_characterController.height * 0.5f - m_characterController.skinWidth);

            bool _groundCheck = Physics.CheckSphere(_colliderButtomtr, m_groundDistance, groundMask);

            // 점프같은 상태에서 바로 체크를 하면 True가 나온 후 False가 나오기에
            // 프레임 단위로 약간의 시간차를 두고 체크
            if (_groundCheck) m_lastGroundTime = Time.time;
            IsGround = (Time.time - m_lastGroundTime) <= 0.1f;
        }

        public float GetGravity()
        {
            return m_gravityConfig.BaseGravity;
        }

        public Vector3 ApplyGravity(float gravity, float currentSpeed, Vector3 moveDir, Vector3 currentVelocity)
        {
            // 현재 속력에서 y속력 값(중력) 적용
            currentVelocity.y += gravity * Time.deltaTime;

            // 이동 방향에 다시 속력(중력) 적용
            Vector3 _gravityMove = (moveDir * currentSpeed) + currentVelocity;
            m_characterController.Move(_gravityMove * Time.deltaTime);
            return currentVelocity;
        }
    }
}