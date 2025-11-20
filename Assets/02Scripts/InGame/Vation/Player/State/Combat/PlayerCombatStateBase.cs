using UnityEngine;

public abstract class PlayerCombatStateBase : PlayerStateBase
{
    public PlayerCombatStateBase(PlayerCore playerCore) : base(playerCore){}

    // Input 잠금
    protected abstract InputLocoLockType m_LockOnEnter { get; }
    protected abstract InputLocoLockType m_LockOnExit { get; }
    protected InputLocoLockType m_UpdateLock;
    public override void Enter()
    {
        if (m_LockOnEnter == InputLocoLockType.All)
            m_PlayerCore.LocomotionFlagsController.AddAllFlags();
        else
            m_PlayerCore.LocomotionFlagsController.AddLockedFlag(m_LockOnEnter);
    }

    public abstract override void FixedUpdate();

    public override void Update()
    {
        m_PlayerCore.LocomotionFlagsController.AddLockedFlag(m_UpdateLock);
    }

    public override void Exit()
    {
        if (m_LockOnEnter == InputLocoLockType.All)
            m_PlayerCore.LocomotionFlagsController.ClearAllFlags();
        else
            m_PlayerCore.LocomotionFlagsController.RemoveLockedFlag(m_LockOnExit);

        m_PlayerCore.LocomotionFlagsController.RemoveLockedFlag(m_UpdateLock);
    }
}
