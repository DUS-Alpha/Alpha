using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackDataSO))]
public class AttackDataSOEditor : Editor
{
    SerializedProperty Name;
    SerializedProperty ID;
    SerializedProperty AttackType;
    SerializedProperty DamageType;
    SerializedProperty WeaponRequirementNum;
    SerializedProperty HitEffectPrefab;

    SerializedProperty Damage;
    SerializedProperty Distance;
    SerializedProperty Radius;
    SerializedProperty StartDelay;
    SerializedProperty Duration;
    SerializedProperty Cooldown;

    SerializedProperty IsSkill;

    SerializedProperty SkillType;
    SerializedProperty SkillCategory;

    private void OnEnable()
    {
        Name = serializedObject.FindProperty("Name");
        ID = serializedObject.FindProperty("ID");
        AttackType = serializedObject.FindProperty("AttackType");
        DamageType = serializedObject.FindProperty("DamageType");
        WeaponRequirementNum = serializedObject.FindProperty("WeaponRequirementNum");
        HitEffectPrefab = serializedObject.FindProperty("HitEffectPrefab");

        Damage = serializedObject.FindProperty("Damage");
        Distance = serializedObject.FindProperty("Distance");
        Radius = serializedObject.FindProperty("Radius");
        StartDelay = serializedObject.FindProperty("StartDelay");
        Duration = serializedObject.FindProperty("Duration");
        Cooldown = serializedObject.FindProperty("Cooldown");

        IsSkill = serializedObject.FindProperty("IsSkill");

        SkillType = serializedObject.FindProperty("SkillType");
        SkillCategory = serializedObject.FindProperty("SkillCategory");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 기존 필드 사용(Tooltip도 같이 적용됨)
        EditorGUILayout.PropertyField(Name);
        EditorGUILayout.PropertyField(ID);
        EditorGUILayout.PropertyField(AttackType);
        EditorGUILayout.PropertyField(DamageType);
        EditorGUILayout.PropertyField(WeaponRequirementNum);
        EditorGUILayout.PropertyField(HitEffectPrefab);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(Damage);
        EditorGUILayout.PropertyField(Distance);
        EditorGUILayout.PropertyField(Radius);
        EditorGUILayout.PropertyField(StartDelay);
        EditorGUILayout.PropertyField(Duration);
        EditorGUILayout.PropertyField(Cooldown);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(IsSkill);

        if(IsSkill.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(SkillType);
            EditorGUILayout.PropertyField(SkillCategory);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
        // 대상 스크립트 참조
        /*AttackDataSO data = (AttackDataSO)target;
        data.IsSkill = EditorGUILayout.Toggle(data.IsSkill);
        if (data.IsSkill)
        {
            EditorGUI.indentLevel++;
            //data.SkillType = (SkillTypes)EditorGUILayout.EnumPopup("Skill AttackType", data.SkillType);
            //data.SkillCategory = (SkillCategorys)EditorGUILayout.EnumPopup("Skill Category", data.SkillCategory);
            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }*/

        // 직접 편집
        /*data.Name = EditorGUILayout.TextField("Name", data.Name);
        data.ID = EditorGUILayout.IntField("ID", data.ID);
        data.WeaponRequirementNum = EditorGUILayout.IntField("WeaponRequirementNum", data.WeaponRequirementNum);
        data.AttackType = (AttackTypes)EditorGUILayout.EnumPopup("AttackType", data.AttackType);
        data.DamageType = (DamageTypes)EditorGUILayout.EnumPopup("DamageType", data.DamageType);
        data.HitEffectPrefab = (GameObject)EditorGUILayout.ObjectField("Hit Effect Prefab", data.HitEffectPrefab, typeof(GameObject), false);
        //data.UseEffectPrefab = (GameObject)EditorGUILayout.ObjectField("Use Effect Prefab", data.UseEffectPrefab, typeof(GameObject), false);

        data.Damage = EditorGUILayout.Slider("Damage", data.Damage, 0, 1000);
        data.Cooldown = EditorGUILayout.FloatField("Cooldown", data.Cooldown);

        if (data.IsSkill)
        {
            EditorGUI.indentLevel++;
            data.SkillType = (SkillTypes)EditorGUILayout.EnumPopup("Skill AttackType", data.SkillType);
            data.SkillCategory = (SkillCategorys)EditorGUILayout.EnumPopup("Skill Category", data.SkillCategory);
            EditorGUI.indentLevel--;
        }

        // 변경 사항 반영
        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }*/
    }
}
