using UnityEngine;

public class PlayerFlyingState : PlayerState
{
    public PlayerFlyingState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {

    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    { 
        m_Locomotion.Movement(m_Combat.IsAim);

        /*if (m_Locomotion.IsFlyUp)
            m_PlayerCore.SwitchLocomotionState(LocomotionState.FlyStartUp);
        else if (m_Locomotion.IsFlyOff)
            m_PlayerCore.SwitchLocomotionState(LocomotionState.Fall);*/
        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        m_Locomotion.FlyExit();
    }


}
