using UnityEngine;

namespace alpha
{
    // “의도적 행동 도메인”
    // 행동을 수행/중단하는 주체
    public class Combat
    {
        public bool IsMeleeAttack => m_isMeleeAttack;
        private bool m_isMeleeAttack;

        public bool IsRangeAttack => m_isRangeAttack;
        private bool m_isRangeAttack;

        public bool IsSkilled => m_isSkilled;
        private bool m_isSkilled;

        public bool IsSwap => m_isSwap;
        private bool m_isSwap;

        public bool IsCombatReady => m_isCombatReady;
        private bool m_isCombatReady;
        public CombatConstraint Constraint;
        public Combat()
        {
            Constraint = new CombatConstraint()
            {
                IsMeleeAttack = m_isMeleeAttack,
                IsRangeAttack = m_isRangeAttack,
                IsSkilled = m_isSkilled,
                IsSwap = m_isSwap,
                IsCombatReady = m_isCombatReady
            };
        }

        public CombatConstraint GetConstraint()
        {

            Constraint.IsMeleeAttack = m_isMeleeAttack;
            Constraint.IsRangeAttack = m_isRangeAttack;
            Constraint.IsSkilled = m_isSkilled;
            Constraint.IsSwap = m_isSwap;
            Constraint.IsCombatReady = m_isCombatReady;

            return Constraint;
        }

        // 스왑

        // 공격

        // 스킬

        // 공격 준비중
    }
}