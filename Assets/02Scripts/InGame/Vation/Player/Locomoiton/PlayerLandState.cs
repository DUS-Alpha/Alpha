using UnityEngine;

public class PlayerLandState : PlayerLocomotionState
{
    public PlayerLandState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;
    private float m_delayT;

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetIsAction(true);
        m_delayT = 0;
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        m_delayT += Time.deltaTime;
        if (m_delayT < 0.4f) return;

        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);
    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.SetIsAction(false);
    }
}
