using UnityEngine;
public enum ItemTypes
{
    expendables, // 소모품
    Tool,       // 곡괭이같은 도구
    Equip
}
public enum ItemGrade
{
    Normal,
    Rare,
    Unique,
    Legend
}
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public abstract class ItemDataSO : ScriptableObject
{
    [Header("[ Item Info ]"), Space(10)]
    public string Description;
    public ItemTypes ItemType;
    public GameObject ItemPrefab;
    public string ID;
    public string Name;
    public ItemGrade Grade;
    public Sprite Icon;
}
