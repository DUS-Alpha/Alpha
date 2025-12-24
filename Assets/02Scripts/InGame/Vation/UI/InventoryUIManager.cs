using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace alpha
{
    // View
    public class InventoryUIManager : MonoBehaviour, IInventoryUIService, IPointerDownHandler, IPointerUpHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action OnClickAddInventory;

        [SerializeField] private Button m_addInventoryBtn;

        public event Action<ItemDataSO> OnEquipRequest;
        public event Action<ItemDataSO> OnUnEquipRequest;
        public event Action<ItemDataSO> OnUseRequest;
        public event Action<ItemDataSO> OnDropRequest;

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

        [SerializeField] private DragSlotUI m_dragSlot;

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
            m_dragSlot.DragUI(_clickSlot);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_dragSlot.SlotUIBase == null) return;
            m_dragSlot.transform.position = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (m_dragSlot.SlotUIBase == null) return;
            m_dragSlot.transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SlotUIBase _dropSlotUI = GetUnderPointerSlot(eventData.position);
            if (_dropSlotUI == null)
            {
                m_dragSlot.Clear();
            }
            /*SlotUIBase _clickSlot = GetUnderPointerSlot(eventData.position);
            if (_clickSlot == m_dragSlot && _clickSlot.SlotIndex == m_dragSlot.SlotUIBase.SlotIndex) m_dragSlot.Clear();*/
        }

        // 드래그 이후에만 동작됨
        public void OnEndDrag(PointerEventData eventData)
        {
            SlotUIBase _dropSlotUI = GetUnderPointerSlot(eventData.position);
            if (m_dragSlot.SlotUIBase == null || _dropSlotUI == null)
            {
                m_dragSlot.Clear();
                return;
            }

            // 아이템이 타슬롯과의 매칭이 맞는지 확인 및 변경
            bool _isDragDrop = OnDragDrop.Invoke(m_dragSlot.SlotUIBase, _dropSlotUI);

            if (!_isDragDrop)
            {
                Debug.Log("슬롯이 맞지 않습니다.");
            }

            m_dragSlot.Clear();

            // 원본(드래그 시작 슬롯) 데이터 복사 (안전하게 로컬에 저장)
            /*SlotUIInfos _dragSlotInfo = m_dragSlot.SlotInfo;
            SlotUIBase _pdSlot = m_pointerDownSlot;

            if (_pdSlot == null)
            {
                CleanupDrag();
                return;
            }

            // OnEndDrag에서 PointerUp한 타겟슬롯 저장
            SlotUIBase _targetSlot = GetSlotUnderPointer(eventData.position);

            // 1) UI 밖으로 드롭 -> 삭제/팝업 처리
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (_pdSlot.SlotInfo.ItemData != null)
                    UnEquipItem(_targetSlot, m_dragSlot.SlotInfo.ItemData); // 실제 오브젝트 제거

                // 예: 삭제 확정 팝업 로직 / 바로 삭제 로직
                // 여기서는 바로 삭제 (원본 슬롯 비움)
                _pdSlot.ClearSlot();
                CleanupDrag();
                return;
            }

            // 2) 아무 슬롯에도 닿지 않았거나 동일 슬롯이면 취소
            if (_targetSlot == null || _targetSlot == _pdSlot)
            {
                CleanupDrag();
                return;
            }

            // 3) 유효성 검사: 양쪽 슬롯이 상대 아이템을 받는지 확인
            //    예: target.CanAcceptItem(srcItem) && srcSlot.CanAcceptItem(targetItem)
            ItemDataSO _dragItemData = _dragSlotInfo.ItemData;
            ItemDataSO _targetItemData = _targetSlot.SlotInfo.ItemData;

            // 기본적으로 target이 src의 아이템을 받을 수 있는지 체크
            if (!_targetSlot.CanAcceptItem(_dragItemData))
            {
                // 거부하면 원상복구. 즉, 다른 슬롯으로 드랍한 상태
                CleanupDrag();
                return;
            }

            // 스택 가능한 아이템 합치기 (같은 아이템 && countable)
            if (_targetSlot.IsEmpty && _targetItemData != null &&
                _dragItemData.ItemType == EItemTypes.Useable &&
                _targetItemData.ItemName == _dragItemData.ItemName)
            {
                // 합치기: target의 카운트 + src의 카운트
                int total = _targetSlot.SlotInfo.ItemCount + _pdSlot.SlotInfo.ItemCount;

                // 적용: target에 total 할당, source 클리어
                int targetIndex = GetSlotIndex(_targetSlot);
                _targetSlot.ApplySlotInfo(targetIndex, _targetSlot.SlotInfo.SlotIcon.sprite, total, _targetItemData);

                // 슬롯 판별하여 장착슬롯 -> 인벤토리일 경우 실제 오브젝트 제거
                UnEquipItem(_targetSlot, _dragItemData); 

                _pdSlot.ClearSlot();
                CleanupDrag();
                return;
            }

            // 기존 장착 해제 (원본 슬롯)
            if (_pdSlot.SlotInfo.ItemData != null)
                UnEquipItem(_pdSlot, _pdSlot.SlotInfo.ItemData);

            // 4) 스왑 혹은 이동 로직
            // 만일 targetSlot이 비어있다면 이동, 아니면 스왑
            if (!_targetSlot.IsEmpty)
            {
                // 이동: target <- src, src <- clear
                int targetIndex = GetSlotIndex(_targetSlot);
                _targetSlot.ApplySlotInfo(targetIndex, _dragSlotInfo.SlotIcon.sprite, _dragSlotInfo.ItemCount, _dragItemData);
                _pdSlot.ClearSlot();

                // Equip슬롯인지 판단 후 정보 전달
                EquipItem(_targetSlot, _targetSlot.SlotInfo.ItemData);
            }
            else
            {
                // 스왑: 단자성(atomic) 보장
                //  - 임시로 두 SlotInfos 보관
                SlotUIInfos targetInfoCopy = _targetSlot.SlotInfo;        // struct copy
                SlotUIInfos srcInfoCopy = _dragSlotInfo;                  // 이미 복사되어 있음

                // 인덱스(슬롯 넘버)는 슬롯의 실제 인덱스로 재설정
                int srcIndex = GetSlotIndex(_pdSlot);
                int targetIndex2 = GetSlotIndex(_targetSlot);

                // 서로 적용 (주의: ApplySlotInfo은 slotNum, sprite, count, item 주입)
                _pdSlot.ApplySlotInfo(srcIndex, targetInfoCopy.SlotIcon.sprite, targetInfoCopy.ItemCount, targetInfoCopy.ItemData);
                _targetSlot.ApplySlotInfo(targetIndex2, srcInfoCopy.SlotIcon.sprite, srcInfoCopy.ItemCount, srcInfoCopy.ItemData);

                // Equip슬롯인지 판단 후 정보 전달
                EquipItem(_targetSlot, srcInfoCopy.ItemData);
            }

            CleanupDrag();*/
        }

        // TODO : 판단된 정보를 받는곳에서 다시 판단할 필요 없이 바로 사용할 수 있게 하는 방법 적용
        private void EquipItem(SlotUIBase targetSlot, ItemDataSO itemData)
        {
            // 슬롯 타입에 따라 이벤트 호출
            if (targetSlot is WeaponQuickSlotUI || targetSlot is ArmorSlotUI || targetSlot is UseableQuickSlotUI)
            {
                OnEquipRequest?.Invoke(itemData);
            }
        }

        private void UnEquipItem(SlotUIBase targetSlot, ItemDataSO itemDataSO)
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
