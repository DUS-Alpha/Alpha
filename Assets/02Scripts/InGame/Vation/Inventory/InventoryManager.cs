using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace alpha
{
    // 로직과 계산을 담당하는 인벤토리 코어
    public class InventoryManager : MonoBehaviour, IInventoryService
    {
        [Range(0, 10)]
        public int VerticalInventorySlotCount => m_verticalSlotCount;
        [SerializeField] private int m_verticalSlotCount = 4;       // 슬롯 세로 개수
        public int HorizontalInventorySlotCount { get; private set; } = 4;                      // 슬롯 가로 개수(인스펙터에서 간격에 의한 설정이기에)

        public List<InventorySlot> InventorySlotList;
        public List<ArmorSlot> ArmorSlotList;
        public List<WeaponQuickSlot> WeaponQuickSlotList;
        public List<UseableQuickSlot> UseableQuickSlotList;

        public event Action<ISlotModel> OnUpdateSlotUI;

        // 장비
        public event Action<ItemDataSO> OnEquipItem;
        public event Action<ItemDataSO> OnUnEquipItem;
        private void Awake()
        {
            // 슬롯 생성 및 초기화
            InventorySlotList = new List<InventorySlot>();
            ArmorSlotList  = new List<ArmorSlot>()
            {
                new ArmorSlot(0, EArmorTypes.Head),
                new ArmorSlot(1, EArmorTypes.UpperBody),
                new ArmorSlot(2, EArmorTypes.LowerBody),
                new ArmorSlot(3, EArmorTypes.Gloves),
                new ArmorSlot(4, EArmorTypes.Boots),
            };

            WeaponQuickSlotList = new List<WeaponQuickSlot>()
            { 
                new WeaponQuickSlot(0, EWeaponTypes.Melee),
                new WeaponQuickSlot(1, EWeaponTypes.MainRange),
                new WeaponQuickSlot(2, EWeaponTypes.SubRange),
            };

            UseableQuickSlotList = new List<UseableQuickSlot>()
            {
                new UseableQuickSlot(0),
            };
        }

        // 인벤토리 슬롯 확장
        public ISlotModel ExpandInventorySlot()
        {
            InventorySlotList.Add(new InventorySlot(InventorySlotList.Count));
            // Count 증가된 상태이기에 -1
            return InventorySlotList[InventorySlotList.Count - 1];
        }

        // 아이템 획득 => PickupItem Class에서 호출
        public bool AddItemToInventory(ItemDataSO itemData)
        {
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
            return false;
        }

        // View에서 이벤트 발생 (타슬롯으로 드래그 & 드롭)
        public bool ExecuteDragDrop(EItemTypes fromType, int fromIndex, EItemTypes toType, int toIndex)
        {
            if(fromType == toType && fromIndex == toIndex) return true;

            SlotBase _dragSlot = GetSlot(fromType, fromIndex);
            SlotBase _dropSlot = GetSlot(toType, toIndex);

            //드래그한 슬롯에 현재 드랍한 슬롯이 허용되는지
            
            bool _isInventorySlot = fromType == EItemTypes.None && toType == EItemTypes.None;

            if (_isInventorySlot || _dropSlot.CanAccept(_dragSlot.CurrentItemData))
            {
                DragDropSlot(_dragSlot, _dropSlot);
                return true;
            }

            return false;
        }

        private SlotBase GetSlot(EItemTypes type, int index)
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

                if (dragSlot.SlotItemType != EItemTypes.None)
                {
                    OnUnEquipItem?.Invoke(dragSlot.CurrentItemData);
                }
                if (_isDragClear) dragSlot.ClearSlot();
                else dragSlot.SwapSlot(_dropSlotCopyInfo);

                if (dropSlot.SlotItemType != EItemTypes.None)
                {
                    // 실제 장비 착용 처리
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

        // 아이템 사용

    }
}