using UnityEngine;

public class PlayerSwapWeaponState : PlayerCombatState
{
    public PlayerSwapWeaponState(PlayerCore playerCore) : base(playerCore)
    {
    }

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Dodge;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Dodge;
    private float m_delayT;

    public override void Enter()
    {
        base.Enter();
        m_delayT = 0f;
        m_Combat.EnterSwapWeapon();
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if(!m_Combat.IsAction)
        {
            m_delayT += Time.deltaTime;

            if(m_delayT > 0.2f)
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
        }
    }
    public override void Exit()
    {
        base.Exit();
        m_Combat.ExitSwapWeapon();
    }
}
