using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        bool _isAttack = m_PlayerCore.InputHandler.IsAttack;

        if(!_isAttack)
        {
            m_PlayerCore.SwitchState(new PlayerIdleState(m_PlayerCore));
        }
        else
            m_Combat.Attack(_isAttack);
        
    }
    public override void Exit()
    {
        m_Combat.Attack(false);
    }
}
