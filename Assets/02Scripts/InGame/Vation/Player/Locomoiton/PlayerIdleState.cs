using UnityEngine;

public class PlayerIdleState : PlayerLocomotionState
{
    // playerCore 부모생성자 생성, 플레이어의 컴포넌트 하나로 쓰는
    // m_PlayerCore, m_Locomotion 사용하면됨
    public PlayerIdleState(PlayerCore playerCore) : base(playerCore) {}

    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
       Vector3 _moveDir = m_Locomotion.LocomotionMovement(m_PlayerCore);

        if (_moveDir != Vector3.zero)
            m_PlayerCore.SwitchState(new PlayerMoveState(m_PlayerCore));
    }

    public override void Exit()
    {
        
    }
}
