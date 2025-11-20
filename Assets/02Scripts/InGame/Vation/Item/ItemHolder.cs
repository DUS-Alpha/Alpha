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
    Quick_Potion,
    Quick_Consumable
}

public class ItemHolder : MonoBehaviour
{
    public HolderTypes HolderType;
}
