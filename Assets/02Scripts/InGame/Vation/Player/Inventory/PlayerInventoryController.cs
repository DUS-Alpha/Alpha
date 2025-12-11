using UnityEngine;

namespace alpha
{
    public class PlayerInventoryController
    {
        private IEquipService m_equipService;
        private IInventoryUIService m_inventoryUIService;
       public void Start()
        {
            m_inventoryUIService.OnEquipRequest += HandleEquip;
            m_inventoryUIService.OnUnEquipRequest += HandleUnEquip;
            m_inventoryUIService.OnUseRequest += HandleUse;
        }

        public void InitializeModule(IEquipService equipService, IInventoryUIService inventoryUI)
        {
            m_equipService = equipService;
            m_inventoryUIService = inventoryUI;
        }

        private void HandleEquip(ItemDataSO data)
        {
            m_equipService.TryEquip(data);
        }
        private void HandleUnEquip(ItemDataSO data)
        {
            m_equipService.TryUnEquip(data);
        }

        private void HandleUse(ItemDataSO data)
        {
            //playerCore.UseItem(data);
        }
    }
}