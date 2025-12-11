using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 수직 이륙
public class PlayerFlyUp : PlayerLocomotionStateBase
{
    public PlayerFlyUp(PlayerCore playerCore) : base(playerCore){}

    private float m_delayT;

    public override void Enter()
    {
        m_Locomotion.SetLocomotionLock(true);
        m_Combat.SetIsCombatLock(true);

        m_Locomotion.SetIsUnCheckGround(true);
        m_Locomotion.EnterFlyUp(m_Combat.CurrentSwapNum > 0);
        m_NextStateDelay = 0f;
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
        m_Locomotion.SetLocomotionLock(false);
        m_Combat.SetIsCombatLock(false);
    }
}
