using UnityEngine;

namespace alpha
{
    public class PlayerInputManager : MonoBehaviour, ILocoInput, IInventory
    {
        private PlayerControls m_playerControl;         // InputSystem

        //  TODO : new InpuSystem 전환할지 고려
        #region ==================== LocomotionInput
        public Vector2 MoveDirInput { get; private set; }
        public bool IsMove { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool IsRotLock { get; private set; }
        public bool IsJump => m_jumpFrame == Time.frameCount;   // 다음 프레임에서 false로 변환해줌
        private int m_jumpFrame;

        public bool IsDash => m_dashFrame == Time.frameCount;
        private int m_dashFrame;
        
        public bool IsFlyUp => m_flyUpFrame == Time.frameCount;
        private int m_flyUpFrame;
        #endregion ==================== /LocomotionInput

        #region ==================== CombatInput
        public bool IsAttackBtn { get; private set; }
        public bool IsAim { get; private set; }
        public int SwapNum { get; private set; }
        public bool IsSwap => m_swapFrame == Time.frameCount;   // 다음 프레임에서 false로 변환해줌
        private int m_swapFrame;

        //public bool IsReload { get; private set; }
        public bool IsSkill { get; private set; }
        public string SkillKey { get; private set; }
        private KeyCode[] m_skillKeyCodes = { KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C };
        public bool IsSkill1 { get; private set; }
        public bool IsSkill2 { get; private set; }
        public bool IsSkill3 { get; private set; }
        public bool IsSkill4 { get; private set; }
        #endregion ==================== /CombatInput

        // UI


        // 상호작용
        public void OnEnable()
        {
            if (m_playerControl == null)
            {
                m_playerControl = new PlayerControls();

                // ==================== Locomotion
                m_playerControl.PlayerLocomotion.Move.performed += i => MoveDirInput = i.ReadValue<Vector2>();

                m_playerControl.PlayerLocomotion.LookInput.performed += i => LookInput = i.ReadValue<Vector2>();

                m_playerControl.PlayerLocomotion.IsRotLock.performed += i => IsRotLock = i.ReadValue<float>() > 0.5f; // 자료형 액션타입이 Button에 대해 bool이 아닌 float로 받아와짐
                m_playerControl.PlayerLocomotion.IsRotLock.canceled += i => IsRotLock = i.ReadValue<float>() > 0.5f;

                m_playerControl.PlayerLocomotion.Jump.performed += i =>
                {
                    m_jumpFrame = Time.frameCount;
                };
                m_playerControl.PlayerLocomotion.FlyUp.performed += i =>
                {
                    m_flyUpFrame = Time.frameCount;
                };
                m_playerControl.PlayerCombat.DodgeSkill.performed += i =>
                {
                    m_dashFrame = Time.frameCount;
                };

                // ==================== Combat
                // Swap
                m_playerControl.PlayerCombat.SwapNum.performed += i =>
                {
                    m_swapFrame = Time.frameCount;
                    SwapNum = int.Parse(i.control.name);
                };

                // Attack
                m_playerControl.PlayerCombat.Attack.performed += i => IsAttackBtn = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Attack.canceled += i => IsAttackBtn = i.ReadValue<float>() > 0.5f;

                // Aim
                m_playerControl.PlayerCombat.Aim.performed += i => IsAim = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Aim.canceled += i => IsAim = i.ReadValue<float>() > 0.5f;

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

                m_playerControl.PlayerLocomotion.Jump.performed -= i => m_jumpFrame = Time.frameCount;
                m_playerControl.PlayerLocomotion.FlyUp.performed -= i => m_flyUpFrame = Time.frameCount;

                // ==================== Combat
                // Swap
                m_playerControl.PlayerCombat.SwapNum.performed -= i => m_swapFrame = Time.frameCount;
                

                // Attack
                m_playerControl.PlayerCombat.Attack.performed -= i => IsAttackBtn = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Attack.canceled -= i => IsAttackBtn = i.ReadValue<float>() > 0.5f;
                // Aim
                m_playerControl.PlayerCombat.Aim.performed -= i => IsAim = i.ReadValue<float>() > 0.5f;
                m_playerControl.PlayerCombat.Aim.canceled -= i => IsAim = i.ReadValue<float>() > 0.5f;

                // Skill
                m_playerControl.PlayerCombat.DodgeSkill.performed -= i => m_dashFrame = Time.frameCount;

            }
            m_playerControl.Disable();
        }

    }
}