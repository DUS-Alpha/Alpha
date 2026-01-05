using UnityEngine;

namespace alpha
{
    // 수직 이륙
    public class PlayerFlyUp : PlayerLocomotionStateBase
    {
        public PlayerFlyUp(PlayerCore playerCore) : base(playerCore) { }

        private float m_currentFlyHeight = 0f;      // 현재 FlyUp 높이
        private float m_currentFlyUpSpeed;

        public override void Enter()
        {
            // 초기 상승 속도 설정
            m_currentFlyHeight = 0f;
            m_currentFlyUpSpeed = m_Locomotion.InitialFlySpeed;

            // 애니메이션
            m_AniM.FlyUpAni();

            // 오디오
            m_Audio.PlayLocomotionAudio(0, SFX_LomotionType.FlyUp);

            // 낙하 타입 설정
            m_Locomotion.SetFallType(EFallType.FlyFall);

            // Comabat 잠금
            //m_Core.SetLockState(true, false);

            m_NextStateDelay = 0f;
        }

        public override void Update()
        {
            m_NextStateDelay += Time.deltaTime;

            if (m_NextStateDelay < 0.4f) return;
            OnUpdateFlyUp();

            // 애니메이션 모션 자연스럽게하기 위해 딜레이 적용
            if (m_NextStateDelay < 1.1f) return;
            //m_Core.SwitchLocomotionState(LocomotionStateType.FlightMove);
        }

        public override void Exit()
        {
            //m_Core.SetLockState(false, false);
        }

        // 로코모션에서 관리하는게 맞지 않을까 고민해보기
        public void OnUpdateFlyUp()
        {
            // 속도를 점점 줄임
            m_currentFlyUpSpeed = Mathf.Max(0f, m_currentFlyUpSpeed - m_Locomotion.FlyDecel * Time.deltaTime);

            // 목표 높이까지 상승
            float deltaY = m_currentFlyUpSpeed * Time.deltaTime;

            // 남은 높이보다 더 올라가면 조정
            if (m_currentFlyHeight + deltaY > m_Locomotion.TargetFlyHeight)
                deltaY = m_Locomotion.TargetFlyHeight - m_currentFlyHeight;

            m_currentFlyHeight += deltaY;

            // 수직 이동만 적용
            m_Locomotion.CharacterCtrl.Move(Vector3.up * deltaY);
        }
    }
}