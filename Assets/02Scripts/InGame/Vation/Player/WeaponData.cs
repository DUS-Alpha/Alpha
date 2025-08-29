using UnityEngine;

// TODO : 장비관련해서는 차후에
public enum WeaponType
{
    Melee,
    Range,
}
[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/Equipments/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Tooltip("[ 설명 ]"),SerializeField]
    private string m_description;
    [SerializeField]
    private WeaponType m_weaponType;
    [SerializeField]
    private GameObject m_weapon;
    [SerializeField]
    private int m_id;
    [SerializeField]
    private string m_name;
    [SerializeField]
    private int m_attackDamage;
    [SerializeField]
    private float m_attackDelay;
}
