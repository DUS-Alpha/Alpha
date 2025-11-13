using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    [System.Serializable]
    public class PlayerInventorySaveData
    {
        public List<string> ItemIDList = new List<string>();

        public PlayerInventorySaveData(List<ItemDataSO> itemList)
        {
            // foreach (var item in itemList)
            //ItemIDList.Add(item.Data.ID);
        }
    }
}