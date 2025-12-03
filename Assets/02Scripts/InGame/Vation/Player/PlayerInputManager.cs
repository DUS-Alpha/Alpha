using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace alpha
{
    public class PlayerInputManager
    {
        private PlayerControls m_playerControl;         // InputSystem

        //  TODO : new InpuSystem 전환할지 고려
        #region ==================== LocomotionInput
        public Vector2 MoveDirInput { get; private set; }
        public bool IsMove { get; private set; }

        public Vector2 LookInput { get; private set; }
        public bool IsRotLock { get; private set; }

        private int m_dashFrame;
        public bool IsDash => m_dashFrame == Time.frameCount;   // 다음 프레임에서 false로 변환해줌
        private int m_jumpFrame;
        public bool IsJump => m_jumpFrame == Time.frameCount;
        private int m_flyUpFrame;
        public bool IsFlyUp => m_flyUpFrame == Time.frameCount;
        #endregion ==================== /LocomotionInput

        #region ==================== CombatInput
        public bool IsAttackBtn { get; private set; }
        public bool IsAim { get; private set; }
        public bool IsSwap => m_isSwap;
        public bool m_isSwap;
        public int SwapNum { get; private set; } = 0;

        //public bool IsReload { get; private set; }
        public bool IsSkill { get; private set; }
        public string SkillKey { get; private set; }
        private KeyCode[] m_skillKeyCodes = { KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C };
        public bool IsSkill1 { get; private set; }
        public bool IsSkill2 { get; private set; }
        public bool IsSkill3 { get; private set; }
        public bool IsSkill4 { get; private set; }
        #endregion ==================== /CombatInput

        public void OnEnable()
        {
            if (m_playerControl == null)
            {
                m_playerControl = new PlayerControls();

                // ==================== Locomotion
                m_playerControl.PlayerLocomotion.Move.performed += i => MoveDirInput = i.ReadValue<Vector2>();

                m_playerControl.PlayerLocomotion.IsRotLock.performed += i => IsRotLock = i.ReadValue<float>() > 0.5f; // 자료형 액션타입이 Button에 대해 bool이 아닌 float로 받아와짐
                m_playerControl.PlayerLocomotion.IsRotLock.canceled += i => IsRotLock = i.ReadValue<float>() > 0.5f;

                m_playerControl.PlayerLocomotion.Dodge.performed += i => m_dashFrame = Time.frameCount;
                m_playerControl.PlayerLocomotion.Jump.performed += i => m_jumpFrame = Time.frameCount;
                m_playerControl.PlayerLocomotion.Fly.performed += i => m_flyUpFrame = Time.frameCount;

                // ==================== Combat
                // Swap
                m_playerControl.PlayerCombat.SwapNum.performed += i => SwapNum = int.Parse(i.control.name);

                // Attack
                m_playerControl.PlayerCombat.Attack.performed += i => IsAttackBtn = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Attack.canceled += i => IsAttackBtn = i.ReadValue<float>() > 0.5f;

                // Skill


            }
            m_playerControl.Enable();   //Enable해야 m_playerControl의 인풋시스템 입력처리가 활성화됨
        }

        public void OnDisable()
        {
            if (m_playerControl != null)
            {
                // ==================== Locomotion
                m_playerControl.PlayerLocomotion.Move.performed -= i => MoveDirInput = i.ReadValue<Vector2>();

                m_playerControl.PlayerLocomotion.IsRotLock.performed -= i => IsRotLock = i.ReadValue<float>() > 0.5f; // 자료형 액션타입이 Button에 대해 bool이 아닌 float로 받아와짐
                m_playerControl.PlayerLocomotion.IsRotLock.canceled -= i => IsRotLock = i.ReadValue<float>() > 0.5f;

                m_playerControl.PlayerLocomotion.Dodge.performed -= i => m_dashFrame = Time.frameCount;
                m_playerControl.PlayerLocomotion.Jump.performed -= i => m_jumpFrame = Time.frameCount;
                m_playerControl.PlayerLocomotion.Fly.performed -= i => m_flyUpFrame = Time.frameCount;

                // ==================== Combat
                // Swap
                m_playerControl.PlayerCombat.SwapNum.performed -= i => SwapNum = int.Parse(i.control.name);

                // Attack
                m_playerControl.PlayerCombat.Attack.performed -= i => IsAttackBtn = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Attack.canceled -= i => IsAttackBtn = i.ReadValue<float>() > 0.5f;

                // Skill


            }
            m_playerControl.Disable();
        }

        // Update is called once per frame
        public void Update()
        {
            HandleInputMove();
        }
        #region ================================================================================ LOCOMOTION

        private void HandleInputMove()
        {
            // 조이스틱 같은 누르는 민감도를 지켜주기 위해 0~1까지는 기본 GetAxis값으로하되
            // 힘(민감도) 1이상일 때 (즉,대각선루트2, 거의 조이스틱 대각선 풀로 움직인 상태) 변경해줌 / 키보드 같은 버튼 방식은 필요없음
            if (MoveDirInput.magnitude >= 1) MoveDirInput.Normalize();

            IsMove = MoveDirInput.sqrMagnitude > 0.1f;
        }
        #endregion ================================================================================ /LOCOMOTION

        #region ================================================================================ COMBAT
        public void SetSwapNum(int swapNum)
        {
            SwapNum = swapNum;
        }
        public void SetIsSwap(bool isSwap)
        {
            m_isSwap = isSwap;
        }
        #endregion ================================================================================ /COMBAT
    }
}