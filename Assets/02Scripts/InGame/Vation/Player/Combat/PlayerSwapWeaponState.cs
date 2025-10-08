using UnityEngine;

public class PlayerSwapWeaponState : PlayerCombatState
{
    public PlayerSwapWeaponState(PlayerCore playerCore) : base(playerCore)
    {
    }

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;
    private float m_delayT;

    public override void Enter()
    {
        base.Enter();
        m_delayT = 0f;
        m_Combat.EnterSwapWeapon(m_Locomotion.IsFlying);
        
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {

        m_delayT += Time.deltaTime;

        if (m_delayT < 0.7f) return;
        
        if (m_Combat.IsInCombat)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
            
        }
        else
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
    }


    public override void Exit()
    {
        base.Exit();
        m_Combat.ExitSwapWeapon();
    }
}
