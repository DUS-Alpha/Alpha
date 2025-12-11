using alpha;
using System.Security.Claims;
using UnityEngine;

public class PlayerIdleState : PlayerLocomotionStateBase
{
    // playerCore 부모생성자 생성, 플레이어의 컴포넌트 하나로 쓰는
    // m_PlayerCore, m_Locomotion 사용하면됨
    public PlayerIdleState(PlayerCore playerCore) : base(playerCore) { }

    public override void Enter()
    {
        // 모든 Locomotion 값 초기화
        // m_Locomotion.InitializeLocotion();
    }

    public override void Update()
    {
        m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Move);
    }

    public override void Exit()
    {

    }
}
