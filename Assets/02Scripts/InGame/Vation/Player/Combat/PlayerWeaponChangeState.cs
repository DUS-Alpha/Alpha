using UnityEngine;

public class PlayerWeaponChangeState : PlayerState
{
    public PlayerWeaponChangeState(PlayerCore playerCore) : base(playerCore){}

    private float m_nextChangeTime;

    public override void Enter()
    {
        m_nextChangeTime = Time.time + 0.1f;
        m_Combat.EnterWeaponChange();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        //if (Time.time >= m_nextChangeTime) //생각보다 0.1f초의 영향이 발생됨(버벅임)
        {
            m_PlayerCore.SwitchState(m_PlayerCore.StateMachine.PrevState);  
        }
    }
    public override void Exit()
    {
        m_nextChangeTime = 0;

    }
}
