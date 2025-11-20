using alpha;
using UnityEngine;

namespace alpha
{
    public class MeleeWeaponItem : WeaponItem
    {
        public MeleeWeaponItemDataSO WeaponItemData { get; private set; }
        public MeleeWeaponItem(MeleeWeaponItemDataSO data) : base(data)
        {
            WeaponItemData = data;
        }

      
    }
}