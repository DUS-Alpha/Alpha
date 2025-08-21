using UnityEngine;

public class PlayerJumpState : PlayerLocomotionState
{
    public PlayerJumpState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        m_Locomotion.JumpStart();
    }
  
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.JumpUpdate();
    }

    public override void Exit()
    {
        m_Locomotion.JumpExit();
    }
}
