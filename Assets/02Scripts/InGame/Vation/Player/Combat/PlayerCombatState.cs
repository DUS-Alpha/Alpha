using UnityEngine;

public abstract class PlayerCombatState : PlayerState
{
    public PlayerCombatState(PlayerCore playerCore) : base(playerCore){}

    // Input 잠금
    protected abstract InputLocoLockType m_LockOnEnter { get; }
    protected abstract InputLocoLockType m_LockOnExit { get; }

    public override void Enter()
    {
        if (m_LockOnEnter == InputLocoLockType.All)
            m_PlayerCore.LocomotionFlagsController.AddAllFlags();
        else
            m_PlayerCore.LocomotionFlagsController.AddLockedFlag(m_LockOnEnter);
        DebugCurrentState();
    }

    public abstract override void FixedUpdate();

    public abstract override void Update();

    public override void Exit()
    {
        if (m_LockOnEnter == InputLocoLockType.All)
            m_PlayerCore.LocomotionFlagsController.ClearAllFlags();
        else
            m_PlayerCore.LocomotionFlagsController.RemoveLockedFlag(m_LockOnExit);
    }
}
