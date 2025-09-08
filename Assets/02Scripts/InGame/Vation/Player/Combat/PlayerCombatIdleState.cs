using UnityEngine;

public class PlayerCombatIdleState : PlayerState
{
    public PlayerCombatIdleState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.CurrentWeaponNum > 1)
        {
            if (m_Combat.IsAim)
            {
                m_PlayerCore.SwitchCombatState(CombatState.Aim);
            }
        }
        else if(m_Combat.IsAttack)
        {
            if (m_Combat.CurrentWeaponNum > 1)
            {
                m_PlayerCore.SwitchCombatState(CombatState.Aim);
            }
            else m_PlayerCore.SwitchCombatState(CombatState.Attack);
        } 
        else if (m_Combat.IsWeaponSwap) m_PlayerCore.SwitchCombatState(CombatState.Swap);
    }

    public override void Exit()
    {
        
    }
}
