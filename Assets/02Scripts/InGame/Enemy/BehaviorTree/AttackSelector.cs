using UnityEngine;
using Combat;

public class AttackSelector : MonoBehaviour
{
    public string[] FarTriggers;
    public string[] MidTriggers;
    public string[] CloseTriggers;

    // range → 해당 배열
    private string[] GetArray(CombatRange range)
    {
        switch (range)
        {
            case CombatRange.Far:   return FarTriggers;
            case CombatRange.Mid:   return MidTriggers;
            case CombatRange.Close: return CloseTriggers;
        }
        return null;
    }
    
    // (권장) 인덱스 말고 문자열을 바로 뽑는 편의 함수
    public string PickTriggerRandom(CombatRange range)
    {
        var arr = GetArray(range);
        if (arr == null || arr.Length == 0) return null;
        return arr[Random.Range(0, arr.Length)];
    }
}