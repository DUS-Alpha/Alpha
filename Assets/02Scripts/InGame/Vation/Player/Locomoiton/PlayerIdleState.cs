using System.Security.Claims;
using UnityEngine;

public class PlayerIdleState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.None;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.None;

    // playerCore 부모생성자 생성, 플레이어의 컴포넌트 하나로 쓰는
    // m_PlayerCore, m_Locomotion 사용하면됨
    public PlayerIdleState(PlayerCore playerCore) : base(playerCore) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        if (!m_Locomotion.IsGrounded) return;

        bool _isAming = m_Combat.IsAttack ? (m_Combat.CurrentWeaponNum > 1 ? true : false) : false || m_Combat.IsAiming;
        m_Locomotion.Movement(_isAming);
        m_Locomotion.ApplyGravity();

        // Locomotion Switch State
        if (m_Locomotion.IsJump)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        }
        else if (m_Locomotion.IsDodge)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Dodge);
        }
        else if (m_Locomotion.IsFlyUp && m_Locomotion.FlyingGauge == m_Locomotion.MaxFlyingGauge)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.MoveDir != Vector3.zero)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Move);
        // else if(IsDie)
    }

    public override void Exit()
    {
        base .Exit();

        
    }
}
