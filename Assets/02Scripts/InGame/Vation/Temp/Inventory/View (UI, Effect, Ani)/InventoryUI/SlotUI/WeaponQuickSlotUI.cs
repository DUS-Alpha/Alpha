using UnityEngine;

namespace alpha
{
    public class WeaponQuickSlotUI : SlotUIBase
    {
        public EWeaponTypes WeaponType;
#if UNITY_EDITOR

        private void OnValidate()
        {
            SlotItemType = EItemTypes.Weapon;
        }
#endif
    }
}