using System;
using UnityEngine;

public enum AttackTypes
{
    Melee,
    Range,
    MeleeAoE,    // 광역 (Area of Effect)
    RangeAoE
}
public enum SkillTypes
{
    Active,
    Passive
}
public enum SkillCategorys
{
    Attack,
    Shield,
    Buff,
    Heal
}

[CreateAssetMenu(fileName = "AttackDataSO", menuName = "Scriptable Objects/CombatData/AttackDataSO")]
public class AttackDataSO : ScriptableObject
{
    public string Name;
    public int ID;
    public AttackTypes AttackType;
    public DamageTypes DamageType;
    [Tooltip("공격조건무기 [ 0:Melee, 1:MainRange, 2:SubRange ]")] // 추후에 3에 Tool도 함께
    public int WeaponRequirementNum;
    //public GameObject UseEffectPrefab;
    public GameObject HitEffectPrefab;
    [Space(10)]

    [Range(0, 1000)]
    public float Damage;
    [Range(0, 1000)]
    public float Distance;
    [Range(0, 1000)]
    public float Radius;
    public float StartDelay;
    public float Duration;

    [Min(0.1f)]
    public float Cooldown;

    public bool IsSkill;

    public SkillTypes SkillType;
    public SkillCategorys SkillCategory;
}
