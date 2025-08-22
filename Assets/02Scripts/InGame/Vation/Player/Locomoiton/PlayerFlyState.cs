using UnityEngine;

public class PlayerFlyState : PlayerLocomotionState
{
    public PlayerFlyState(PlayerCore playerCore) : base(playerCore){}
    float delayTime;
    public override void Enter()
    {
        m_Locomotion.FlyStart();
        delayTime = 0;
    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
       m_Locomotion.LocomotionFlyMovement();
        if(m_Locomotion.IsFlyDown)
            m_PlayerCore.SwitchState(new PlayerFallState(m_PlayerCore));
    }

    public override void Exit()
    {
        m_Locomotion.FlyExit();
    }

    
}
