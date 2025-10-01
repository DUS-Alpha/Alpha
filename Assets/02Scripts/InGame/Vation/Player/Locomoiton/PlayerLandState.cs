using UnityEngine;

public class PlayerLandState : PlayerLocomotionState
{
    public PlayerLandState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;
    private float m_delayT;
    private float m_duration;
    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetIsAction(true);
        m_delayT = 0;
        m_Locomotion.EnterLanding();
        if (m_Locomotion.IsFlyFall) m_duration = 0.8f;
        else m_duration = 0.4f;
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        m_delayT += Time.deltaTime;
        if (m_delayT < m_duration) return;

        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);
    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.SetIsAction(false);
        m_Locomotion.ExitLanding();
    }
}
