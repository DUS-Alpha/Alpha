using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace alpha
{
    // 의도 전달
    // InputAdapter
    public class InventoryController : MonoBehaviour
    {
        private IInventory m_input;
        public event Action OnTakeAway;

        public void Bind(IInventory input)
        {
            m_input = input;
        }

        private void Update()
        {
            
        }

        public void OnClickSlot()
        {

        }

        public void OnDropSlot()
        {

        }

        /*// 아이템 획득 => PickupItem Class에서 호출
        public bool AddItemToInventory(ItemDataSO itemData)
        {
            var _reslutSlot = m_inventoryModel.TryAddItem(itemData);

            if(_reslutSlot == null) return false;

            // Service 외부(XML, Json 등)로 전달 될 슬롯 보내기

            // 인벤토리 슬롯에서 아이템 접근 여부 판단(중복 가능인지, 빈칸인지)
            foreach (var slot in InventorySlotList)
            {
                if (slot.CanAccept(itemData))
                {
                    slot.AddItem(itemData);
                    OnUpdateSlotUI?.Invoke(slot);
                    return true;
                }
            }
            return true;
        }*/

        // View에서 이벤트 발생 (타슬롯으로 드래그 & 드롭)
        public bool ExecuteDragDrop(EItemTypes fromType, int fromIndex, EItemTypes toType, int toIndex)
        {
            /*m_inventoryModel.ExecuteDragDrop(fromType, fromIndex, toType, toIndex);

            if (fromType == toType && fromIndex == toIndex) return true;

            SlotBase _dragSlot = GetSlot(fromType, fromIndex);
            SlotBase _dropSlot = GetSlot(toType, toIndex);

            //드래그한 슬롯에 현재 드랍한 슬롯이 허용되는지
            
            bool _isInventorySlot = fromType == EItemTypes.None && toType == EItemTypes.None;

            if (_isInventorySlot || _dropSlot.CanAccept(_dragSlot.CurrentItemData))
            {
                DragDropSlot(_dragSlot, _dropSlot);
                return true;
            }*/

            return false;
        }

        /*private SlotBase GetSlot(EItemTypes type, int index)
        {
            return type switch
            {
                EItemTypes.None => InventorySlotList[index],
                EItemTypes.Armor => ArmorSlotList[index],
                EItemTypes.Weapon => WeaponQuickSlotList[index],
                EItemTypes.Useable => UseableQuickSlotList[index],
            };
        }
        private SlotInfo GetCopySlotInfo(SlotBase slot)
        {
            SlotInfo copySlotInfo = new SlotInfo()
            {
                ItemData = slot.CurrentItemData,
                ItemCount = slot.CurrentItemCount,
            };
            return copySlotInfo;
        }

        private void DragDropSlot(SlotBase dragSlot, SlotBase dropSlot)
        {
            bool _isDragClear = dropSlot.IsEmpty;

            if (CanStack(dragSlot, dropSlot))
            {
                dropSlot.StackItem(dragSlot.CurrentItemCount);
                dragSlot.ClearSlot();
            }
            else
            {
                SlotInfo _dragSlotCopyInfo = GetCopySlotInfo(dragSlot);
                SlotInfo _dropSlotCopyInfo = GetCopySlotInfo(dropSlot);

                dropSlot.SwapSlot(_dragSlotCopyInfo);

                // 1. 이전 슬롯 실제 모델(장비) 해제 처리 먼저
                if (dragSlot.SlotItemType != EItemTypes.None)
                {
                    OnUnEquipItem?.Invoke(dragSlot.CurrentItemData);
                }

                // 2. 이전 슬롯 논리 모델(장비) 해제
                if (_isDragClear) dragSlot.ClearSlot();
                else dragSlot.SwapSlot(_dropSlotCopyInfo);

                // 3. 실제 모델(장비) 착용 처리
                if (dropSlot.SlotItemType != EItemTypes.None)
                {
                    OnEquipItem?.Invoke(dropSlot.CurrentItemData);
                }
            }

            UpdateDragDrop(dragSlot, dropSlot);
        }

        private void UpdateDragDrop(SlotBase dragSlot, SlotBase dropSlot)
        {
            OnUpdateSlotUI?.Invoke(dropSlot);
            OnUpdateSlotUI?.Invoke(dragSlot);
        }

        private bool CanStack(SlotBase dragSlot, SlotBase dropSlot)
        {
            if (dragSlot.IsEmpty || dropSlot.IsEmpty) return false;
            if (dragSlot.CurrentItemData != dropSlot.CurrentItemData) return false;
            if (!dropSlot.CurrentItemData.IsStackable) return false;

            return true;
        }

        // 아이템 버리기, 삭제
        public void DestoryItem(ItemDataSO itemData)
        {
            
        }

        // 아이템 사용*/

    }
}