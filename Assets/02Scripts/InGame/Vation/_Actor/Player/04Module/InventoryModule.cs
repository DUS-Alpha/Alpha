using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    public class InventoryModule : MonoBehaviour, IInventoryPort
    {
        // ==================== Ref Component
        private IInventoryViewPort m_uiViewPort;

        // ==================== Config Data


        // ==================== Runtime Data
        // Slot
        private List<SlotModuleBase> m_inventorySlotList;
        private List<ArmorSlot> m_armorSlotList;
        private List<SlotModuleBase> m_quickSlotList;

        // ==================== State Data

        public void Bind(IInventoryViewPort uiView)
        {
            m_uiViewPort = uiView;
        }
        public void CreateInventorySlot()
        {

        }

        public void ConnectedSlotView()
        {

        }

        // 아이템 저장
        // TODO : Port로
        public void AddItem(ItemModuleBase item)
        {
            // 인벤토리 슬롯 추가 가능 여부

            // 슬롯에 추가

            // 뷰에 전달
            
        }
    }
}