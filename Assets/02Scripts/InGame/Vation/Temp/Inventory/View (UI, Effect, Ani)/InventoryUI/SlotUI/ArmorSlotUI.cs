using alpha;
using Unity.Collections;
using UnityEngine;

namespace alpha
{

    public class ArmorSlotUI : SlotUIBase
    {
        [Header("[ Armor ]")]
        public EArmorTypes ArmorType;

#if UNITY_EDITOR

        private void OnValidate()
        {
            SlotItemType = EItemTypes.Armor;
        }
#endif
    }
}