using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveDir { get; private set; }
    public bool IsSprint { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsAttack { get; private set; }
    public bool IsMouseRightDown { get; private set; }
    public bool IsFly { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
    public bool IsDodge { get; private set; }
    public bool IsSkill1 { get; private set; }

    public int SwapWeaponNum { get; private set; }
    // Update is called once per frame
    void Update()
    {
        HandleInputMove();
        IsSprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        IsJump = Input.GetKeyDown(KeyCode.Space);
        IsAttack = Input.GetMouseButton(0);
        IsMouseRightDown = Input.GetMouseButtonDown(1);
        IsFly = Input.GetKeyDown(KeyCode.Q);
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
}
