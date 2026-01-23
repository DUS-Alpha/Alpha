using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    // 장비 창 전체 상태 + 규칙
    // 장비 창 역할 (논리)
    public class EquipModel : IEquipModel
    {
        public List<ArmorSlot> ArmorSlotList;
        public List<WeaponQuickSlot> WeaponQuickSlotList;
        public List<UseableQuickSlot> UseableQuickSlotList;

        public EquipModel()
        {
            ArmorSlotList = new List<ArmorSlot>()
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


    }
}