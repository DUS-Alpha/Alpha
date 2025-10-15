using UnityEngine;

public class PlayerDodgeState : PlayerLocomotionState
{
    public PlayerDodgeState(PlayerCore playerCore) : base(playerCore)
    {
    }
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;
    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public override void Enter()
    {
        base.Enter();
    }


    public override void FixedUpdate()
    {
       
    }

    public override void Update()
    {
        if (m_Locomotion.IsDie) return;
        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
