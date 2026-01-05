using UnityEngine;

namespace alpha
{
    public class PlayerSkillState : PlayerCombatStateBase
    {
        public PlayerSkillState(PlayerCore playerCore) : base(playerCore) { }

        private bool m_isMelee;
        private float m_nextDelay;
        private float m_aniLength;
        public override void Enter()
        {
            m_nextDelay = 0;
        }

        public override void Update()
        {
            //m_aniLength = m_Core.AniManager.GetCurrentAniInfo(2);
            m_nextDelay += Time.deltaTime;
            if (m_nextDelay < m_aniLength + 1.3f) return;

            //m_Core.SwitchCombatState(CombatStateType.NonCombat);
        }

        public override void Exit()
        {

        }
    }
}