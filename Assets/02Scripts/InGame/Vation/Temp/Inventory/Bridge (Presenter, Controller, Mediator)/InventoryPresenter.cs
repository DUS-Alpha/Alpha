using alpha;
using UnityEngine;

namespace alpha
{
    // Model 상태를 UI로 변환하는 계층
    // Snapshot → UI, UI 이벤트 전달
    // Model 변경 x, 게임 규칙 x
    public class InventoryPresenter
    {
        [SerializeField] private InventoryController m_inventoryController;

        private IInventoryController m_inventoryModel;
        //private IInventoryViewPort m_inventoryUIView;

        public InventoryPresenter(IInventoryViewPort inventoryUI)
        {
            //m_inventoryUIView = inventoryUI;
        }

        // 원래는 Presenter를 인스톨러 클래스에서 주입해줘야 하지만, 지금은 간단히 인스펙터에서 할당
        /*private void Awake()
        {
            m_inventoryModel = m_inventoryController;
            m_inventoryUIView = m_inventoryUI;
        }

        private void OnEnable()
        {
            m_inventoryUIView.OnClickAddInventory += HandleExpandInventory;

            m_inventoryModel.OnUpdateSlotUI += HandleOnAddItem;
            m_inventoryUIView.OnDragDrop += HandleDragDrop;

            m_inventoryModel.OnEquipItem += HandleEquipItem;
            m_inventoryModel.OnUnEquipItem += HandleUnEquipItem;
        }*/

        // 인벤토리 늘리는 이벤트가 발생되면 호출
        public void HandleExpandInventory()
        {
            ISlotModel slot = m_inventoryModel.ExpandInventorySlot();

            // i보다는 실제 모델의 인덱스를 주입해주기
            //m_inventoryUIView.AddAndBindInventorySlotUI(slot.CurrentSlotIndex);
        }

        private void HandleOnAddItem(ISlotModel slot)
        {
            //m_inventoryUIView.UpdateSlotUI(slot);
        }

        private bool HandleDragDrop(SlotUIBase dragUI, SlotUIBase dropUI)
        {
            EItemTypes _fromType = dragUI.SlotItemType;
            int _fromIndex = dragUI.SlotIndex;

            EItemTypes _toType = dropUI.SlotItemType;
            int _toIndex = dropUI.SlotIndex;
            //return m_inventoryModel.ExecuteDragDrop(_fromType, _fromIndex, _toType, _toIndex);
            return false;
        }
    }
}