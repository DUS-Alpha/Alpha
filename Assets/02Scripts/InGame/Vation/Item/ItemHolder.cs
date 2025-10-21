using UnityEngine;

public enum HolderTypes
{
    None,
    Head,
    Chest,
    Gloves,
    Feets,
    Melee,
    MainRange,
    SubRange
}

public class ItemHolder : MonoBehaviour
{
    public HolderTypes HolderType;
}
