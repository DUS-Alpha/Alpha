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
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO itemData)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, itemData);
        }
        public override bool CanAcceptItem(ItemDataSO itemData)
        {
            CountableItemDataSO _countableItemData = itemData as CountableItemDataSO;

            if (_countableItemData == null) return false;
            if (_countableItemData.CountableType == ECountableTypes.Ammo) return false;
            if (_countableItemData.CountableType == ECountableTypes.CraftingMaterial) return false;
            return true;
        }
    }
}