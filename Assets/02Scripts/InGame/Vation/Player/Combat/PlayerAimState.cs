using UnityEngine;

public class PlayerAimState : PlayerState
{
    public PlayerAimState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        m_Combat.Aiming(true);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if(m_Combat.IsAttack)
            m_PlayerCore.SwitchCombatState(CombatState.Attack);
        else if(!m_Combat.IsAim)
        {
            m_Combat.Aiming(false);
            m_PlayerCore.SwitchCombatState(CombatState.CombatIdle);
        }
    }
    public override void Exit()
    {
        
    }
}
