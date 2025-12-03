using UnityEngine;
using UnityEngine.UI;

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
    Dash,       // Dodge(회피) 여부
    Recovery,   // 회복
    Buff,       // 강화
}

public enum ECastingTypes
{
    Active,
    Passive
}

public enum ESkillTargetTypes
{
    SingleTarget,       // 근접 단일 타겟
    ForwardFan,         // 근접 정면 타겟들
    AOE_UserCentic,     // Area Of Effect, 플레이어 중심 광역기
    AOE_Projectile,     // 투척 광역기
}

public class SkillDataBase : ScriptableObject
{
    [Header("[ Base ]")]
    public string SkillName;
    public EKeyTypes KeyType;
    public ESkillTypes SkillType;
    public ECastingTypes CastingType;
    public ESkillTargetTypes SkillTargetType;
    public Sprite Icon;
    public float CoolDown;
    [TextArea] public string Description;
}
