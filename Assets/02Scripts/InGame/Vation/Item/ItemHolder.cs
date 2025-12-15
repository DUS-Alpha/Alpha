using UnityEngine;

public enum HolderTypes
{
    None,
    Head,
    UpperBody,
    LowerBody,
    Gloves,
    Boots,
    Melee,
    MainRange,
    SubRange,
    Countable_Potion,
    Countable_Consumable
}

public class ItemHolder : MonoBehaviour
{
    public HolderTypes HolderType;
}
