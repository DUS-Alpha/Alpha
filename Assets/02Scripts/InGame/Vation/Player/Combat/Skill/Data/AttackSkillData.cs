using alpha;
using UnityEngine;

public enum ESkillAttackTypes
{
    InPlaceAttack,    // 제자리 공격
    DashAttack,
    JumpAttack,
    ChargingAttack,
}

public class AttackSkillData : SkillDataBase
{
    [Header("[ AttackData ]")]
    public EWeaponTypes WeaponType;
    public ESkillAttackTypes SkillAttackType;
    public float Damage;
}
