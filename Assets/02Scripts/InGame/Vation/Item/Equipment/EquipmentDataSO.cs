using UnityEngine;

public enum ApplicableSlots
{
    Head,
    Chest,
    Feets,
    Hands,
    MeleeWeapon,
    RifleWeapon,
    SniperWeapon
}

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Scriptable Objects/EquipmentDataSO")]
public abstract  class EquipmentDataSO : ItemDataSO
{
    [Header("[ Equipment Info ]"), Space(10)]
    public ApplicableSlots ApplicableSlot; // 적용 슬롯
    public int UsageLevel;      // 착용 레벨
    public float Weight;
    [Tooltip("내구성")]
    public float Durability;
    //public StatModifier[] StatModifiers; // 공격력, 방어력, 크리티컬 등*/
}
