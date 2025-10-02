using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyUpState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore){}

    private bool m_canFlyUp;
    private float m_delayT;
    private float m_nextStateDelay;
    public override void Enter()
    {
        base.Enter();
        m_Locomotion.SetIsAction(true);
        
        m_Locomotion.FlyUpStart();
        m_delayT = 0f;
        m_nextStateDelay = 0f;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        // 애니메이션 모션 자연스럽게하기 위해 딜레이
        m_delayT += Time.deltaTime;

        if (m_delayT < 0.4f) return;
        m_Locomotion.FlyUpUpdate();

        m_nextStateDelay += Time.deltaTime;

        if (m_nextStateDelay < 1f) return;

        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlightMove);
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.SetIsAction(false);
    }
}
