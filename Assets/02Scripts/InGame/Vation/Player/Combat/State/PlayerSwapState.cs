using UnityEngine;

public class PlayerSwapState : PlayerCombatStateBase
{
    public PlayerSwapState(PlayerCore playerCore) : base(playerCore){}
 
    public override void Enter()
    {
        m_Combat.EnterSwap();
    }

    public override void Update()
    {
        m_Combat.InvokeRegenerateGauge();
        if (m_Combat.IsAction) return;

        if(m_Combat.IsInCombat) m_Core.SwitchCombatState(CombatStateType.CombatReady);
        else m_Core.SwitchCombatState(CombatStateType.NonCombat);
    }

    public override void Exit()
    {
        //m_Combat.ExitSwap(m_Locomotion.IsFlying);
    }
}
