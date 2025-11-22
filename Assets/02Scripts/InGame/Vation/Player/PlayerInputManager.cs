using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace alpha
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerCombat m_combat;
        private InputLockedFlagsController<InputLocoLockType> m_inputLockedFlags;
        private InputLockedFlagsController<InputCombatLockType> m_inputCombatFlags;

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
        public bool IsAttack { get; private set; }
        public bool IsAim { get; private set; }
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

        private void OnEnable()
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
                m_playerControl.PlayerCombat.SwapNum1.performed += i => SwapNum = int.Parse(i.control.name);
                m_playerControl.PlayerCombat.SwapNum1.canceled += i => { if (SwapNum == 1) SwapNum = 0; };

                m_playerControl.PlayerCombat.SwapNum2.performed += i => SwapNum = int.Parse(i.control.name);
                m_playerControl.PlayerCombat.SwapNum2.canceled += i => { if (SwapNum == 2) SwapNum = 0; };

                m_playerControl.PlayerCombat.SwapNum3.performed += i => SwapNum = int.Parse(i.control.name);
                m_playerControl.PlayerCombat.SwapNum3.canceled += i => { if (SwapNum == 3) SwapNum = 0; };

                m_playerControl.PlayerCombat.SwapNum4.performed += i => SwapNum = int.Parse(i.control.name);
                m_playerControl.PlayerCombat.SwapNum4.canceled += i => { if (SwapNum == 4) SwapNum = 0; };

                // Attack



                // Skill





            }
            m_playerControl.Enable();   //Enable해야 m_playerControl의 인풋시스템 입력처리가 활성화됨
        }

        public void InitializeModule(PlayerCombat playerCombat, InputLockedFlagsController<InputLocoLockType> inputLocoflags, InputLockedFlagsController<InputCombatLockType> inputCombatflags)
        {
            m_combat = playerCombat;
            m_inputLockedFlags = inputLocoflags;
            m_inputCombatFlags = inputCombatflags;
        }

        private void Start()
        {
            //SwapWeaponNum = 0;
        }
        // Update is called once per frame
        void Update()
        {
            LocomotionInput();
            CombatInput();
        }
        #region ================================================================================ LOCOMOTION
        private void LocomotionInput()
        {
            HandleInputMove();
        }

        private void HandleInputMove()
        {
            // 조이스틱 같은 누르는 민감도를 지켜주기 위해 0~1까지는 기본 GetAxis값으로하되
            // 힘(민감도) 1이상일 때 (즉,대각선루트2, 거의 조이스틱 대각선 풀로 움직인 상태) 변경해줌 / 키보드 같은 버튼 방식은 필요없음
            if (MoveDirInput.magnitude >= 1) MoveDirInput.Normalize();

            IsMove = MoveDirInput.sqrMagnitude > 0.1f;
        }
        #endregion ================================================================================ /LOCOMOTION

        #region ================================================================================ COMBAT
        private void CombatInput()
        {
            //IsSwap = IsSwapInput();
            IsAttack = Input.GetMouseButton(0);
            IsAim = Input.GetMouseButtonDown(1) && m_combat.CurrentWeaponNum > 1;
            //IsReload = Input.GetKeyDown(KeyCode.R);
            IsSkill = SkillKeyCode();
        }

        /*private bool IsSwapInput()
        {
            // 1 ~ 4
            for (int i = 1; i <= 4; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    SwapNum = i;
                    return true;
                }
            }
            return false;
        }*/

        public bool SkillKeyCode()
        {
            foreach (var key in m_skillKeyCodes)
            {
                if (Input.GetKeyDown(key))
                {
                    SkillKey = key.ToString();
                    return true;
                }
            }
            return false;
        }
        #endregion ================================================================================ /COMBAT
    }
}