using UnityEngine;
using UnityEngine.Windows;
public class PlayerSkillState : PlayerCombatStateBase
{
    public PlayerSkillState(PlayerCore playerCore) : base(playerCore){}

    private bool m_isMelee;
    private float m_nextDelay;
    private float m_aniLength;
    public override void Enter()
    {
        if (m_Combat.CurrentSwapNum == 1) m_isMelee = true;
        m_Combat.EnterSkill();
        //m_Combat.SetIsAction(true);
        
        m_nextDelay = 0;
       
        // 켜진 현재 애니메이션 끝나는지 확인
        //m_PlayerCore.CameraManger.StopRotCamera(true);
    }

    public override void Update()
    {
        //m_Combat.SetIsAction(true);
        m_aniLength = m_Core.AniManager.GetCurrentAniInfo(2);
        m_nextDelay += Time.deltaTime;
        if (m_nextDelay < m_aniLength + 1.3f) return;

        m_Core.SwitchCombatState(CombatStateType.NonCombat);
    }
    
    public override void Exit()
    {
        m_Combat.ExitSkill();
        //m_Combat.SetIsAction(false);
    }
}
