using UnityEngine;

namespace alpha
{
    public class InventoryCore
    {
        public Inventory Inventory;
        public InventoryRule rule;
        public InventoryCore()
        {
            Inventory = new Inventory();
            rule = new InventoryRule();
        }
    }
}