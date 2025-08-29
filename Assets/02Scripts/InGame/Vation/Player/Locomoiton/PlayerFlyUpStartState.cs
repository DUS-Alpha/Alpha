using UnityEngine;

public class PlayerFlyUpStartState : PlayerState
{
    public PlayerFlyUpStartState(PlayerCore playerCore) : base(playerCore){}
    private float m_delay;
    public override void Enter()
    {
        m_Locomotion.FlyUpStart();
        m_delay = 0f;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_delay += Time.deltaTime / 0.4f;
        if (m_delay >= 1f)
        {
            m_PlayerCore.SwitchState(new PlayerFlyUpState(m_PlayerCore));
        }
    }
    public override void Exit()
    {
        m_delay = 0f;
    }
}
