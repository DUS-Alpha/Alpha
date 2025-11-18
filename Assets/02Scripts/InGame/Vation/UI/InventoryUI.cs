using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace alpha
{
    public class InventoryUI : BaseUI, IPointerDownHandler, IPointerUpHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("[ UI Settings ]")]
        [Range(0, 10)]
        [SerializeField] private int m_verticalSlotCount = 4;      // 슬롯 세로 개수
        private int m_horizontalSlotCount = 4;  // 슬롯 가로 개수(인스펙터에서 간격에 의한 설정이기에)

        [Header("[ Connected Objects ]")]
        
        [SerializeField] private RectTransform m_contentRt;
        [SerializeField] private GameObject m_slotPrefab;

        [Header("[ Slots ]")]
        public List<SlotBase> InventorySlotList = new();
        public List<SlotBase> ArmorSlotList = new();
        public List<SlotBase> WeaponSlotList = new();
        public List<SlotBase> QuickSlotList = new();
        private SlotBase m_pointerDownSlot;

        [SerializeField] private DragSlot m_dragSlot;
        private bool m_isSlotClick;
        [Space(10)]

        [Header("[ PopUpUI ]")]
        [SerializeField] private Transform m_popUITr;
        [SerializeField] private GameObject m_confirmUI;
        [SerializeField] private GameObject m_amountInputUI;
        private GameObject currentPopUpUI;
        
        private void Awake()
        {
            Initialize();

            // 슬롯별 리스트 저장
            InventorySlot[] _inventorySlots = GetComponentsInChildren<InventorySlot>();
            foreach (InventorySlot slot in _inventorySlots) InventorySlotList.Add(slot);
            
            ArmorSlot[] _armorSlots = GetComponentsInChildren<ArmorSlot>();
            foreach (ArmorSlot slot in _armorSlots) ArmorSlotList.Add(slot);
            
            WeaponSlot[] _weaponSlots = GetComponentsInChildren<WeaponSlot>();
            foreach (WeaponSlot slot in _weaponSlots) WeaponSlotList.Add(slot);
            
            QuickSlot[] _quickSlots = GetComponentsInChildren<QuickSlot>();
            foreach (QuickSlot slot in _quickSlots) QuickSlotList.Add(slot);
        }

        private void Initialize()
        {
            int _n = m_verticalSlotCount * m_horizontalSlotCount;
            for (int i = 0; i < _n; i++)
            {
                Instantiate(m_slotPrefab, m_contentRt);
            }
        }

        private void Start()
        {
            m_dragSlot.transform.GetChild(0).gameObject.SetActive(false);
        }

        public void AddItem(ItemDataSO itemInfo)
        {
            for (int i = 0; i < InventorySlotList.Count; i++)
            {
                // 현재 슬롯에 아이템이 있는 경우
                if(InventorySlotList[i].HasItem)
                {
                    if (itemInfo.ItemType != EItemTypes.ConuntableItem) continue;
                    if (InventorySlotList[i].SlotInfo.ItemInfo.Name == itemInfo.Name) //중복 체크
                    {
                        AddToSlot(i, itemInfo);
                        return;
                    }
                }
                else
                {
                    CreateNewSlot(i, itemInfo);
                    return;
                }
            }
        }

        // Add,와 Create로 구분한 이유는 초기화가 없는 상황시 위험한 코드이기에 명확성을 위해 중복내용도 그대로 사용
        private void AddToSlot(int index, ItemDataSO itemInfo)
        {
            // 슬롯 저장
            var _slot = InventorySlotList[index];

            // 기존 스택 증가
            _slot.SlotInfo.ItemCount++;

            _slot.ApplySlotInfo(index, itemInfo.IconSprite, _slot.SlotInfo.ItemCount, itemInfo);
        }

        private void CreateNewSlot(int index, ItemDataSO itemInfo)
        {
            var _slot = InventorySlotList[index];

            // 신규 슬롯은 무조건 1로 명시
            _slot.ApplySlotInfo(index, itemInfo.IconSprite, 1, itemInfo);
        }
        private SlotBase GetSlotUnderPointer(Vector2 pos)
        {
            foreach (var slot in InventorySlotList)
                if (slot.IsInRect(pos)) return slot;

            foreach (var slot in ArmorSlotList)
                if (slot.IsInRect(pos)) return slot;

            foreach (var slot in WeaponSlotList)
                if (slot.IsInRect(pos)) return slot;

            foreach (var slot in QuickSlotList)
                if (slot.IsInRect(pos)) return slot;

            return null;
        }

        private int GetSlotIndex(SlotBase slot)
        {
            if (slot == null) return -1;
            int idx = InventorySlotList.IndexOf(slot);
            if (idx >= 0) return idx;
            idx = ArmorSlotList.IndexOf(slot);
            if (idx >= 0) return idx;
            idx = WeaponSlotList.IndexOf(slot);
            if (idx >= 0) return idx;
            idx = QuickSlotList.IndexOf(slot);
            if (idx >= 0) return idx;
            return -1;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_isSlotClick = false;

            // 클릭한 슬롯 확인
            SlotBase _clickSlot = GetSlotUnderPointer(eventData.position);
            if (_clickSlot == null || !_clickSlot.HasItem) return;

            // 드래그 시작 슬롯 저장
            m_pointerDownSlot = _clickSlot;

            // drag slot에 복사
            int _srcIndex = GetSlotIndex(_clickSlot);
            m_dragSlot.CopySlotInfo(_srcIndex, _clickSlot.SlotInfo);

            // 드래그 활성화 플래그
            m_isSlotClick = true;

            // 드래그 아이콘 활성 (DragSlot 내부에서 위치 / 보이기 처리)
            m_dragSlot.transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_isSlotClick) return;
            m_dragSlot.transform.position = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!m_isSlotClick) return;
            m_dragSlot.transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_isSlotClick) return;
            //m_dragSlot.ClearSlot();
        }
        // 드래그 종료 후 공통 정리
        private void CleanupDrag()
        {
            m_dragSlot.ClearSlot();
            m_isSlotClick = false;
            m_pointerDownSlot = null;
        }

        // 드래그 이후에만 동작됨
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!m_isSlotClick) return;

            // 원본(드래그 시작 슬롯) 데이터 복사 (안전하게 로컬에 저장)
            SlotInfos _dragSlotInfo = m_dragSlot.SlotInfo;
            SlotBase _pdSlot = m_pointerDownSlot;

            if (_pdSlot == null)
            {
                CleanupDrag();
                return;
            }

            // OnEndDrag에서 PointerUp한 타겟슬롯 저장
            SlotBase _targetSlot = GetSlotUnderPointer(eventData.position);

            // 1) UI 밖으로 드롭 -> 삭제/팝업 처리
            if (!EventSystem.current.IsPointerOverGameObject())
            {
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
            ItemDataSO _dragItem = _dragSlotInfo.ItemInfo;
            ItemDataSO _targetItem = _targetSlot.SlotInfo.ItemInfo;

            // 기본적으로 target이 src의 아이템을 받을 수 있는지 체크
            if (!_targetSlot.CanAcceptItem(_dragItem))
            {
                // 거부하면 원상복구. 즉, 다른 슬롯으로 드랍한 상태
                CleanupDrag();
                return;
            }

            // 스택 가능한 아이템 합치기 (같은 아이템 && countable)
            if (_targetSlot.HasItem && _targetItem != null &&
                _dragItem.ItemType == EItemTypes.ConuntableItem &&
                _targetItem.Name == _dragItem.Name)
            {
                // 합치기: target의 카운트 + src의 카운트
                int _total = _targetSlot.SlotInfo.ItemCount + _pdSlot.SlotInfo.ItemCount;

                // 적용: target에 total 할당, source 클리어
                int _targetIndex = GetSlotIndex(_targetSlot);
                _targetSlot.ApplySlotInfo(_targetIndex, _targetSlot.SlotInfo.SlotIcon.sprite, _total, _targetItem);

                _pdSlot.ClearSlot();
                CleanupDrag();
                return;
            }


            // 4) 스왑 혹은 이동 로직
            // 만일 targetSlot이 비어있다면 이동, 아니면 스왑
            if (!_targetSlot.HasItem)
            {
                // 이동: target <- src, src <- clear
                int targetIndex = GetSlotIndex(_targetSlot);
                _targetSlot.ApplySlotInfo(targetIndex, _dragSlotInfo.SlotIcon.sprite, _dragSlotInfo.ItemCount, _dragItem);
                _pdSlot.ClearSlot();
                CleanupDrag();
                return;
            }
            else
            {
                // 스왑: 단자성(atomic) 보장
                //  - 임시로 두 SlotInfos 보관
                SlotInfos targetInfoCopy = _targetSlot.SlotInfo; // struct copy
                SlotInfos srcInfoCopy = _dragSlotInfo;                // 이미 복사되어 있음

                // 인덱스(슬롯 넘버)는 슬롯의 실제 인덱스로 재설정
                int srcIndex = GetSlotIndex(_pdSlot);
                int targetIndex2 = GetSlotIndex(_targetSlot);

                // 서로 적용 (주의: ApplySlotInfo은 slotNum, sprite, count, item 주입)
                _pdSlot.ApplySlotInfo(srcIndex, targetInfoCopy.SlotIcon.sprite, targetInfoCopy.ItemCount, targetInfoCopy.ItemInfo);
                _targetSlot.ApplySlotInfo(targetIndex2, srcInfoCopy.SlotIcon.sprite, srcInfoCopy.ItemCount, srcInfoCopy.ItemInfo);

                CleanupDrag();
                return;
            }
        }

        public void InventoryPopUI()
        {
            // 마우스 포인터가 UI 위에 있는지 확인

        }

    }

}
