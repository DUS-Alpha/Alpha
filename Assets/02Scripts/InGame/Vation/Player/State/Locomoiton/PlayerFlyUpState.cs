using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyUpState : PlayerLocomotionStateBase
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore){}

    private float m_delayT;

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetLocomotionLock(true);
        m_Combat.SetIsCombatLock(true);

        m_Locomotion.EnterFlyUp(m_Combat.CurrentSwapNum > 0);
        m_NextStateDelay = 0f;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        // 애니메이션 모션 자연스럽게하기 위해 딜레이
        m_NextStateDelay += Time.deltaTime;

        if (m_Locomotion.IsDie)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
        
        if (m_NextStateDelay < 0.4f) return;
        m_Locomotion.UpdateFlyUp();

        if (m_NextStateDelay < 1.1f) return;
        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlightMove);
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.SetLocomotionLock(false);
        m_Combat.SetIsCombatLock(false);
    }
}
