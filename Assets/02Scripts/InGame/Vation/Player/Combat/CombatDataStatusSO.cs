using System;
using UnityEngine;

public enum AttackTypes
{
    Melee,
    Range,
    AoE    // 광역 (Area of Effect)
}

[CreateAssetMenu(fileName = "CombatStatusSO", menuName = "Scriptable Objects/CombatData/CombatStatusSO")]
public class CombatDataStatusSO : ScriptableObject
{
    public string CombatName;
    public int CombatID;
    public AttackTypes AttackType;

    [Range(0, 1000)]
    public float damage;

    [Min(0.1f)]
    public float cooldown;
}
