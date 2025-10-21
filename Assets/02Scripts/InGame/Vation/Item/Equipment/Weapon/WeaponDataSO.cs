using UnityEngine;

// TODO : 장비관련해서는 차후에
public enum WeaponTypes
{
    Melee,
    MainRange,
    SubRange
}

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Scriptable Objects/Item/WeaponDataSO")]
public class WeaponDataSO : EquipmentDataSO
{
    [Header("[ MeleeWeapon Info ]"), Space(10)]

    public WeaponTypes WeaponType;
    public CombatDataStatus CombatData;
}
