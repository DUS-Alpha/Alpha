using UnityEngine;

public class PlayerDieState : PlayerLocomotionStateBase
{
    public PlayerDieState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        m_Locomotion.EnterDie();
    }

    public override void Update()
    {
        if(m_Locomotion.IsFlying)
        {
            m_Locomotion.ApplyGravity();
        }
    }
    public override void Exit()
    {

    }
}
