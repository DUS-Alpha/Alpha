using UnityEngine;

public class PlayerFlightMoveState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.Skill;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.Skill;

    public PlayerFlightMoveState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {
        base.Enter();
        
    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        m_Locomotion.Movement(m_Combat.IsInCombat, m_Combat.IsAction);
        m_Locomotion.UpdateFlightMove(m_Combat.CurrentWeaponNum > 0);

        if(m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
        }

        if (m_Locomotion.IsFlyUp || m_Locomotion.FlyingGauge <= 0)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
            m_Locomotion.SetVelocityY(2.5f);
        }

        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.FlightMoveExit();
       // m_PlayerCore.SetAnimatorLayer(1, 0);
    }


}
