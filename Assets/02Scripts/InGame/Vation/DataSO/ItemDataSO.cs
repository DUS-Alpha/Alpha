using UnityEngine;
public enum ItemTypes
{
    expendables,    // 소모품
    Equipment,
}

public enum ItemGrade
{
    Normal,
    Rare,
    Unique,
    Legend
}
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/Item/ItemDataSO")]
public abstract class ItemDataSO : ScriptableObject
{
    [Header("[ Item Info ]"), Space(10)]
    public string Description;
    public ItemTypes ItemType;
    public GameObject ItemPrefab;
    public long SerialNumber;
    public string ID;
    public string Name;
    public ItemGrade Grade;
    public Sprite Icon;

    [Space(10)]
    public bool IsEquip;


}
