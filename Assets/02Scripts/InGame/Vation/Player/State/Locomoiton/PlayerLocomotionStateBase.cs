using UnityEngine;

public abstract class PlayerLocomotionStateBase : PlayerStateBase
{
    public PlayerLocomotionStateBase(PlayerCore playerCore) : base(playerCore){}

    // Input 잠금
    protected abstract InputCombatLockType m_LockOnEnter  { get; }
    protected abstract InputCombatLockType m_LockOnExit  { get; }
    
public override void Enter()
    {
        if (m_LockOnEnter == InputCombatLockType.All) 
            m_PlayerCore.CombatFlagsController.AddAllFlags();
        else
            m_PlayerCore.CombatFlagsController.AddLockedFlag(m_LockOnEnter);
    }
    public abstract override void FixedUpdate();
    public abstract override void Update();
    public override void Exit()
    {
        if (m_LockOnEnter == InputCombatLockType.All)
            m_PlayerCore.CombatFlagsController.ClearAllFlags();
        else
            m_PlayerCore.CombatFlagsController.RemoveLockedFlag(m_LockOnExit);
    }
}
