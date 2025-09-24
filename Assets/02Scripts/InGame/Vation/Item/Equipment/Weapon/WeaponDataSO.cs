using UnityEngine;

// TODO : 장비관련해서는 차후에
public enum WeaponTypes
{
    Hand,
    Melee,
    Range,
}
[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Scriptable Objects/WeaponDataSO")]
public class WeaponDataSO : EquipmentDataSO
{
    [Header("[ Weapon Info ]"),Space(10)]
    public WeaponTypes WeaponType;
    public int AttackDamage;
    public float AttackDelay;
}
