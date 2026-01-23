namespace alpha
{
    public class CombatStateData
    {
        public bool IsMeleeAttacking { get; set; }
        public bool IsRangedAttacking { get; set; }
        public bool IsInCombat { get; set; }
        public bool IsCombatReady { get; set; }
        public bool IsSkilling { get; set; }
        public bool IsSwapping { get; set; }
    }
}