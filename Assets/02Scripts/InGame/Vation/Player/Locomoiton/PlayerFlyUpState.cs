using UnityEngine;

public class PlayerFlyUpState : PlayerState
{
    public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        
    }

   
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        bool _isFlyUp = m_PlayerCore.InputHandler.IsFlyUp;

        if(_isFlyUp)
            m_Locomotion.FlyUp();
        else if(!_isFlyUp)
        {
            m_PlayerCore.SwitchState(new PlayerFlyState(m_PlayerCore));
        }
    }
    public override void Exit()
    {
        m_Locomotion.FlyUpExit();
    }
}
