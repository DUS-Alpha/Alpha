using UnityEngine;

namespace alpha
{
    public class WeaponQuickSlot : SlotBase
    {
        public override EItemTypes SlotItemType => EItemTypes.Weapon;
        public EWeaponTypes WeaponTypes;

        public WeaponQuickSlot(int slotIndex, EWeaponTypes weaponTypes) : base(slotIndex)
        {
            WeaponTypes = weaponTypes;
        }

        public override bool CanAccept(ItemDataSO itemData)
        {
            if(itemData.ItemType == SlotItemType)
            {
                WeaponItemDataSO _weapon = (WeaponItemDataSO)itemData;
                if(_weapon.WeaponType == WeaponTypes) return true;
            }
            return false;
        }
    }
}