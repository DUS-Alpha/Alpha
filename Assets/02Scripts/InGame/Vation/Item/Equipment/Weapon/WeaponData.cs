using UnityEngine;

// TODO : 장비관련해서는 차후에
public enum WeaponTypes
{
    Hand,
    Melee,
    Range,
}
[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : EquipmentData
{
    [Header("[ Weapon Info ]"),Space(10)]
    public WeaponTypes WeaponType;
    public int AttackDamage;
    public float AttackDelay;
}
