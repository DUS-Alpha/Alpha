using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerCombat m_combat;
    private InputLockedFlagsController<InputLocoLockType> m_inputLockedFlags;
    private InputLockedFlagsController<InputCombatLockType> m_inputCombatFlags;

    //  TODO : new InpuSystem 전환할지 고려
    #region ==================== LocomotionInput
    public Vector3 MoveDir { get; private set; }
    public bool IsDodge { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
    #endregion ==================== /LocomotionInput

    #region ==================== CombatInput
    public bool IsAttack { get; private set; }
    public int SwapWeaponNum { get; private set; } = 0;
    public bool IsAim { get; private set; }
    public bool IsWeaponSwap { get; private set; }
    public bool IsReload { get; private set; }
    public bool IsSkill1 { get; private set; }
    #endregion ==================== /CombatInput

    #region ==================== ETC
    private bool m_isLocomotionLock;
    private bool m_isCombatLock;
    public bool IsInventory { get; private set; }

    #endregion ==================== /ETC

    public void InitializeModule(PlayerCombat playerCombat, InputLockedFlagsController<InputLocoLockType> inputLocoflags, InputLockedFlagsController<InputCombatLockType> inputCombatflags)
    {
        m_combat = playerCombat;
        m_inputLockedFlags = inputLocoflags;
        m_inputCombatFlags = inputCombatflags;
    }

    private void Start()
    {
        SwapWeaponNum = 0;
    }
    // Update is called once per frame
    void Update()
    {
        LocomotionInput();
        CombatInput();

        IsInventory = Input.GetKeyDown(KeyCode.I);
    }

    private void LocomotionInput()
    {
        if (m_inputLockedFlags.HasFlag(InputLocoLockType.All))
        {
            IsJump = false;
            IsDodge = false;
            IsFlyUp = false;
            IsFlyOff = false;
            MoveDir = Vector3.zero;
            return;
        }

        HandleInputMove();

        IsJump = !m_inputLockedFlags.HasFlag(InputLocoLockType.Jump)
            && Input.GetKeyDown(KeyCode.Space);
        IsFlyUp = !m_inputLockedFlags.HasFlag(InputLocoLockType.FlyUp)
            && Input.GetKeyDown(KeyCode.Q);
        IsFlyOff = !m_inputLockedFlags.HasFlag(InputLocoLockType.FlyOff) && Input.GetKeyDown(KeyCode.E);
        IsDodge = !m_inputLockedFlags.HasFlag(InputLocoLockType.Dodge) 
            && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
    }
    private void HandleInputMove()
    {
        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 조이스틱 같은 누르는 민감도를 지켜주기 위해 0~1까지는 기본 GetAxis값으로하되
        // 힘(민감도) 1이상일 때 (즉,대각선루트2, 거의 조이스틱 대각선 풀로 움직인 상태) 변경해줌 / 키보드 같은 버튼 방식은 필요없음
        if (MoveDir.magnitude >= 1) MoveDir.Normalize();

        if (m_inputLockedFlags.HasFlag(InputLocoLockType.Move))
        {
            MoveDir = Vector3.zero;
        }
    }

    private void CombatInput()
    {
        if(m_inputCombatFlags.HasFlag(InputCombatLockType.All))
        {
            IsAttack = false;
            IsAim = false;
            IsReload = false;
            SwapWeaponNum = 0;
            return;
        }
        WeaponSwapNum();
        IsAttack = !m_inputCombatFlags.HasFlag(InputCombatLockType.Attack) && Input.GetMouseButton(0);
  
        if (m_combat.CurrentWeaponNum > 1)
        {
            IsAim = (!m_inputCombatFlags.HasFlag(InputCombatLockType.Aim) && Input.GetMouseButtonDown(1));  
            IsReload = !m_inputCombatFlags.HasFlag(InputCombatLockType.Reload) && Input.GetKeyDown(KeyCode.R);
        }
        else
        {
            IsAim = false;
            IsReload = false;
        }
    }

    private void WeaponSwapNum()
    {
        IsWeaponSwap = (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3));
        
        if(IsWeaponSwap)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwapWeaponNum = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwapWeaponNum = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwapWeaponNum = 3;
            }
        }
        else
        {
            SwapWeaponNum = 0;
        }
    }
}
