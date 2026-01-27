using alpha;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : MonoBehaviour , IInventoryViewPort, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ==================== Ref
    private IInventoryPresenterPort m_presenterPort;

    // ==================== Config Data
    [Header("[ Inventory Config ]")]
    [SerializeField] private Transform m_scrollerView;
    [SerializeField] private Transform m_scrollerContent;
    [Space(10)]

    [SerializeField] private SlotView m_inventoryPrefab;
    [SerializeField] private SlotView m_equipSlotPrefab;
    [SerializeField] private SlotView m_quickSlotPrefab;
    [SerializeField] private DragSlotView m_dragView;

    // ==================== Runtime Data
    private List<SlotView> m_inventorySlotList = new List<SlotView>();
    private List<SlotView> m_equipSlotList = new List<SlotView>();
    private List<SlotView> m_quickSlotList = new List<SlotView>();


    public void Bind(IInventoryPresenterPort inventoryPresenterPort)
    {
        m_presenterPort = inventoryPresenterPort;
        m_presenterPort.OnCreateInventorySlotView += CreateInventorySlotView;
    }

    public void CreateInventorySlotView(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SlotView _inventorySlotView = Instantiate(m_inventoryPrefab, m_scrollerContent);
            m_inventorySlotList.Add(_inventorySlotView);
        }
    }

    private SlotView GetUnderPointerSlot(Vector2 pos)
    {
        foreach (var slotUI in m_inventorySlotList)
            if (slotUI.IsInRect(pos)) return slotUI;
        foreach (var slotUI in m_equipSlotList)
            if (slotUI.IsInRect(pos)) return slotUI;
        foreach (var slotUI in m_quickSlotList)
            if (slotUI.IsInRect(pos)) return slotUI;
        return null;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭 슬롯 정보 확인
        SlotView _clickSlot = GetUnderPointerSlot(eventData.position);

        // 리턴들
        if (_clickSlot == null || _clickSlot.IsEmpty) return;

        // DragView에 복사
        m_dragView.SetSlotView(_clickSlot.SlotInfo);

        // DragView
        m_dragView.gameObject.SetActive(true);
        m_dragView.transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }




}
