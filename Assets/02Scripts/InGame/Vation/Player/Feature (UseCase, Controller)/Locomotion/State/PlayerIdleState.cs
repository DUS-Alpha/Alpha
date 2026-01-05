namespace alpha
{
    public class PlayerIdleState : PlayerLocomotionStateBase
    {
        // playerCore 부모생성자 생성, 플레이어의 컴포넌트 하나로 쓰는
        // m_PlayerCore, m_Locomotion 사용하면됨
        public PlayerIdleState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            //m_Core.SetLockState(false, false);
        }

        public override void Update()
        {
            //m_Core.SwitchLocomotionState(LocomotionStateType.Move);
        }

        public override void Exit()
        {

        }
    }
}