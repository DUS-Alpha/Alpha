using System;
using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    public class InventoryModule : MonoBehaviour, IInventoryPickupPort, IInventoryPresenterPort
    {
        // ==================== Ref Component
        private IInventoryViewPort m_uiViewPort;

        // ==================== Config Data
        [SerializeField] private int m_inventorySlotCount;

        // ==================== Runtime Data
        // Slot
        private List<SlotModuleBase> m_inventorySlotList;
        private List<ArmorSlot> m_armorSlotList;
        private List<SlotModuleBase> m_quickSlotList;

        // ==================== State Data
        public event Action<int> OnCreateInventorySlotView;
        public void Bind(IInventoryViewPort uiView)
        {
            m_uiViewPort = uiView;
        }
        private void Start()
        {
            CreateInventorySlot(m_inventorySlotCount);
        }
        
        // 생성
        public void CreateInventorySlot(int inventorySlotCount)
        {
            // 리스트에 생성된 슬롯 저장
            m_inventorySlotList = new List<SlotModuleBase>();
            for (int i = 0; i < m_inventorySlotCount; i++)
            {
                m_inventorySlotList.Add(new SlotModuleBase(i));
            }

            // 뷰도 생성 및 연결 (이벤트로)
            OnCreateInventorySlotView?.Invoke(inventorySlotCount);
        }

        // 미리 셋팅
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