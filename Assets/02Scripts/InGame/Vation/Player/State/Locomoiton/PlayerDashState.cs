using UnityEngine;

public class PlayerDashState : PlayerLocomotionStateBase
{
    public PlayerDashState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetLocomotionLock(true);
        m_Combat.SetIsCombatLock(true);

        m_Locomotion.DashEnter();
        m_NextStateDelay = 0;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.UpdateDashMove();
        m_NextStateDelay += Time.deltaTime;

        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }
        
        if (m_NextStateDelay > 0.45f)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);

    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.SetLocomotionLock(false);
        m_Combat.SetIsCombatLock(false);
        
        m_Locomotion.DashExit();
    }
}
