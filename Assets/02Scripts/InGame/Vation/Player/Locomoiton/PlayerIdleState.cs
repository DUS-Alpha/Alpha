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
        // 모든 Locomotion 값 초기화
       // m_Locomotion.InitializeLocotion();
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }

        m_Locomotion.Movement(m_Combat.IsInCombat, m_Combat.IsAction);
        m_Locomotion.ApplyGravity();


        // Locomotion Switch State
        if (m_Locomotion.IsJump) 
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        else if (m_Locomotion.IsFlyUp && m_Locomotion.FlyingGauge > 0) 
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        else if (m_Locomotion.IsMoving)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Move);
    }

    public override void Exit()
    {
        base .Exit();
    }
}
