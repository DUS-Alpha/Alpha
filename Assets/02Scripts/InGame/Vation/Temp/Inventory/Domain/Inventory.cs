using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    // 도메인
    // Aggregate (행위 소유)
    public class Inventory
    {
        InventoryRule m_rule;

        private List<ItemModuleBase> itemList;
        public void Bind(InventoryRule rule)
        {
            m_rule = rule;
        }

        public void TryAdd(ItemModuleBase item)
        {
            if (!m_rule.CanAdd()) return;

            itemList.Add(item);
        }

        public void TryRemove()
        {
            // 인벤토리 지워지기
        }

        // 아이템 강탈
        public void TakeAwayItem()
        {

        }
    }
}
