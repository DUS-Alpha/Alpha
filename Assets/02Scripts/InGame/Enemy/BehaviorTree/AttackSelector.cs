using UnityEngine;

public class AttackSelector : MonoBehaviour
{
    // [Header("공격 트리거들 (지금은 랜덤)")]
    public string[] triggers;

    // 지금은 랜덤 → 나중에 이 함수를 가중치 버전으로 교체
    public int PickIndexRandom()
    {
        if (triggers == null || triggers.Length == 0) return -1;
        return Random.Range(0, triggers.Length);
    }

    public string GetTrigger(int idx) => triggers[idx];
}