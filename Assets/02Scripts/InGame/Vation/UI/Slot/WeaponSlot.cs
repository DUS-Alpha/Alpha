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
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO itemData)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, itemData);

        }
        public override bool CanAcceptItem(ItemDataSO item)
        {
            WeaponItemDataSO _weaponItemData = item as WeaponItemDataSO;

            if (_weaponItemData == null) return false;

            if (WeaponSlotType == _weaponItemData.WeaponType)
                return true;

            return false;
        }
    }
}