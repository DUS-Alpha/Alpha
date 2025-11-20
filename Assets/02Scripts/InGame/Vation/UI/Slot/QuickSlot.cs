using UnityEngine;

namespace alpha
{
    public class QuickSlot : SlotBase
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            SlotType = ESlotTypes.Quick;
        }
#endif
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO info)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, info);
        }
        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            CountableItemDataSO _countableItemData = itemdata as CountableItemDataSO;

            if (_countableItemData == null) return false;
            if (_countableItemData.CountableType == ECountableTypes.Ammo) return false;
            if (_countableItemData.CountableType == ECountableTypes.CraftingMaterial) return false;
            return true;
        }
    }
}