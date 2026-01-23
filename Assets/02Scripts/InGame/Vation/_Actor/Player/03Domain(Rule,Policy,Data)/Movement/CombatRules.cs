using UnityEngine;

namespace alpha
{
    public class CombatRules
    {
        public bool CanAttack()
        {
            return true;
        }

        public bool CanSwap(Item weapon)
        {
            return weapon != null;
        }

        public bool CanSkill(bool isCool)
        {
            return isCool;
        }

        public bool CanCombatReady()
        {
            return true;
        }
    }
}