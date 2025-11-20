using UnityEngine;

public class PlayerSwapWeaponState : PlayerCombatStateBase
{
    public PlayerSwapWeaponState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public override void Enter()
    {
        base.Enter();
        m_NextStateDelay = 0f;
        m_Combat.EnterSwapWeapon(m_Locomotion.IsFlying);
        
        m_PlayerCore.CameraManger.ChangeCM(CMType.BaseCM);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Locomotion.IsDie) return;

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
