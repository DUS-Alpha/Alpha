using UnityEngine;

public class PlayerMoveState : PlayerLocomotionStateBase
{
    public PlayerMoveState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
   
    }

    public override void Update()
    {
        if(m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }

        bool _isJump = m_InputM.IsJump;
        bool _isFlyUp = m_InputM.IsFlyUp;

        m_Locomotion.Movement(true, m_InputM.MoveDirInput, m_InputM.LookInput, true, false, m_Combat.IsInCombat);
        m_Locomotion.ApplyGravity();


        if (_isJump)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        else if (_isFlyUp)
        {
            if(m_Locomotion.ActionGauge > 0.1f)
                m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }

    }
    public override void Exit()
    {
        
    }
}
