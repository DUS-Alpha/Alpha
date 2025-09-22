using UnityEngine;

public class PlayerReloadState : PlayerCombatState
{
    public PlayerReloadState(PlayerCore playerCore) : base(playerCore)
    {
    }

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Dodge;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Dodge;

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
