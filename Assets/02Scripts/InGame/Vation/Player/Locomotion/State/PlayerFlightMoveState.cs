using UnityEngine;

public class PlayerFlightMoveState : PlayerLocomotionStateBase
{
    public PlayerFlightMoveState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {
        m_Locomotion.EnterFlightMove();
    }

    public override void Update()
    {
        bool _isFlyUp = m_InputM.IsFlyUp;


        m_Locomotion.Movement(true, m_InputM.MoveDirInput, m_InputM.LookInput, true, true, m_Combat.IsInCombat);
        
        if(_isFlyUp || m_Locomotion.ActionGauge < 0.1f)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
        }
       
        /*
        if(m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }

        if (m_Locomotion.IsFlyUp || m_Locomotion.ActionGauge < 0.1f)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
            m_Locomotion.SetVelocityY(2.5f);
        }*/

        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        m_Locomotion.ExitFlightMove();
       // m_PlayerCore.SetAnimatorLayer(1, 0);
    }


}
