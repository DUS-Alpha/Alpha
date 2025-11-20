using UnityEngine;

namespace alpha
{
    public class WeaponSlot : SlotBase
    {
        public EWeaponTypes WeaponSlotType;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SlotType = ESlotTypes.Weapon;
        }
#endif
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO info)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, info);

        }
        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            WeaponItemDataSO weaponItemDataSO = itemdata as WeaponItemDataSO;

            if (weaponItemDataSO == null) return false;

            if (WeaponSlotType == weaponItemDataSO.WeaponType)
                return true;

            return false;
        }
    }
}