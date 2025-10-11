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
    public bool IsMove {  get; private set; }
    public bool ISRot {  get; private set; }
    public bool IsDash { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
    #endregion ==================== /LocomotionInput

    #region ==================== CombatInput
    public bool IsAttack { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsSwap { get; private set; }
    public int SwapWeaponNum { get; private set; } = 0;
    public bool IsReload { get; private set; }
    public bool IsSkill {  get; private set; }
    public string SkillKey { get; private set; }
    private KeyCode[] m_skillKeyCodes = { KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C };
    public bool IsSkill1 { get; private set; }
    public bool IsSkill2 {  get; private set; }
    public bool IsSkill3 { get; private set; }
    public bool IsSkill4 { get; private set; }
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
        HandleInputMove();
        bool _isRot = Input.GetKeyDown(KeyCode.LeftAlt);
        if (_isRot) ISRot = !ISRot;
        IsJump = Input.GetKeyDown(KeyCode.Space);
        IsFlyUp = Input.GetKeyDown(KeyCode.F);
        IsFlyOff = Input.GetKeyDown(KeyCode.E);
        IsDash = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
    }
    private void HandleInputMove()
    {
        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 조이스틱 같은 누르는 민감도를 지켜주기 위해 0~1까지는 기본 GetAxis값으로하되
        // 힘(민감도) 1이상일 때 (즉,대각선루트2, 거의 조이스틱 대각선 풀로 움직인 상태) 변경해줌 / 키보드 같은 버튼 방식은 필요없음
        if (MoveDir.magnitude >= 1) MoveDir.Normalize();

        IsMove = MoveDir.sqrMagnitude > 0.1f;
    }

    private void CombatInput()
    {
        IsSwap = IsSwapInput();
        IsAttack = Input.GetMouseButton(0);
        IsAim = Input.GetMouseButtonDown(1) && m_combat.CurrentWeaponNum > 1;
        IsReload = Input.GetKeyDown(KeyCode.R);

        IsSkill = SkillKeyCode();
    }

    private bool IsSwapInput()
    {
        // 1 ~ 4
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                SwapWeaponNum = i;
                return true;
            }
        }
        return false;
    }

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
}
