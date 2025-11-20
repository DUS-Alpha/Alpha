using UnityEngine;

public class PlayerDieState : PlayerLocomotionStateBase
{
    public PlayerDieState(PlayerCore playerCore) : base(playerCore){}

    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.EnterDie();
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if(m_Locomotion.IsFlying)
        {
            m_Locomotion.ApplyGravity();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
