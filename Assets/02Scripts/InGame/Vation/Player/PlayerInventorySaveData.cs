using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventorySaveData
{
    public List<string> ItemIDList = new List<string>();

    public PlayerInventorySaveData(List<Item> itemList)
    {
        foreach (var item in itemList)
            ItemIDList.Add(item.Data.ID);
    }
}
