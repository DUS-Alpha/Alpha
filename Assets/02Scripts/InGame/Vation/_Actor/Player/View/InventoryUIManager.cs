using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace alpha
{
    // View
    public class InventoryUIManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action OnClickAddInventory;

        [SerializeField] private Button m_addInventoryBtn;

        public event Action<ItemSO> OnEquipRequest;
        public event Action<ItemSO> OnUnEquipRequest;
        public event Action<ItemSO> OnUseRequest;
        public event Action<ItemSO> OnDropRequest;

        [Header("[ Connected Objects ]")]
        
        [SerializeField] private RectTransform m_contentRt;
        [SerializeField] private GameObject m_inventorySlotPrefab;  // 나머지 슬롯들은 미리 배치

        [Header("[ SlotUIs ]")]
        // 인벤토리를 제외한 나머지 슬롯들은 미리 만들어 배치하고 있기에 크기가 고정
        [SerializeField] private List<InventorySlotUI> m_inventorySlotUIs;
        [SerializeField] private ArmorSlotUI[] m_armorSlotUIs;              
        [SerializeField] private WeaponQuickSlotUI[] m_weaponQuickSlotUIs;
        [SerializeField] private UseableQuickSlotUI[] m_useableQuickSlotUIs;
        [Space(10)]

        [SerializeField] private DragSlotView m_dragSlot;

        [Space(10)]

        [Header("[ PopUpUI ]")]
        [SerializeField] private Transform m_popUITr;
        [SerializeField] private GameObject m_confirmUI;
        [SerializeField] private GameObject m_amountInputUI;
        private GameObject currentPopUpUI;

        public event Func<SlotUIBase, SlotUIBase, bool> OnDragDrop;

        public event Action<int> OnClickSlot;
        public event Action<int> OnDragSlotUI;

        private void Awake()
        {
            m_inventorySlotUIs = new List<InventorySlotUI>();

            // 인벤토리 슬롯 제외하고는 미리 배치된 슬롯들 가져오기
            m_armorSlotUIs = GetComponentsInChildren<ArmorSlotUI>();
            for(int i = 0; i < m_armorSlotUIs.Length; i++)
            {
                m_armorSlotUIs[i].SetIndex(i);
            }

            m_weaponQuickSlotUIs = GetComponentsInChildren<WeaponQuickSlotUI>();
            for (int i = 0; i < m_weaponQuickSlotUIs.Length; i++)
            {
                m_weaponQuickSlotUIs[i].SetIndex(i);
            }

            m_useableQuickSlotUIs = GetComponentsInChildren<UseableQuickSlotUI>();
            for (int i = 0; i < m_useableQuickSlotUIs.Length; i++)
            {
                m_useableQuickSlotUIs[i].SetIndex(i);
            }
        }

        private void OnEnable()
        {
            m_addInventoryBtn.onClick.AddListener(AddInventory);
        }

        private void AddInventory()
        {
            OnClickAddInventory?.Invoke();
        }

        // 만들어진 SlotModel에 대해 시각화 UI객체 생성하고 인덱스를 키값으로
        public void AddAndBindInventorySlotUI(int index)
        {
            GameObject _inventorySlotGO = Instantiate(m_inventorySlotPrefab, m_contentRt);
            InventorySlotUI _inventorySlotUI = _inventorySlotGO.GetComponent<InventorySlotUI>();
            _inventorySlotUI.SetIndex(index);

            m_inventorySlotUIs.Add(_inventorySlotUI);
        }

        public void UpdateSlotUI(ISlotModel slot)
        {
            SlotUIBase _slotBaseUI = GetSlotBaseUI(slot);
            _slotBaseUI.AddItem(slot);
        }

        private SlotUIBase GetSlotBaseUI(ISlotModel slot)
        {
            return slot.SlotItemType switch
            {
                EItemTypes.None => m_inventorySlotUIs[slot.CurrentSlotIndex],
                EItemTypes.Armor => m_armorSlotUIs[slot.CurrentSlotIndex],
                EItemTypes.Weapon => m_weaponQuickSlotUIs[slot.CurrentSlotIndex],
                EItemTypes.Useable => m_useableQuickSlotUIs[slot.CurrentSlotIndex],
            };
        }

        // 포인터가 클릭 혹은 드랍한 위치의 슬롯
        private SlotUIBase GetUnderPointerSlot(Vector2 pos)
        {
            foreach (var slotUI in m_inventorySlotUIs)
                if(slotUI.IsInRect(pos)) return slotUI;
            foreach (var slotUI in m_armorSlotUIs)
                if (slotUI.IsInRect(pos)) return slotUI;
            foreach (var slotUI in m_weaponQuickSlotUIs)
                if (slotUI.IsInRect(pos)) return slotUI;
            foreach (var slotUI in m_useableQuickSlotUIs)
                if (slotUI.IsInRect(pos)) return slotUI;
            return null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 클릭한 인벤토리 위치의 슬롯 판별
            SlotUIBase _clickSlot = GetUnderPointerSlot(eventData.position);

            // 인덱스 전달 UI -> Model
            OnClickSlot?.Invoke(_clickSlot.SlotIndex);

            if (_clickSlot == null) return;
            if (_clickSlot.IsEmpty) return;

            m_dragSlot.transform.position = eventData.position;
            //m_dragSlot.DragUI(_clickSlot);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //if (m_dragSlot.SlotUIBase == null) return;
            m_dragSlot.transform.position = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            //if (m_dragSlot.SlotUIBase == null) return;
            m_dragSlot.transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SlotUIBase _dropSlotUI = GetUnderPointerSlot(eventData.position);
            /*if (_dropSlotUI == null)
            {
                m_dragSlot.Clear();
            }*/
            /*SlotUIBase _clickSlot = GetUnderPointerSlot(eventData.position);
            if (_clickSlot == m_dragSlot && _clickSlot.SlotIndex == m_dragSlot.SlotUIBase.SlotIndex) m_dragSlot.Clear();*/
        }

        // 드래그 이후에만 동작됨
        public void OnEndDrag(PointerEventData eventData)
        {
            SlotUIBase _dropSlotUI = GetUnderPointerSlot(eventData.position);
            /*if (m_dragSlot.SlotUIBase == null || _dropSlotUI == null)
            {
                m_dragSlot.Clear();
                return;
            }*/

            // 아이템이 타슬롯과의 매칭이 맞는지 확인 및 변경
            /*bool _isDragDrop = OnDragDrop.Invoke(m_dragSlot.SlotUIBase, _dropSlotUI);

            if (!_isDragDrop)
            {
                Debug.Log("슬롯이 맞지 않습니다.");
            }*/

            m_dragSlot.Clear();
        }

        // TODO : 판단된 정보를 받는곳에서 다시 판단할 필요 없이 바로 사용할 수 있게 하는 방법 적용
        private void EquipItem(SlotUIBase targetSlot, ItemSO itemData)
        {
            // 슬롯 타입에 따라 이벤트 호출
            if (targetSlot is WeaponQuickSlotUI || targetSlot is ArmorSlotUI || targetSlot is UseableQuickSlotUI)
            {
                OnEquipRequest?.Invoke(itemData);
            }
        }

        private void UnEquipItem(SlotUIBase targetSlot, ItemSO itemDataSO)
        {
            // 슬롯 타입에 따라 이벤트 호출
            if (targetSlot is WeaponQuickSlotUI || targetSlot is ArmorSlotUI || targetSlot is UseableQuickSlotUI)
            {
                OnUnEquipRequest?.Invoke(itemDataSO);
            }
        }

        public void OpenUI()
        {
            this.gameObject.SetActive(true);
        }
        public void CloseUI()
        {
            this.gameObject.SetActive(false);
        }

    }

}
