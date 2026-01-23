using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSkill", menuName = "Scriptable Objects/Skill/Attack/MeleeSkill")]
public class MeleeSkillDataSO : AttackSkillData
{
    // 상속시 EquipmentType값 자동 설정
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        WeaponType = alpha.EWeaponTypes.Melee;
        // 부모에서는 아무것도 안함 (자식에서 설정)
    }
#endif
}
