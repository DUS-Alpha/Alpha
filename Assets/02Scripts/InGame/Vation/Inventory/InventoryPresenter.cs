using alpha;
using UnityEngine;

namespace alpha
{
    // 중재자 역할 (MVP패턴)
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private InventoryManager m_inventoryManager;
        [SerializeField] private InventoryUIManager m_inventoryUI;

        private IInventoryService m_inventoryModel;
        private IInventoryUIService m_inventoryUIView;

        // 원래는 Presenter를 인스톨러 클래스에서 주입해줘야 하지만, 지금은 간단히 인스펙터에서 할당
        private void Awake()
        {
            m_inventoryModel = m_inventoryManager;
            m_inventoryUIView = m_inventoryUI;
        }

        private void OnEnable()
        {
            m_inventoryUIView.OnClickAddInventory += HandleExpandInventory;

            m_inventoryModel.OnUpdateSlotUI += HandleOnAddItem;

            m_inventoryUIView.OnDragDrop += HandleDragDrop;
        }

        private void Start()
        {
            // 초기 인벤토리 생성
            int _n = m_inventoryModel.VerticalInventorySlotCount * m_inventoryModel.HorizontalInventorySlotCount;
            for (int i = 0; i < _n; i++)
            {
                HandleExpandInventory();
            }
        }

        // 인벤토리 늘리는 이벤트가 발생되면 호출
        public void HandleExpandInventory()
        {
            ISlotModel slot = m_inventoryModel.ExpandInventorySlot();

            // i보다는 실제 모델의 인덱스를 주입해주기
            m_inventoryUIView.AddAndBindInventorySlotUI(slot.CurrentSlotIndex);
        }

        private void HandleOnAddItem(ISlotModel slot)
        {
            m_inventoryUIView.UpdateSlotUI(slot);
        }

        private bool HandleDragDrop(SlotUIBase dragUI, SlotUIBase dropUI)
        {
            EItemTypes _fromType = dragUI.SlotItemType;
            int _fromIndex = dragUI.SlotIndex;

            EItemTypes _toType = dropUI.SlotItemType;
            int _toIndex = dropUI.SlotIndex;
            return m_inventoryModel.ExecuteDragDrop(_fromType, _fromIndex, _toType, _toIndex);
        }
    }
}