using UnityEngine;

public class PlayerDashState : PlayerLocomotionState
{
    public PlayerDashState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => throw new System.NotImplementedException();

    protected override InputCombatLockType m_LockOnExit => throw new System.NotImplementedException();

    public override void Enter()
    {
        base.Enter();
    }
    public override void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
