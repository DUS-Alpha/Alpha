using UnityEngine;

public class PlayerFlyingState : PlayerState
{
    public PlayerFlyingState(PlayerCore playerCore) : base(playerCore) { }
    public override void Enter()
    {

    }
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        m_Locomotion.Movement();
        m_Combat.SwapWeapon();

        if (m_Locomotion.IsFlyUp)
            m_PlayerCore.SwitchState(new PlayerFlyUpStartState(m_PlayerCore));
        else if (m_Locomotion.IsFlyOff)
            m_PlayerCore.SwitchState(new PlayerFallState(m_PlayerCore));
        else if (m_Combat.IsAttack)
            m_PlayerCore.SwitchState(new PlayerAttackState(m_PlayerCore));
        // TODO : 중력 적용 시 Idle 전환 처리(애니메이션 처리 미흡해서 현재는 적용x)
    }

    public override void Exit()
    {
        m_Locomotion.FlyExit();
    }


}
