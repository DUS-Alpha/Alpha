using UnityEngine;
using UnityEngine.Windows;
public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerCore playerCore) : base(playerCore){}
    private bool m_isCombo;

    public override void Enter()
    {
        if (m_Combat.CurrentWeaponNum == 1)
            m_Combat.SetIsAllBodyAction(true);
        else m_Combat.SetIsAllBodyAction(false);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Combat.Attack(m_Combat.IsAttack);

        if(m_Combat.IsCombatProgressing)
        {
            bool _isTag = m_PlayerCore.AniController.CheckComboAnimation();

            // 콤보 시작 지점 기록
            if (!m_isCombo && _isTag)
            {
                m_isCombo = true;
                return;
            }

            // 콤보가 끝났을 때만 Idle 전환
            if (m_isCombo && !_isTag)
            {
                m_PlayerCore.SwitchCombatState(CombatState.CombatIdle);
            }
        }
        else
        {
            if(!m_Combat.IsAttack) m_PlayerCore.SwitchCombatState(CombatState.CombatIdle);
        }
    }
    public override void Exit()
    {
        m_Combat.SetIsAllBodyAction(false);
        m_Combat.Attack(false);
        if (m_Combat.CurrentWeaponNum > 1) m_Combat.Aiming(false);
    }
}
