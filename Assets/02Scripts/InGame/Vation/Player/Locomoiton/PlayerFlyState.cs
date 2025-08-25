using UnityEngine;

public class PlayerFlyState : PlayerLocomotionState
{
    public PlayerFlyState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {

    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        bool _isFlyOff = m_PlayerCore.InputHandler.IsFlyOff;
        bool _IsFlyUpStart = m_PlayerCore.InputHandler.IsFlyUp;
        

        m_Locomotion.LocomotionFlyMovement();
        if (_IsFlyUpStart)
            m_PlayerCore.SwitchState(new PlayerFlyUpStartState(m_PlayerCore));
        else if (_isFlyOff)
            m_PlayerCore.SwitchState(new PlayerFallState(m_PlayerCore));
        
        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        m_Locomotion.FlyExit();
    }


}
