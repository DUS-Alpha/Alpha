using UnityEngine;

public class PlayerDashState : PlayerLocomotionState
{
    public PlayerDashState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetIsAction(true);
        m_Locomotion.DashEnter();
        m_nextStateDelay = 0;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.UpdateDashMove();
        m_nextStateDelay += Time.deltaTime;

        if (m_nextStateDelay > 0.45f)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);

    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.DashExit();
        m_Locomotion.SetIsAction(false);
    }
}
