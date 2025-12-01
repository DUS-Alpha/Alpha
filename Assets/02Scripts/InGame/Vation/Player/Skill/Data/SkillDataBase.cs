using UnityEngine;

public enum EKeyTypes
{
    Q,
    E,
    Z,
    C
}

public enum ESkillTypes
{
    Attack,
    Buff,
}

public enum ECastingTypes
{
    Active,
    Passive
}

public enum EDamageTypes
{
    SingleTarget,
    ForwardFan,
    AOE_UserCentic,     // Area Of Effect, 광역기
    AOE_throw,          // 투척 광역
}

public class SkillDataBase : ScriptableObject
{
    public string SkillName;
    public EKeyTypes KeyType;
    public EDamageTypes DamageType;
    public string AniName;

    public float Damage;
    public float m_coolDown;
}
