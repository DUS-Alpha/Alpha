using UnityEngine;

namespace alpha
{
    public class PlayerInventoryController : MonoBehaviour
    {
        // Class Ref
        private PlayerEquipManager m_equipManager;
        private PlayerCombat m_combat;
        private PlayerInputManager m_inputManager;
        public InventoryUI InventoryUI;
        private void Start()
        {
            InventoryUI.OnEquipRequest += HandleEquip;
            InventoryUI.OnUnEquipRequest += HandleUnEquip;
            InventoryUI.OnUseRequest += HandleUse;
        }

        public void InitializeModule(PlayerInputManager playerInputManager, PlayerEquipManager equipManager, PlayerCombat combat)
        {
            m_equipManager = equipManager;
            m_inputManager = playerInputManager;
            m_combat = combat;
        }
        public void InitializeEvents(IPlayerEvents events)
        {
            events.CheckInputAction += CheckInput;
        }

        private void CheckInput()
        {

        }

        private void HandleEquip(ItemDataSO data)
        {
            m_equipManager.TryEquip(data);
        }
        private void HandleUnEquip(ItemDataSO data)
        {
            m_equipManager.TryUnEquip(data);
        }

        private void HandleUse(ItemDataSO data)
        {
            //playerCore.UseItem(data);
        }


    }
}