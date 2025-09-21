using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyUpState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore){}

    private bool m_canFlyUp;
    private float delayT;
    public override void Enter()
    {
        base.Enter();
        m_Locomotion.FlyUpStart();
        delayT = 0f;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        // 애니메이션 모션 자연스럽게하기 위해 딜레이
        delayT += Time.deltaTime;

        if (delayT < 0.4f) return;

        // TODO 애니메이션 모션 이후 힘적용 초반 발사 느낌으로
        if (m_Locomotion.IsFlyUp)
            m_Locomotion.AntiGravity();
        else if(!m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Flying);
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.FlyUpExit();
    }
}
