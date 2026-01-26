using UnityEngine;

namespace alpha
{
    public class WeaponQuickSlot : SlotModuleBase
    {
        public override EItemTypes SlotItemType => EItemTypes.Weapon;
        public EWeaponTypes WeaponTypes;

        public WeaponQuickSlot(int slotIndex, EWeaponTypes weaponTypes) : base(slotIndex)
        {
            WeaponTypes = weaponTypes;
        }

        /*public override bool CanAccept(ItemSO itemData)
        {
            if(itemData.ItemType == SlotItemType)
            {
                WeaponSO _weapon = (WeaponSO)itemData;
                if(_weapon.WeaponType == WeaponTypes) return true;
            }
            return false;
        }*/
    }
}