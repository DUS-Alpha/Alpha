using UnityEngine;
namespace alpha
{
    public class PlayerLandState : PlayerLocomotionStateBase
    {
        public PlayerLandState(PlayerCore playerCore) : base(playerCore) { }
        private float m_delayT;
        private float m_duration;
        public override void Enter()
        {
            // m_Core.SetLockState(true, false);

            // 애니메이션
            m_AniM.LandAni(m_Locomotion.CurrentFallType);

            // 오디오
            m_Audio.PlayLocomotionAudio(0, SFX_LomotionType.Land);

            // 낙하 타입에 따른 전환 시점 설정
            if (m_Locomotion.CurrentFallType == EFallType.FlyFall) m_duration = 0.8f;
            else m_duration = 0.4f;

            m_delayT = 0;
        }

        public override void Update()
        {
            m_delayT += Time.deltaTime;

            if (m_delayT < m_duration) return;

            // m_Core.SwitchLocomotionState(LocomotionStateType.Idle);
        }
        public override void Exit()
        {
            // 낙하 타입 초기화
            m_Locomotion.SetFallType(EFallType.None);
        }
    }
}