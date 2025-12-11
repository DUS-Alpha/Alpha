using UnityEngine;

public class PlayerLandState : PlayerLocomotionStateBase
{
    public PlayerLandState(PlayerCore playerCore) : base(playerCore){}
    private float m_delayT;
    private float m_duration;
    public override void Enter()
    {
        m_Locomotion.SetLocomotionLock(true);
        m_Combat.SetIsCombatLock(true);

        m_delayT = 0;
        m_Locomotion.EnterLanding();
        m_duration = 0.4f;
    }

    public override void Update()
    {
        m_delayT += Time.deltaTime;
        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }
        if (m_delayT < m_duration) return;

        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);
    }
    public override void Exit()
    {
        m_Locomotion.SetLocomotionLock(false);
        m_Combat.SetIsCombatLock(false);

        m_Locomotion.ExitLanding();
    }
}
