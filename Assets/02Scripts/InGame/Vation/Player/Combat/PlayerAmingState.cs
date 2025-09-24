using UnityEngine;

public class PlayerAmingState : PlayerCombatState
{
    public PlayerAmingState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Look;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Look;

    private float m_delayTime;
    public override void Enter()
    {
        base.Enter();
        m_Combat.SetAming(true);
        m_delayTime = 0f;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        // TODO : Combat에서 처리
        if (m_Locomotion.IsJump || m_Locomotion.IsDodge || m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
        }
        else if(m_Combat.IsSwapWeapon)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        else if (m_Combat.IsAttack)
        {
            m_delayTime += Time.deltaTime;
            if(m_delayTime >= 0.2f)
            m_PlayerCore.SwitchCombatState(CombatStateType.Attack);

        }
        else if (!m_Combat.IsAim)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
        if(!m_Combat.IsAttack) m_Combat.SetAming(false, m_Locomotion.IsFlying);
    }
}
