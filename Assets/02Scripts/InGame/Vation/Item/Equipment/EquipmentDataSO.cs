using UnityEngine;

public enum EquipTypes
{
    Head,
    Chest,
    Feets,
    Gloves,
    Weapon
}

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Scriptable Objects/Item/EquipmentDataSO")]
public abstract  class EquipmentDataSO : ItemDataSO
{
    [Header("[ Equipment Info ]"), Space(10)]
    public EquipTypes EquipType;    // 장비 타입
    public int UsageLevel;          // 착용 레벨
    public float Weight;
    [Tooltip("내구성")]
    public float Durability;
    //public StatModifier[] StatModifiers; // 공격력, 방어력, 크리티컬 등*/
}
