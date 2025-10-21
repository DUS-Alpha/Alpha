using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDataSO))]
public class WeaponDataSOEditor : Editor
{
    // 제외하고 싶은 부모 필드 목록
    /*private readonly string[] excludeFields = new string[]
    {
        "m_Script", // Unity 기본 필드
        "serialNumber",
        "id",
    };*/
    /*public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("[ WeaponDataSO Inspector ]", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // excludeFields에 포함된 변수는 숨기고 나머지는 모두 표시
        DrawPropertiesExcluding(serializedObject, excludeFields);

        serializedObject.ApplyModifiedProperties();
    }*/
}
