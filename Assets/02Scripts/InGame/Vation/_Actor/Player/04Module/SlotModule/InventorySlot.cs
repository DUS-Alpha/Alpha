using NUnit.Framework.Interfaces;
using UnityEngine;

namespace alpha
{
    public class InventorySlot : SlotModuleBase
    {
        public InventorySlot(int slotIndex) : base(slotIndex){}

        /*public override bool CanAccept(ItemSO itemData)
        {
            if (!IsEmpty)
            {
                if(!itemData.IsStackable || CurrentItemData.ItemName != itemData.ItemName )
                {
                    return false;
                }
            }
            return true;
        }*/
    }
}