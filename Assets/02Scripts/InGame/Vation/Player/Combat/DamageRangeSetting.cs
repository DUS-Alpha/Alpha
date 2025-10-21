using UnityEngine;

public class DamageRangeSetting : MonoBehaviour
{
    public float MaxDistance;
    public float MinDistance;
    public float Radius;

    [Tooltip("현재 설정을 저장할 ScriptableObject")]
    public SkillDataSO skillAsset;

    private void OnDrawGizmos()
    {

    }
}
