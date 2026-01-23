using UnityEngine;
namespace alpha
{
    public class PlayerLandState : PlayerStateBase
    {
        public PlayerLandState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsLand(EFallType.NormalFall);
        }

        public override void Update()
        {
            float _aniTime = m_LocomotionM.CheckedFinishAni();

            // Landing 애니메이션이 끝나도 다른 애니메이션 시간 받아올 수 있기에
            if (m_Core.ActionPolicy.FinishLand(_aniTime))
            {
                SetNextStateDelay(0.5f);
            }

            if (!m_CanNextState) return;

            if(Time.time >= m_NextStateDelay)
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Idle);
            
        }
        public override void Exit()
        {
            // 낙하 타입 초기화
            //m_Locomotion.SetFallType(EFallType.None);
        }
    }
}