using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace alpha
{
    public class InventoryUI : BaseUI, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("[ UI Settings ]")]
        [Range(0, 10)]
        [SerializeField] private int m_verticalSlotCount = 4;      // 슬롯 세로 개수
        private int m_horizontalSlotCount = 4;  // 슬롯 가로 개수(인스펙터에서 간격에 의한 설정이기에)

        [Header("[ Connected Objects ]")]
        [SerializeField] private RectTransform m_contentRt;
        [SerializeField] private GameObject m_slotPrefab;

        [Header("[ Slots ]")]
        public List<InventorySlot> InventorySlotList;

        [SerializeField] private DragSlot m_dragSlot;

        private bool m_isEmptySlotClick;

        private void Awake()
        {
            Initialize();
            
            InventorySlotList = new List<InventorySlot>();

            InventorySlot[] _slots = GetComponentsInChildren<InventorySlot>();

            foreach (InventorySlot slot in _slots)
            {
                InventorySlotList.Add(slot);
            }
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
            m_dragSlot.gameObject.SetActive(false);
        }

        public void AddItem(ItemDataSO itemInfo)
        {
            for (int i = 0; i < InventorySlotList.Count; i++)
            {
                if (InventorySlotList[i].SlotIcon.gameObject.activeSelf)
                {
                    if (InventorySlotList[i].SavedItemInfoSO != null && InventorySlotList[i].SavedItemInfoSO.Name == itemInfo.Name) //중복 체크
                    {
                        InventorySlotList[i].ItemCount++;
                        break;
                    }
                    else continue;  // 위 조건 제외 빈공간 아닐경우 다음 슬롯으로
                }

                InventorySlotList[i].SlotIcon.sprite = itemInfo.Icon;
                InventorySlotList[i].ItemCount++;
                InventorySlotList[i].CountTMP.text = InventorySlotList[i].ItemCount.ToString();
                InventorySlotList[i].SavedItemInfoSO = itemInfo;
                InventorySlotList[i].SlotIcon.gameObject.SetActive(true);
                break;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            for (int i = 0; i < InventorySlotList.Count; i++)
            {
                if (InventorySlotList[i].IsInRect(eventData.position))
                {
                    if (InventorySlotList[i].SlotIcon.gameObject.activeSelf == true)    // 슬롯에 아이템 있을 때
                    {
                        m_dragSlot.transform.position = eventData.position;
                        m_dragSlot.gameObject.SetActive(true);

                        // InventorySlot Info -> Drag Info Save
                        m_dragSlot.SavedIcon.sprite = InventorySlotList[i].SlotIcon.sprite;
                        m_dragSlot.SavedItemCount = InventorySlotList[i].ItemCount;
                        m_dragSlot.SavedCountTMP.text = InventorySlotList[i].ItemCount.ToString();
                        m_dragSlot.SavedSlotNum = i;
                        m_dragSlot.SavedItemInfo = InventorySlotList[i].SavedItemInfoSO;

                        m_isEmptySlotClick = false;
                        break;
                    }
                }
                else
                {
                    m_isEmptySlotClick = true;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_isEmptySlotClick) return;
            m_dragSlot.transform.position = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (m_isEmptySlotClick) return;
            m_dragSlot.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (m_isEmptySlotClick)
            {
                m_isEmptySlotClick = false;
                return;
            }

            int _targetIndex = -1;

            // 1) 드롭된 슬롯(대상)을 먼저 찾음
            for (int i = 0; i < InventorySlotList.Count; i++)
            {
                if (InventorySlotList[i].IsInRect(eventData.position))
                {
                    _targetIndex = i;
                    break;
                }
            }

            // 2) 대상 슬롯이 없으면 드래그 슬롯 초기화(원상복구 또는 취소 동작)
            if (_targetIndex == -1)
            {
                // 선택: 원래 위치로 되돌리거나 단순히 초기화
                // 현재 코드는 InitializeDragSlot()로 드래그 정보 초기화
                m_dragSlot.InitializeDragSlot();
                return;
            }

            // 3) 대상 슬롯이 있으면 한 번만 처리
            int _prevSlotIndex = m_dragSlot.SavedSlotNum;

            if (InventorySlotList[_targetIndex].SlotIcon.gameObject.activeSelf) // 대상이 빈 슬롯이 아닐 경우(이미 아이템 있음) -> 스왑
            {
                // 장비 슬롯에 두었을 경우
                
                // 이전 슬롯에 대상 슬롯의 정보 덮어쓰기 (swap)
                InventorySlotList[_prevSlotIndex].SlotIcon.sprite = InventorySlotList[_targetIndex].SlotIcon.sprite;
                InventorySlotList[_prevSlotIndex].ItemCount = InventorySlotList[_targetIndex].ItemCount;
                InventorySlotList[_prevSlotIndex].CountTMP.text = InventorySlotList[_targetIndex].ItemCount.ToString();
                InventorySlotList[_prevSlotIndex].SavedItemInfoSO = InventorySlotList[_targetIndex].SavedItemInfoSO;
            }
            else // 대상이 빈 슬롯이면 이전 슬롯은 비움
            {
                InventorySlotList[_prevSlotIndex].SlotIcon.gameObject.SetActive(false);
                InventorySlotList[_prevSlotIndex].ItemCount = 0;
                InventorySlotList[_prevSlotIndex].CountTMP.text = "0";
                InventorySlotList[_prevSlotIndex].SavedItemInfoSO = null;
            }

            // 대상 슬롯에 드래그 아이템 적용
            InventorySlotList[_targetIndex].SlotIcon.gameObject.SetActive(true);
            InventorySlotList[_targetIndex].SlotIcon.sprite = m_dragSlot.SavedIcon.sprite;
            InventorySlotList[_targetIndex].ItemCount = m_dragSlot.SavedItemCount;
            InventorySlotList[_targetIndex].CountTMP.text = m_dragSlot.SavedItemCount.ToString();
            InventorySlotList[_targetIndex].SavedItemInfoSO = m_dragSlot.SavedItemInfo;

            // DragSlot 초기화
            m_dragSlot.InitializeDragSlot();
        }
    }
}
