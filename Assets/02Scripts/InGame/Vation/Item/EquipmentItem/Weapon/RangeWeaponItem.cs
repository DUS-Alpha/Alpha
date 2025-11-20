using UnityEngine;

namespace alpha
{
    public class RangeWeaponItem : WeaponItem
    {
        RangeWeaponItemDataSO RangeWeaponItemData;
        public RangeWeaponItem(RangeWeaponItemDataSO data) : base(data)
        {
            RangeWeaponItemData = data;
        }
    }
}