using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    public class InventoryModule : MonoBehaviour, IInventoryPort
    {
        // 슬롯 생성 and View 연결 
        private IInventoryViewPort m_uiView;

        public void Bind(IInventoryViewPort uiView)
        {
            m_uiView = uiView;
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