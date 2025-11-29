using UnityEngine;

public class PlayerSwapState : PlayerCombatStateBase
{
    public PlayerSwapState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public override void Enter()
    {
        base.Enter();

        m_Combat.EnterSwap();
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.IsAction) return;

        if(m_Combat.IsInCombat) m_PlayerCore.SwitchCombatState(CombatStateType.InCombat);
        else m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
    }

    public override void Exit()
    {
        base.Exit();
        m_Combat.ExitSwap(m_Locomotion.IsFlying);
    }
}
