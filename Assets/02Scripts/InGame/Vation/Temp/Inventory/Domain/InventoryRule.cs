using UnityEngine;

namespace alpha
{
    public class InventoryRule
    {
        public bool CanAdd()
        {
            // 공격중x, 슬롯 스택 제한, 등등
            return true;
        }
        public bool CanRemove() 
        { 
            return true; 
        }

        public bool CanOpenInventory()
        {
            // 인벤토리 키값, 등
            return true;
        }


    }
}