using alpha;
using UnityEngine;

namespace alpha
{
    public abstract class EquipmentItem : Item
    {
        public new EquipmentItemDataSO Data => base.Data as EquipmentItemDataSO;
        protected EquipmentItem(EquipmentItemDataSO data) : base(data){}
    }
}