using UnityEngine;

public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Hands,
    Weapon,
    Shield,
    Accessory1,
    Accessory2
}

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Objects/EquipmentData")]
public abstract  class EquipmentData : ItemData
{
    [Header("[ Equipment Info ]"), Space(10)]
    public EquipmentSlot Slot;
    public int UsageLevel;   // 착용 레벨
    public float Weight;
    [Tooltip("내구성")]
    public float Durability;
    //public StatModifier[] StatModifiers; // 공격력, 방어력, 크리티컬 등*/
}
