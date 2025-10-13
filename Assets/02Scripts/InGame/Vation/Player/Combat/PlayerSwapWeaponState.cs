using UnityEngine;

public class PlayerSwapWeaponState : PlayerCombatState
{
    public PlayerSwapWeaponState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public override void Enter()
    {
        base.Enter();
        m_NextStateDelay = 0f;
        m_Combat.EnterSwapWeapon(m_Locomotion.IsFlying);

        if(m_Combat.CurrentWeaponNum > 1)
            m_PlayerCore.CameraManger.ChangeCM(CMType.RangeRifleCM);
        else 
            m_PlayerCore.CameraManger.ChangeCM(CMType.MeleeCM);

    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_NextStateDelay += Time.deltaTime;

        if (m_NextStateDelay < 0.3f)
            m_Combat.SwapInventoryWeapon();
        
        if (m_NextStateDelay < 0.7f) return;
        
        m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
    }


    public override void Exit()
    {
        base.Exit();
        m_Combat.ExitSwapWeapon(m_Locomotion.IsFlying);
    }
}
