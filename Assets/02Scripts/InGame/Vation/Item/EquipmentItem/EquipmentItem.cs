using alpha;
using UnityEngine;

namespace alpha
{
    public abstract class EquipmentItem : Item
    {
        public EquipmentItemDataSO EquipData => (EquipmentItemDataSO)Data;
    }
}