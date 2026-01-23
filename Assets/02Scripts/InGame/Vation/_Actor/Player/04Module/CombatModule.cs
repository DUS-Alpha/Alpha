using UnityEngine;

namespace alpha
{
    public class CombatModule : MonoBehaviour
    {
        // ==================== Runtime Data : 변하는 데이터

        // State Data
        public CombatStateData CombatStateData = new CombatStateData();
        public bool IsMeleeAttacking { get; set; }
        public bool IsRangedAttacking { get; set; }
        public bool IsInCombat => IsMeleeAttacking || IsRangedAttacking;
        public bool IsCombatReady;
        public bool IsSkilling { get; set; }
        public bool IsSwapping { get; set; }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OnUpdateStateData();
        }

        // State Data 갱신
        // TODO : 차후 행위쪽에서 CombatStateData로 갱신
        private void OnUpdateStateData()
        {
            CombatStateData.IsMeleeAttacking= IsMeleeAttacking;
            CombatStateData.IsRangedAttacking= IsRangedAttacking;
            CombatStateData.IsSkilling= IsSkilling;
            CombatStateData.IsSwapping= IsSwapping;
            CombatStateData.IsCombatReady= IsCombatReady;
            CombatStateData.IsInCombat= IsInCombat;
        }
    }
}