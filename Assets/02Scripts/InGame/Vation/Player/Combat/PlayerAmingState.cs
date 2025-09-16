using UnityEngine;

public class PlayerAmingState : PlayerCombatState
{
    public PlayerAmingState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Look;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Look;
    public override void Enter()
    {
        base.Enter();
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
        else if(m_Combat.IsMeleeAttack || m_Combat.IsRangeShooting)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.MeleeAttack_All);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
