using UnityEngine;

public class PlayerMoveState : PlayerLocomotionState
{
    public PlayerMoveState(PlayerCore playerCore) : base(playerCore) {}

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
      Vector3 _moveDir  = m_Locomotion.LocomotionMovement(m_PlayerCore);

        if (_moveDir == Vector3.zero) 
            m_PlayerCore.SwitchState(new PlayerIdleState(m_PlayerCore));
    }
}
