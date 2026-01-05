
namespace alpha
{
    public class PlayerFlightMoveState : PlayerLocomotionStateBase
    {
        public PlayerFlightMoveState(PlayerCore playerCore) : base(playerCore) { }
        public override void Enter()
        {
            WorldAudioManager.Instance.PlaySFXLoop(0, SFX_LoopTypes.AirField);
            m_AniM.SetFlightMoveType(m_Combat.IsInCombat);
        }

        public override void Update()
        {
            bool _isFlyUp = m_InputM.IsFlyUp;

            m_Locomotion.Movement(true, m_InputM.MoveDirInput, m_InputM.LookInput, true, m_Combat.IsInCombat);

            // 애니메이션
            m_AniM.FlightMoveAni(m_InputM.MoveDirInput.x, m_InputM.MoveDirInput.y, m_Combat.IsInCombat);

            // 오디오


            // 상태 전환
            if (_isFlyUp || m_Locomotion.ActionGauge < 0.1f)
            {
                //m_Core.SwitchLocomotionState(LocomotionStateType.Fall);
            }

            /*
            if(m_Locomotion.IsDie)
            {
                m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
                return;
            }

            if (m_Locomotion.IsFlyUp || m_Locomotion.ActionGauge < 0.1f)
            {
                m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
                m_Locomotion.SetVelocityY(2.5f);
            }*/

            // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
        }

        public override void Exit()
        {

        }


    }
}