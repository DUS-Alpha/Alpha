using UnityEngine;

public static class ItemFactory
{
    public static Item CreateItem(ItemData data)
    {
        GameObject ItemObj = new GameObject(data.Name);
        Item ItemBase;

        if (data is WeaponData weaponData)
            ItemBase = ItemObj.AddComponent<Weapon>();
       /* else if (data is ArmorData armorData)
            ItemBase = ItemObj.AddComponent<Armor>();*/
        else
            ItemBase = ItemObj.AddComponent<Item>();

        ItemBase.Initialize(data);
        return ItemBase;
    }
}
