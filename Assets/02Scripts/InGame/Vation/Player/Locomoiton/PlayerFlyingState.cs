using UnityEngine;

public class PlayerFlyingState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.Skill;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.Skill;

    public PlayerFlyingState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {
        base.Enter();
    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        bool _isAming = m_Combat.IsAttack ? (m_Combat.CurrentWeaponNum > 1 ? true : false) : false || m_Combat.IsAiming;
        m_Locomotion.Movement(_isAming);
        //m_Locomotion.UpdateFlyingGauge();

        if (m_Locomotion.IsFlyOff || m_Locomotion.FlyingGauge <= 0)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
            m_Locomotion.SetVelocityY(2.5f);
        }
        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.FlyingExit();
       // m_PlayerCore.SetAnimatorLayer(1, 0);
    }


}
