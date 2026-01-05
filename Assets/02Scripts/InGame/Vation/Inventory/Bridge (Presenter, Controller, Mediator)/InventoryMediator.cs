using UnityEngine;

namespace alpha
{
    // 서로 몰라야 하는 객체들의 상호작용을 중계
    // 이벤트 라우팅, 호출 연결
    // 도메인(계산, ) 판단 x, 흐름 판단 o
    public class InventoryMediator
    {
        private InventoryController m_controller;
        private InventoryPresenter m_presenter;

        public InventoryMediator(InventoryController controller, InventoryPresenter presenter)
        {
            m_controller = controller;
            m_presenter = presenter;
        }

        public void HandleAddItme()
        {

        }

        public void HandleExpandInventorySlot()
        {

        }

        public void HandleDragAndDrop()
        {

        }

    }
}