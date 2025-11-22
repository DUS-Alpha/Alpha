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
            int _swapNum = m_inputManager.SwapNum;

            // 1. Swap이 가능한지 Combat(행동이 가능한지) && EquipManger(슬롯에 무기있는지)에서 확인
            bool _isSwap = !m_combat.IsActionLock && m_equipManager.CanSwap(_swapNum);
            
            if(_isSwap)
            {
                HandleSwap();
            }
        }

        private void HandleEquip(ItemDataSO data)
        {
            m_equipManager.TryEquip(data);
        }
        private void HandleUnEquip(ItemDataSO data)
        {
            m_equipManager.TryUnEquip(data);
        }

        private void HandleSwap()
        {
            // 1. EquipManager에서 무기 교체
            Item _item = m_equipManager.TrySwap();
            int _swapNum = m_equipManager.CurrentSwapNum;

            // 2. EquipManager -> Combat 정보 전달
            m_combat.SwapAction(_swapNum, _item);
        }

        private void HandleUse(ItemDataSO data)
        {
            //playerCore.UseItem(data);
        }


    }
}