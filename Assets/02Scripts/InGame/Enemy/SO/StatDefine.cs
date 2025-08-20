using UnityEngine;

[CreateAssetMenu(fileName = "StatDefine", menuName = "Scriptable Objects/StatDefine")]
public class StatDefine : ScriptableObject
{
    [Header("Base Stats")] 
    public float baseHP;
    public float baseATK;
    public float baseMoveSpeed;
}
