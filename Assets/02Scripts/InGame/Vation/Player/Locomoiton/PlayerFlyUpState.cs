using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyUpState : PlayerState
{
    public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore){}
    
    public override void Enter()
    {
        m_Locomotion.FlyUpStart();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (!m_Locomotion.CanFlyUp) return;
        
        m_Locomotion.ApplyGravity();
        
        if(!m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Flying);
        }
    }

    public override void Exit()
    {
        m_Locomotion.FlyExit();
    }
}
