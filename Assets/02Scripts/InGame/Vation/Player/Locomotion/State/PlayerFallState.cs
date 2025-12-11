using UnityEngine;

public class PlayerFallState : PlayerLocomotionStateBase
{
    public PlayerFallState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
       

    }

    public override void Update()
    {
        m_Locomotion.AirMovement();
        m_Locomotion.ApplyGravity();

        bool _isFlyUp = m_InputM.IsFlyUp;

        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }
        if (_isFlyUp && m_Locomotion.ActionGauge > 0)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.IsGrounded)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Land);
        }
    }

    public override void Exit()
    {
       
    }
}
