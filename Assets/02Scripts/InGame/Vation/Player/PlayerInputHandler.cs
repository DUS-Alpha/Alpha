using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    //  TODO : new InpuSystem 전환할지 고려
    #region ==================== LocomotionInput
    public Vector3 MoveDir { get; private set; }
    public bool IsSprint { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
    #endregion ==================== /LocomotionInput

    #region ==================== CombatInput
    public bool IsAttack { get; private set; }
    public int SwapWeaponNum { get; private set; } = 0;
    public bool IsAim { get; private set; }
    public bool IsSniperScope { get; private set; }
    public bool IsWeaponSwap { get; private set; }
    public bool IsReload { get; private set; }
    public bool IsDodge { get; private set; }
    public bool IsSkill1 { get; private set; }
    #endregion ==================== /CombatInput

    #region ==================== ETC
    public bool IsInventory { get; private set; }
    #endregion ==================== /ETC


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
        IsSprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        IsJump = Input.GetKeyDown(KeyCode.Space);
        IsFlyUp = Input.GetKey(KeyCode.Q);
        IsFlyOff = Input.GetKeyDown(KeyCode.E);
    }
    private void HandleInputMove()
    {
        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 조이스틱 같은 누르는 민감도를 지켜주기 위해 0~1까지는 기본 GetAxis값으로하되
        // 힘(민감도) 1이상일 때 (즉,대각선루트2, 거의 조이스틱 대각선 풀로 움직인 상태) 변경해줌 / 키보드 같은 버튼 방식은 필요없음
        if (MoveDir.magnitude >= 1) MoveDir.Normalize();
    }

    private void CombatInput()
    {
        IsAttack = Input.GetMouseButton(0);
       
        IsReload = Input.GetKeyDown(KeyCode.R);
        WeaponSwapNum();
        IsAim = Input.GetMouseButton(1);
        IsSniperScope = Input.GetMouseButtonDown(1);
    }

    private void WeaponSwapNum()
    {
        IsWeaponSwap = Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3);

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
