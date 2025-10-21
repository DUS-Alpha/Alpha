using UnityEngine;

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

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/Skill/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public SkillTypes SkillType;
    public SkillCategorys SkillCategory;

    public GameObject EffectPrefab;
}
