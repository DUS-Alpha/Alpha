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
        /*if (m_Combat.IsAim)
        {
            m_PlayerCore.SwitchCombatState(CombatFlagsState.Aiming);
        }
        else if(m_Combat.IsAttack)
        {
            if (m_Combat.CurrentWeaponNum > 1)
            {
                m_PlayerCore.SwitchCombatState(CombatFlagsState.Aiming);
            }
            //else m_PlayerCore.SwitchCombatState(CombatState.Attack);
        } 
        else if (m_Combat.IsWeaponSwap) m_PlayerCore.SwitchCombatState(CombatFlagsState.Swap);*/
    }

    public override void Exit()
    {
        
    }
}
