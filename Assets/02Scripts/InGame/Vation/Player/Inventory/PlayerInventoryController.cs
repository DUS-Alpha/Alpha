using UnityEngine;

namespace alpha
{
    public class PlayerInventoryController : MonoBehaviour
    {
        // Class Ref
        private PlayerEquipManager m_equipManager;
        private PlayerCombat m_combat;

        public InventoryUI InventoryUI;
        
        private int m_swapNum;

        private void Start()
        {
            InventoryUI.OnEquipRequest += HandleEquip;
            InventoryUI.OnUnEquipRequest += HandleUnEquip;
            InventoryUI.OnUseRequest += HandleUse;

            // 누가 이벤트를 가지고 있어야 하는가?
            //m_equipManager.OnSwapAction += HandleSwap;
        }

        public void InitializeModule(PlayerEquipManager equipManager, PlayerCombat combat)
        {
            m_equipManager = equipManager;

            m_combat = combat;
        }

        private void HandleEquip(ItemDataSO data)
        {
            m_equipManager.TryEquip(data);
        }
        private void HandleUnEquip(ItemDataSO data)
        {
            m_equipManager.TryUnEquip(data);
        }
        private void HandleSwap(int swapNum)
        {
            //Combat.Swap(swapNum);
        }

        private void HandleUse(ItemDataSO data)
        {
            //playerCore.UseItem(data);
        }


    }
}