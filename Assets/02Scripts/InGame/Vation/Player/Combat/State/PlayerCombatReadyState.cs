using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

// 공격 대기 상태
public class PlayerCombatReadyState : PlayerCombatStateBase
{
    public PlayerCombatReadyState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        m_NextStateDelay = 0;
    }

    public override void Update()
    {
        m_NextStateDelay += Time.deltaTime;
        m_Combat.InvokeRegenerateGauge();
        if (m_Combat.CanSwap1)
        {
            m_Core.SwitchCombatState(CombatStateType.Swap);
            return;
        }
        if(m_Combat.IsAttackBtn)
        {
            m_Core.SwitchCombatState(CombatStateType.Attack);
            return;
        }

        if (m_NextStateDelay > 2.5f)
        {
            m_Core.SwitchCombatState(CombatStateType.NonCombat);
            m_Combat.SetIsInCombat(false);
        }
    }
    public override void Exit()
    {

    }

}
