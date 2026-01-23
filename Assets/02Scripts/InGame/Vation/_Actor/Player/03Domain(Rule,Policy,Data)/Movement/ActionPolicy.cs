using UnityEngine;

namespace alpha
{
    public class ActionPolicy : IActionPolicy
    {
        private LocomotionRules m_locoRule;
        private CombatRules m_combatRule;

        public ActionPolicy(LocomotionRules locoRule, CombatRules combatRule)
        {
            m_locoRule = locoRule;
            m_combatRule = combatRule;
        }

        #region ================ Locomotion Action Policy ==================
        public bool CanMove(bool isGrounded, CombatStateData combatData)
        {
            if (!m_locoRule.CanMove(isGrounded)) { return false; }
            if(combatData.IsMeleeAttacking) { return false; }
            if (combatData.IsSkilling) { return false; }

            return true;
        }

        public bool CanJump(bool isGrounded, CombatStateData combatData)
        {
            if(!m_locoRule.CanJump(isGrounded)) return false;
            //if(StateType == LocomotionStateType.Dash) return false;
            if (combatData.IsMeleeAttacking) return false;
            if (combatData.IsSkilling) return false;

            return true;
        }

        public bool FinishLand(float aniTime)
        {
            // 착지 애니메이션 종료 후 가능
            if (aniTime < 0.95f) return false;
            return true;
        }

        public bool CanDash(float gauge, CombatStateData combatData)
        {
            if(!m_locoRule.CanDash(gauge)) return false;

            return true;
        }

        public bool CanFlyUp(CombatStateData combatData)
        {

            return true;
        }

        #endregion ================ Locomotion Action Policy ==================

        #region ================ Combat Action Policy ==================

        #endregion ================ Combat Action Policy ==================
    }
}