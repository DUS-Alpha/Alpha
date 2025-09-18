using UnityEngine;

public class PlayerCombatIdleState : PlayerCombatState
{
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public PlayerCombatIdleState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.IsSwapWeapon) m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        else if (m_Combat.IsMeleeAttack)    // 우선은 같은 Attack으로 넘어가지만 향후를 위해 구분
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.MeleeAttack_All);
        }
        else if (m_Combat.IsRangeShooting || m_Combat.IsAim)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
